using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRSDKSharp;
using System.Text.RegularExpressions;

namespace iRSDKSharp
{
    public enum TrackLocation
    {
        irsdk_NotInWorld = -1,
        irsdk_OffTrack,
        irsdk_InPitStall,
        irsdk_AproachingPits,
        irsdk_OnTrack
    };
}

namespace WheelDisplayHostApp
{
    class iracing
    {
        // Constants
        private static Int32 fuelconslaps = 5; // how many laps over fuel consumption is averaged

        // API variables
        private iRacingSDK sdk;
        private Int32 carIdx;
        private Int32 lastSesInfoUpdate;

        // internal variables
        private Boolean init;
        private Int32 gear;
        private Int32 rpm;
        private Int32 speed;
        private Int32 fuel;
        private Int32 fuelneed;
        private Int32 lap;
        private Int32 lapsrem;
        private Int32 position;
        private TimeSpan delta;
        private TimeSpan prevlap;
        private SessionTypes sessiontype;
        private Single shiftindicator;
        private Boolean pitlimiter;

        private Single lastTickTrackPos = 0;
        private Double lastTickTime;
        private Double lapStartTime = -1;
        private Int32 trackLength;
        private TimeDelta timedelta;
        private Boolean lapTimeValid;
        private Single[] fuelcons;
        private Int32 fuelconsPtr;
        private Single fuelconsumption;

        // public interface
        public Boolean isInitialized { get { return init; } set { } }
        public Int32 Gear { get { return gear; } set { } }
        public Int32 RPM { get { return rpm; } set { } }
        public Int32 Speed { get { return speed; } set { } }
        public Int32 Fuel { get { return fuel; } set { } }
        public Int32 FuelNeeded { get { return fuelneed; } set { } }
        public Single fuelConsumption { get { return fuelconsumption; } set { } }
        public Int32 Lap { get { return lap; } set { } }
        public Int32 LapsRemaining { get { return lapsrem; } set { } }
        public Int32 Position { get { return position; } set { } }
        public TimeSpan LapTime { get { return new TimeSpan(0, 0, 0, (Int32)Math.Floor(lastTickTime - lapStartTime), (Int32)(((lastTickTime - lapStartTime) % 1) * 1000)); } set { } }
        public TimeSpan BestLap { get { if (timedelta != null) return timedelta.BestLap; else return new TimeSpan(); } set { } }
        public TimeSpan Delta { get { return delta; } set { } }
        public TimeSpan PreviousLap { get { return prevlap; } set { } }
        public Single ShiftIndicator { get { return shiftindicator; } set { } }
        public Boolean PitLimiter { get { return pitlimiter; } set { } }

        // public enums
        public enum SessionTypes 
        {
            invalid,
            practice,
            qualify,
            race
        }

        public iracing()
        {
            // Forcing US locale for correct string to float conversion
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        }

        public void initialize()
        {
            sdk = new iRacingSDK();
            sdk.Startup();

            // check connection
            if (sdk.IsConnected())
            {
                String yaml = sdk.GetSessionInfo();

                // caridx
                Int32 start = yaml.IndexOf("DriverCarIdx: ") + "DriverCarIdx: ".Length;
                Int32 end = yaml.IndexOf("\n", start);
                carIdx = Int32.Parse(yaml.Substring(start, end - start));

                // track length
                start = yaml.IndexOf("TrackLength: ") + "TrackLength: ".Length;
                end = yaml.IndexOf("km\n", start);
                String dbg = yaml.Substring(start, end - start);
                trackLength = (Int32)(Single.Parse(yaml.Substring(start, end - start)) * 1000);

                // session types
                RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
                MatchCollection sessionNums, sessionTypes;
                Regex optionRegex = new Regex(@"SessionNum: (\d+)", options);

                // Get matches of pattern in yaml
                sessionNums = optionRegex.Matches(yaml);

                optionRegex = new Regex(@"SessionType: (\w+)", options);
                sessionTypes = optionRegex.Matches(yaml);

                Int32 currentSessionNum = (Int32)sdk.GetData("SessionNum");

                // Iterate matches
                for (Int32 ctr = 0; ctr < Math.Min(sessionNums.Count, sessionTypes.Count); ctr++)
                {
                    if (Int32.Parse(sessionNums[ctr].Value.Substring(12)) == currentSessionNum)
                    {
                        switch (sessionTypes[ctr].Value.Substring(13).Trim())
                        {
                            case "Practice":
                                sessiontype = iracing.SessionTypes.practice;
                                break;
                            case "Qualify":
                                sessiontype = iracing.SessionTypes.qualify;
                                break;
                            case "Race":
                                sessiontype = iracing.SessionTypes.race;
                                break;
                            default:
                                sessiontype = iracing.SessionTypes.invalid;
                                break;
                        }
                    }      
                }

                // reset laptimes
                lapStartTime = (Double)sdk.GetData("ReplaySessionTime");
                lapTimeValid = false;

                // fuel consumption, last 5 lap rolling
                fuelcons = new Single[fuelconslaps];
                fuelconsPtr = 0;

                // init timedelta
                timedelta = new TimeDelta(trackLength);
                timedelta.SaveBestLap(carIdx);

                init = true;
            }
            else // retry next tick
            {
                init = false;
            }
        }

        public void updateData()
        {
            // Check if the SDK is connected
            if (sdk.IsConnected())
            {
                // telemetry
                gear = (Int32)sdk.GetData("Gear");
                rpm = (Int32)(Single)sdk.GetData("RPM");
                speed = (Int32)((Single)sdk.GetData("Speed") * 3.6);
                fuel = (Int32)((Single)sdk.GetData("FuelLevel"));
                fuelneed = 0; // TODO
                shiftindicator = (Single)sdk.GetData("ShiftIndicatorPct");

                Int32 enwarn = (Int32)sdk.GetData("EngineWarnings");
                if (((Int32)sdk.GetData("EngineWarnings") & 0x10) > 0)
                    pitlimiter = true;
                else
                    pitlimiter = false;

                lap = (Int32)sdk.GetData("Lap");
                lapsrem = (Int32)sdk.GetData("SessionLapsRemain");

                Double sessionTime = new Double();
                Boolean ontrk = (Boolean)sdk.GetData("IsOnTrack");

                if ((Boolean)sdk.GetData("IsOnTrack"))
                    sessionTime = (Double)sdk.GetData("SessionTime");
                else
                    sessionTime = (Double)sdk.GetData("ReplaySessionTime");

                if (carIdx >= 0) // skip things that require caridx if we don't have it
                {
                    Int32[] driverLaps = new Int32[64];
                    driverLaps = (Int32[])sdk.GetData("CarIdxLap");

                    Single[] driverTrkPos = new Single[64];
                    driverTrkPos = (Single[])sdk.GetData("CarIdxLapDistPct");

                    Int32 lapPointer = (Int32)Math.Floor((driverTrkPos[carIdx] % 1) * (trackLength / 10));

                    timedelta.Update(sessionTime, driverTrkPos);

                    if (driverTrkPos[carIdx] < 0.1 && lastTickTrackPos > 0.9)
                    {
                        Double distance = (1 - lastTickTrackPos) + driverTrkPos[carIdx];
                        Double time = sessionTime - lastTickTime;
                        Double tickCorrection = (1 - lastTickTrackPos) / distance;

                        // save lap time
                        if (lapTimeValid)
                        {
                            Double laptime = (sessionTime - (1 - tickCorrection) * time) - lapStartTime;
                            prevlap = new TimeSpan(0, 0, 0, (Int32)Math.Floor(laptime), (Int32)Math.Floor((laptime % 1) * 1000));

                            fuelcons[fuelconsPtr % fuelcons.Length] = (Single)sdk.GetData("FuelLevel");

                            // update fuel consumption after one full lap
                            if (fuelconsPtr > 0)
                            {
                                if (fuelconsPtr >= fuelcons.Length)
                                {
                                    Single[] consrate = new Single[fuelcons.Length-1];
                                    Int32 j = 0;
                                    for (int i = fuelconsPtr; i < fuelconsPtr+consrate.Length; i++)
                                    {
                                        consrate[j++] = fuelcons[(i + 1) % fuelcons.Length] - fuelcons[(i + 2) % fuelcons.Length];
                                    }
                                    fuelneed = (Int32)(fuelcons[fuelconsPtr % fuelcons.Length] - (lapsrem * consrate.Average()));
                                    fuelconsumption = consrate.Average();
                                }
                                else if (fuelconsPtr > 0)
                                {
                                    fuelneed = (Int32)(fuelcons[fuelconsPtr % fuelcons.Length] - (lapsrem * fuelcons[(fuelconsPtr - 1) % fuelcons.Length]));
                                    fuelconsumption = fuelcons[(fuelconsPtr - 1) % fuelcons.Length] - fuelcons[fuelconsPtr % fuelcons.Length];
                                }
                            }
                            fuelconsPtr++;
                        }

                        // start new lap
                        lapStartTime = sessionTime - (1 - tickCorrection) * time;
                        lapTimeValid = true;
                    }
                    else if(Math.Abs(driverTrkPos[carIdx] - lastTickTrackPos) > 0.1) {
                        // invalidate lap time if jumping too much
                        lapTimeValid = false;
                    }

                    // reset fuel consumption when in pits
                    TrackLocation[] trackSurface = (iRSDKSharp.TrackLocation[])sdk.GetData("CarIdxTrackSurface");
                    if (trackSurface[carIdx] == TrackLocation.irsdk_InPitStall)
                    {
                        fuelcons = new Single[5];
                        fuelconsPtr = 0;
                    }

                    lastTickTrackPos = driverTrkPos[carIdx]; // save for next tick
                    lastTickTime = sessionTime;

                    Int32[] driverCarIdx = new Int32[64];

                    if (sessiontype == SessionTypes.race)
                    {
                        // in race calculate who is infront using trackpct and lap number
                        for (Int32 i = 0; i < 64; i++)
                        {
                            driverTrkPos[i] += (Single)driverLaps[i];
                            driverCarIdx[i] = i;
                        }

                        Array.Sort(driverTrkPos, driverCarIdx);
                        Array.Reverse(driverCarIdx);
                        position = (Int32)(Array.IndexOf(driverCarIdx, carIdx) + 1);

                        delta = timedelta.GetDelta(carIdx, driverCarIdx[Math.Max(position - 2, 0)]);
                    }
                    else
                    {
                        if (sdk.Header.SessionInfoUpdate != lastSesInfoUpdate)
                        {
                            // parse position
                            String yaml = sdk.GetSessionInfo();

                            Int32 sessionmatch = yaml.IndexOf(" - SessionNum: " + ((Int32)sdk.GetData("SessionNum")).ToString());
                            Int32 carmatch = yaml.IndexOf("CarIdx: " + carIdx.ToString(), sessionmatch);
                            Int32 positionmatch = yaml.LastIndexOf("Position:", carmatch);
                            if (positionmatch < 0)
                                position = 0;
                            else
                                position = Int32.Parse(yaml.Substring(positionmatch + "Position:".Length, 2));
                        }

                        delta = timedelta.GetBestLapDelta(driverTrkPos[carIdx] % 1);
                    }
                }

                lastSesInfoUpdate = sdk.Header.SessionInfoUpdate;
            }
            else
            {
                init = false;
            }
        }
    }
}
