using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace WheelDisplayHostApp
{
    class rFactor2
    {
        // config
        private static Int32 maxcars = 64;

        // MMF access
        MemoryMappedFile mmap;
        MemoryMappedViewAccessor shmem;

        // internal variables
        private Boolean initialized;
        private Int32 carid;
        private Int32 gear;
        private Double rpm;
        private Double speed;
        private Double fuel;
        private Double timestamp;
        private Int32 fuelneed;
        private Int32 lap;
        private Int32 totallaps;
        private Int32 position;
        private TimeSpan delta;
        private TimeSpan prevlap;
        private Single shiftindicator;
        private Int32 pitlimiter;
        private Double[] trackposition;

        // interface
        public Int32 Speed { get { return (Int32)speed; } set { } }
        public Int32 Gear { get { return gear; } set { } }
        public Int32 RPM { get { return (Int32)rpm; } set { } }
        public Int32 Fuel { get { return (Int32)Math.Floor(fuel); } set { } }
        public Int32 FuelNeeded { get { return fuelneed; } set { } }
        //public Single fuelConsumption { get { return fuelconsumption; } set { } }
        public Int32 Lap { get { return lap; } set { } }
        public Int32 LapsRemaining { get { if (totallaps > 0) return totallaps - lap; else return 0; } set { } }
        public Int32 Position { get { return position; } set { } }
        //public TimeSpan LapTime { get { return new TimeSpan(0, 0, 0, (Int32)Math.Floor(lastTickTime - lapStartTime), (Int32)(((lastTickTime - lapStartTime) % 1) * 1000)); } set { } }
        //public TimeSpan BestLap { get { if (timedelta != null) return timedelta.BestLap; else return new TimeSpan(); } set { } }
        public TimeSpan Delta { get { return delta; } set { } }
        public TimeSpan PreviousLap { get { return prevlap; } set { } }
        public Single ShiftIndicator { get { return shiftindicator; } set { } }
        public Boolean PitLimiter { get { if(pitlimiter != 0) return true; else return false; } set { } }

        public rFactor2()
        {
            initialized = false;
            trackposition = new Double[maxcars];
        }

        public void initialize()
        {
            try
            {
                mmap = System.IO.MemoryMappedFiles.MemoryMappedFile.OpenExisting(
                   "rFtelemetry",
                   MemoryMappedFileRights.Read);

                shmem = mmap.CreateViewAccessor(0, 1024, MemoryMappedFileAccess.Read);

                initialized = true;
            }
            catch
            {
                initialized = false;
            }
        }

        public Boolean isInitialized { get { return initialized; } set { } }

        public void updateData() {

            if (initialized)
            {
                //shmem.Read(0, out data);
                //Console.WriteLine("{0}\n", data.trackpositions[0]);

                Int64 pos = 0;

                carid = shmem.ReadInt32(pos);
                pos += sizeof(Int32);

                gear = shmem.ReadInt32(pos);
                pos += sizeof(Int32);

                lap = shmem.ReadInt32(pos);
                pos += sizeof(Int32);

                totallaps = shmem.ReadInt32(pos);
                pos += sizeof(Int32);

                position = shmem.ReadInt32(pos);
                pos += sizeof(Int32);

                pitlimiter = shmem.ReadInt32(pos);
                pos += sizeof(Int32);

                timestamp = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                speed = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                rpm = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                fuel = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                for (Int32 i = 0; i < maxcars; i++)
                {
                    trackposition[i] = shmem.ReadDouble(pos);
                    pos += sizeof(Double);
                }

                Console.WriteLine("{0}", trackposition[carid]);

            }
        }
    }
}
