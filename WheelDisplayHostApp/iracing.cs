using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRSDKSharp;

namespace WheelDisplayHostApp
{
    class iracing
    {
        // API variables
        private iRacingSDK sdk;
        private Int32 carIdx;

        // internal variables
        private Int32 gear;
        private Int32 rpm;
        private Int32 speed;
        private Int32 fuel;
        private Int32 fuelneed;
        private Int32 lap;
        private Int32 lapsrem;
        private Int32 position;

        private Single lastTickTrackPos = 0;
        private Double lastTickTime;
        private Double lapStartTime = -1;

        // public interface
        public Int32 Gear { get { return gear; } set { } }
        public Int32 RPM { get { return rpm; } set { } }
        public Int32 Speed { get { return speed; } set { } }
        public Int32 Fuel { get { return fuel; } set { } }
        public Int32 FuelNeeded { get { return fuelneed; } set { } }
        public Int32 Lap { get { return lap; } set { } }
        public Int32 LapsRemaining { get { return lapsrem; } set { } }
        public Int32 Position { get { return position; } set { } }
        public TimeSpan LapTime { get { return new TimeSpan(0, 0, 0, (Int32)Math.Floor(lastTickTime - lapStartTime), (Int32)(((lastTickTime - lapStartTime) % 1) * 1000)); } set { } }
        public TimeSpan Delta { get { return new TimeSpan(); } set { } }

        // private 

        public iracing()
        {
            sdk = new iRacingSDK();
            sdk.Startup();

            // wait connection
            while (!sdk.IsConnected());
            
            // get caridx
            string yaml = sdk.GetSessionInfo();
            Int32 found = yaml.IndexOf("DriverCarIdx: ");
            carIdx = Int32.Parse(yaml.Substring(found + "DriverCarIdx: ".Length, 4));

            // reset laptime
            lapStartTime = (Double)sdk.GetData("ReplaySessionTime");
        }

        public void updateData()
        {
            //Check if the SDK is connected
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

                if (carIdx >= 0) // skip thing that require caridx if we don't have it
                {
                    Int32[] driverLaps = new Int32[64];
                    driverLaps = (Int32[])sdk.GetData("CarIdxLap");

                    Single[] driverTrkPos = new Single[64];
                    driverTrkPos = (Single[])sdk.GetData("CarIdxLapDistPct");

                    if (driverTrkPos[carIdx] < 0.1 && lastTickTrackPos > 0.9)
                    {
                        Double distance = (1 - lastTickTrackPos) + driverTrkPos[carIdx];
                        Double time = (Double)sdk.GetData("ReplaySessionTime") - lastTickTime;
                        Double tickCorrection = (1 - lastTickTrackPos) / distance;
                        lapStartTime = (Double)sdk.GetData("ReplaySessionTime") - (1 - tickCorrection) * time;
                    }

                    lastTickTrackPos = driverTrkPos[carIdx]; // save for next tick
                    lastTickTime = (Double)sdk.GetData("ReplaySessionTime");

                    Int32[] driverCarIdx = new Int32[64];

                    for (int i = 0; i < 64; i++)
                    {
                        driverTrkPos[i] += (Single)driverLaps[i];
                        driverCarIdx[i] = i;
                    }

                    Array.Sort(driverTrkPos, driverCarIdx);
                    Array.Reverse(driverCarIdx);
                    position = (Int32)(Array.IndexOf(driverCarIdx, carIdx) + 1);
                }
            }
        }
    }
}
