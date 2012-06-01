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
        private static Int32 fuelconslaps = 5; // how many laps over fuel consumption is averaged

        // MMF access
        MemoryMappedFile mmap;
        MemoryMappedViewAccessor shmem;

        // internal variables
        private Boolean initialized;
        private Int32 carid;
        private Int32 gear;
        private Double rpm;
        private Double maxrpm;
        private Double speed;
        private Double fuel;
        private Double timestamp;
        private Int32 fuelneed;
        private Int32 lap;
        private Int32 totallaps;
        private Int32 position;
        private TimeSpan delta;
        private Double prevlap;
        private Int32 pitlimiter;
        private Double[] driverTrkPos;
        private Int32 carinfront;
        private String carname;
        private String trackname;
        private Int32 state;
        private Int32 prevstate;

        private Double lastTickTrackPos = 0;
        private Double lastTickTime;
        private Double lapStartTime = -1;
        private Double trackLength;
        private TimeDelta timedelta;
        private Boolean lapTimeValid;
        private Double[] fuelcons;
        private Int32 fuelconsPtr;
        private Double fuelconsumption;
        private Int32 sessiontype;
        private Boolean needReset;

        // interface
        public Int32 Speed { get { return (Int32)speed; } set { } }
        public Int32 Gear { get { return gear; } set { } }
        public Int32 RPM { get { return (Int32)rpm; } set { } }
        public Int32 Fuel { get { return (Int32)Math.Floor(fuel); } set { } }
        public Int32 FuelNeeded { get { return fuelneed; } set { } }
        public Double fuelConsumption { get { return fuelconsumption; } set { } }
        public Int32 Lap { get { return lap; } set { } }
        public Int32 LapsRemaining { get { if (totallaps > 0) return totallaps - lap; else return 0; } set { } }
        public Int32 Position { get { return position; } set { } }
        public TimeSpan LapTime { 
            get { 
                if (state == 2) 
                    return new TimeSpan(0, 0, 0, (Int32)Math.Floor(lastTickTime - lapStartTime), (Int32)(((lastTickTime - lapStartTime) % 1) * 1000)); 
                else 
                    return new TimeSpan(); 
            } 
            set { } 
        }
        public TimeSpan BestLap { get { if (timedelta != null) return timedelta.BestLap; else return new TimeSpan(); } set { } }
        public TimeSpan Delta { get { return delta; } set { } }
        public TimeSpan PreviousLap { get { return new TimeSpan(0, 0, 0, 0, (Int32)(prevlap*1000)); } set { } }
        public Boolean PitLimiter { get { if(pitlimiter != 0) return true; else return false; } set { } }

        public Double ShiftIndicator { 
            get {
                Double rpmpct = (rpm / maxrpm);
                if (rpmpct > 0.75)
                    return Math.Min((rpmpct - 0.75) * 4.1, 1.0);
                else
                    return 0.0;
            } 
            set { } 
        }

        public rFactor2()
        {
            initialized = false;
            reset();
        }

        ~rFactor2()
        {
            SaveBestLap();
        }

        public void initialize()
        {
            try
            {
                mmap = System.IO.MemoryMappedFiles.MemoryMappedFile.OpenExisting(
                   "rFtelemetry",
                   MemoryMappedFileRights.Read);

                shmem = mmap.CreateViewAccessor(0, 1024, MemoryMappedFileAccess.Read);

                needReset = true;
                initialized = true;
            }
            catch
            {
                initialized = false;
            }
        }

        public Boolean isInitialized { get { return initialized; } set { } }

        private void reset() {
            // empty track position
            driverTrkPos = new Double[maxcars];

            // reset laptimes
            lapStartTime = 0;
            lapTimeValid = false;

            // fuel consumption, last 5 lap rolling
            fuelcons = new Double[fuelconslaps];
            fuelconsPtr = 0;

            // init timedelta
            timedelta = new TimeDelta((Int32)trackLength);
            timedelta.SaveBestLap(carid);
        }

        public void updateData() {

            if (initialized)
            {
                Int64 pos = 0;

                state = shmem.ReadInt32(pos);
                pos += sizeof(Int32);

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

                sessiontype = shmem.ReadInt32(pos);
                pos += sizeof(Int32);

                carinfront = shmem.ReadInt32(pos);
                pos += sizeof(Int32);

                timestamp = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                speed = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                rpm = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                fuel = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                prevlap = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                trackLength = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                maxrpm = shmem.ReadDouble(pos);
                pos += sizeof(Double);

                shmem.ReadArray<Double>(pos, driverTrkPos, 0, maxcars);
                pos += maxcars * sizeof(Double);

                Byte[] buf = new Byte[64];

                shmem.ReadArray<Byte>(pos, buf, 0, 64);
                pos += 64 * sizeof(Byte);
                buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, buf);
                carname = Encoding.UTF8.GetString(buf, 0, 64);
                carname = carname.Substring(0, carname.IndexOf('\0'));

                shmem.ReadArray<Byte>(pos, buf, 0, 64);
                pos += 64 * sizeof(Byte);
                buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, buf);
                trackname = Encoding.UTF8.GetString(buf, 0, 64);
                trackname = trackname.Substring(0, trackname.IndexOf('\0'));

                // process
                if (needReset && trackLength > 0)
                {
                    reset();
                    LoadBestLap();
                    needReset = false;
                }

                if (prevstate != state)
                {
                    if (state == 2) // entering driving mode
                        LoadBestLap();
                    else if (prevstate == 2) // exiting driving mode
                        SaveBestLap();

                    prevstate = state;
                }

                timedelta.Update(timestamp, driverTrkPos);

                if (driverTrkPos[carid] < 0.1 && lastTickTrackPos > 0.9)
                {
                    Double distance = (1 - lastTickTrackPos) + driverTrkPos[carid];
                    Double time = timestamp - lastTickTime;
                    Double tickCorrection = (1 - lastTickTrackPos) / distance;

                    // save lap time
                    if (lapTimeValid)
                    {
                        fuelcons[fuelconsPtr % fuelcons.Length] = fuel;

                        // update fuel consumption after one full lap
                        if (fuelconsPtr > 0)
                        {
                            if (fuelconsPtr >= fuelcons.Length)
                            {
                                Double[] consrate = new Double[fuelcons.Length - 1];
                                Int32 j = 0;
                                for (int i = fuelconsPtr; i < fuelconsPtr + consrate.Length; i++)
                                {
                                    consrate[j++] = fuelcons[(i + 1) % fuelcons.Length] - fuelcons[(i + 2) % fuelcons.Length];
                                }
                                fuelneed = (Int32)(fuelcons[fuelconsPtr % fuelcons.Length] - ((totallaps-lap) * consrate.Average()));
                                fuelconsumption = consrate.Average();
                            }
                            else if (fuelconsPtr > 0)
                            {
                                fuelneed = (Int32)(fuelcons[fuelconsPtr % fuelcons.Length] - ((totallaps - lap) * fuelcons[(fuelconsPtr - 1) % fuelcons.Length]));
                                fuelconsumption = fuelcons[(fuelconsPtr - 1) % fuelcons.Length] - fuelcons[fuelconsPtr % fuelcons.Length];
                            }
                        }
                        fuelconsPtr++;
                    }

                    // start new lap
                    lapStartTime = timestamp - (1 - tickCorrection) * time;
                    lapTimeValid = true;

                    // reset fuel consumption when in pits
                    if (driverTrkPos[carid] < 0)
                    {
                        fuelcons = new Double[fuelconslaps];
                        fuelconsPtr = 0;
                    }
                }
                else if (Math.Abs(driverTrkPos[carid] - lastTickTrackPos) > 0.1)
                {
                    // invalidate lap time if jumping too much
                    lapTimeValid = false;
                }

                lastTickTrackPos = driverTrkPos[carid]; // save for next tick
                lastTickTime = timestamp;

                if (sessiontype >= 10) // if (race)
                    delta = timedelta.GetDelta(carid, carinfront);
                else
                    delta = timedelta.GetBestLapDelta(driverTrkPos[carid] % 1);
            }
        }

        private void LoadBestLap() {
            String workdir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Wheel Display\\rFactor2\\" + carname + "\\";
            System.IO.Directory.CreateDirectory(workdir);
            timedelta.LoadLap(workdir + trackname + ".lap");
        }

        private void SaveBestLap()
        {
            String workdir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Wheel Display\\rFactor2\\" + carname + "\\";
            System.IO.Directory.CreateDirectory(workdir);
            timedelta.StoreLap(workdir + trackname + ".lap");
        }
    }
}
