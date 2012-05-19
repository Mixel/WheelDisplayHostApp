using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;

namespace WheelDisplayHostApp
{
    public struct TelemetryData
    {
        public Int32 carid;
        public Int32 gear;
        public Int32 lap;
        public Int32 totallaps;
        public Int32 position;
        public Int32 pitlimiter;

        public Double timestamp;
        public Double speed;
        public Double rpm;
        public Double fuel;
    };

    class rFactor2
    {
        // MMF access
        MemoryMappedFile mmap;
        MemoryMappedViewAccessor shmem;

        // API
        TelemetryData data;

        // internal variables
        Boolean initialized;
        private Int32 fuelneed;
        private TimeSpan delta;
        private TimeSpan prevlap;
        private Single shiftindicator;
        private Double timestamp;

        // interface
        public Int32 Speed { get { return (Int32)data.speed; } set { } }
        public Int32 Gear { get { return data.gear; } set { } }
        public Int32 RPM { get { return (Int32)data.rpm; } set { } }
        public Int32 Fuel { get { return (Int32)Math.Floor(data.fuel); } set { } }
        public Int32 FuelNeeded { get { return fuelneed; } set { } }
        //public Single fuelConsumption { get { return fuelconsumption; } set { } }
        public Int32 Lap { get { return data.lap; } set { } }
        public Int32 LapsRemaining { get { if (data.totallaps > 0) return data.totallaps - data.lap; else return 0; } set { } }
        public Int32 Position { get { return data.position; } set { } }
        //public TimeSpan LapTime { get { return new TimeSpan(0, 0, 0, (Int32)Math.Floor(lastTickTime - lapStartTime), (Int32)(((lastTickTime - lapStartTime) % 1) * 1000)); } set { } }
        //public TimeSpan BestLap { get { if (timedelta != null) return timedelta.BestLap; else return new TimeSpan(); } set { } }
        public TimeSpan Delta { get { return delta; } set { } }
        public TimeSpan PreviousLap { get { return prevlap; } set { } }
        public Single ShiftIndicator { get { return shiftindicator; } set { } }
        public Boolean PitLimiter { get { if(data.pitlimiter != 0) return true; else return false; } set { } }

        public rFactor2()
        {
            data = new TelemetryData();
            initialized = false;
        }

        public void initialize()
        {
            try
            {
                mmap = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateOrOpen(
                   "rFtelemetry",
                   System.Runtime.InteropServices.Marshal.SizeOf(data),
                   MemoryMappedFileAccess.Read);

                shmem = mmap.CreateViewAccessor(0, System.Runtime.InteropServices.Marshal.SizeOf(data), MemoryMappedFileAccess.Read);

                initialized = true;
            }
            catch
            {
                initialized = false;
            }
        }

        public Boolean isInitialized { get { return initialized; } set { } }

        public void updateData()
        {
            if (initialized)
            {
                shmem.Read(0, out data);
            }
        }
    }
}
