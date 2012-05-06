using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRSDKSharp;
using System.Text.RegularExpressions;

namespace WheelDisplayHostApp
{
    class iracing
    {
        // API variables
        private iRacingSDK sdk;
        private Int32 carIdx;

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

        private Single lastTickTrackPos = 0;
        private Double lastTickTime;
        private Double lapStartTime = -1;
        private Int32 trackLength;
        private TimeDelta timedelta;
        private Boolean lapTimeValid;
        private Double[] bestlap;
        private Double[] currentlap;

        // public interface
        public Boolean isInitialized { get { return init; } set { } }
        public Int32 Gear { get { return gear; } set { } }
        public Int32 RPM { get { return rpm; } set { } }
        public Int32 Speed { get { return speed; } set { } }
        public Int32 Fuel { get { return fuel; } set { } }
        public Int32 FuelNeeded { get { return fuelneed; } set { } }
        public Int32 Lap { get { return lap; } set { } }
        public Int32 LapsRemaining { get { return lapsrem; } set { } }
        public Int32 Position { get { return position; } set { } }
        public TimeSpan LapTime { get { return new TimeSpan(0, 0, 0, (Int32)Math.Floor(lastTickTime - lapStartTime), (Int32)(((lastTickTime - lapStartTime) % 1) * 1000)); } set { } }
        public TimeSpan Delta { get { return delta; } set { } }
        public TimeSpan PreviousLap { get { return prevlap; } set { } }

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
                string yaml = sdk.GetSessionInfo();

                // caridx
                Int32 start = yaml.IndexOf("DriverCarIdx: ") + "DriverCarIdx: ".Length;
                Int32 end = yaml.IndexOf("\n", start);
                carIdx = Int32.Parse(yaml.Substring(start, end - start));

                // track length
                start = yaml.IndexOf("TrackLength: ") + "TrackLength: ".Length;
                end = yaml.IndexOf("km\n", start);
                string dbg = yaml.Substring(start, end - start);
                trackLength = (Int32)(Single.Parse(yaml.Substring(start, end - start)) * 1000);

                // session types
                //string pattern = @"SessionNum: (\d+)"; //SessionType: (\d+)";
                RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
                MatchCollection sessionNums, sessionTypes;
                Regex optionRegex = new Regex(@"SessionNum: (\d+)", options);

                // Get matches of pattern in yaml
                sessionNums = optionRegex.Matches(yaml);

                optionRegex = new Regex(@"SessionType: (\w+)", options);
                sessionTypes = optionRegex.Matches(yaml);

                Int32 currentSessionNum = (Int32)sdk.GetData("SessionNum");

                // Iterate matches
                for (int ctr = 0; ctr < Math.Min(sessionNums.Count, sessionTypes.Count); ctr++)
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
                bestlap = new Double[trackLength / 10];
                currentlap = new Double[trackLength / 10];

                // init timedelta
                timedelta = new TimeDelta(trackLength);

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

                    Int32 lapPointer = (Int32)Math.Floor(driverTrkPos[carIdx] * (trackLength / 10));

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

                            if (currentlap[currentlap.Length - 1] < bestlap[bestlap.Length - 1] || bestlap[bestlap.Length - 1] == 0.0)
                            {
                                Array.Copy(currentlap, bestlap, currentlap.Length);
                            }
                        }

                        // start new lap
                        lapStartTime = sessionTime - (1 - tickCorrection) * time;
                        lapTimeValid = true;
                    }
                    else if(Math.Abs(driverTrkPos[carIdx] - lastTickTrackPos) > 0.1) {
                        // invalidate lap time if jumping too much
                        lapTimeValid = false;
                    }

                    if (lapTimeValid && lapPointer >= 0)
                    {
                        currentlap[lapPointer] = (sessionTime - lapStartTime);
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
                    else if (lapPointer >= 0)
                    {
                        Double diff = currentlap[lapPointer] - bestlap[lapPointer];
                        delta = new TimeSpan(0, 0, 0, (Int32)Math.Floor(diff), (Int32)Math.Abs((diff % 1) * 1000));
                    }
                    else
                        delta = new TimeSpan();
                }
            }
            else
            {
                init = false;
            }
        }
    }
}
