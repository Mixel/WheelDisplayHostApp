using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelDisplayHostApp
{
    class TimeDelta
    {
        private static Int32 maxcars = 64;
        private Double[][] splits = new Double[maxcars][];
        private Int32[] splitPointer = new Int32[maxcars];
        private Single splitLength;
        private Double prevTimestamp;
        private Int32 followed;
        private Double[] bestlap;
        private Double[] currentlap;
        private Boolean validbestlap;
        private Double lapstarttime;

        public TimeDelta(Single length)
        {
            // split times every 10 meters
            Int32 arraySize = (Int32)Math.Round(length / 10);

            // set split length
            splitLength = (Single)(1.0 / (Double)arraySize);

            // init best lap
            followed = -1;
            bestlap = new Double[arraySize];
            currentlap = new Double[arraySize];
            validbestlap = false;

            // initialize array
            for (Int32 i = 0; i < maxcars; i++)
                splits[i] = new Double[arraySize];
        }

        public void SaveBestLap(Int32 caridx)
        {
            followed = caridx;
        }

        public TimeSpan BestLap { get { if (validbestlap) return new TimeSpan(0, 0, 0, (Int32)bestlap[bestlap.Length - 1], (Int32)((bestlap[bestlap.Length - 1] % 1) * 1000)); else return new TimeSpan(); } set { } }

        public void Update(Double timestamp, Single[] trackPosition) 
        {
            // sanity check
            if (timestamp > prevTimestamp)
            {
                Int32 currentSplitPointer;

                for (Int32 i = 0; i < trackPosition.Length; i++)
                {
                    if (trackPosition[i] > 0)
                    {
                        // interpolate split border crossing
                        currentSplitPointer = (Int32)Math.Floor((trackPosition[i] % 1) / splitLength);

                        if (currentSplitPointer != splitPointer[i])
                        {
                            // interpolate
                            Single distance = trackPosition[i] - (currentSplitPointer * splitLength);
                            Single correction = distance / splitLength;
                            Double currentSplitTime = timestamp - ((timestamp - prevTimestamp) * correction);

                            // save in case of new lap record
                            if (followed >= 0)
                            {
                                // check new lap
                                if (currentSplitPointer == 0)
                                {
                                    if ((currentSplitTime - splits[i][0]) < bestlap[bestlap.Length - 1] || bestlap[bestlap.Length - 1] == 0)
                                    {
                                        validbestlap = true;
                                        // save lap and substract session time offset
                                        for (Int32 j = 0; j < bestlap.Length - 1; j++)
                                        {
                                            bestlap[j] = splits[i][j + 1] - splits[i][0];
                                            if (splits[i][j + 1] == 0.0)
                                                validbestlap = false;
                                        }

                                        bestlap[bestlap.Length - 1] = currentSplitTime - splits[i][0];
                                    }
                                }

                                lapstarttime = currentlap[currentSplitPointer];
                                currentlap[currentSplitPointer] = currentSplitTime;
                            }
                            
                            // save
                            splits[i][currentSplitPointer] = currentSplitTime;
                            splitPointer[i] = currentSplitPointer;
                        }
                    }
                }

                prevTimestamp = timestamp;
            }
        }

        public TimeSpan GetBestLapDelta(Single trackPosition)
        {
            if (validbestlap)
            {
                Int32 currentSplitPointer = (Int32)Math.Floor((Math.Abs(trackPosition) % 1) / splitLength);
                Double delta;

                if (currentSplitPointer == 0)
                    delta = (splits[followed][0] - lapstarttime) - bestlap[bestlap.Length - 1];
                else if (currentSplitPointer == (bestlap.Length - 1))
                    delta = (splits[followed][currentSplitPointer] - lapstarttime) - bestlap[bestlap.Length - 1];
                else
                    delta = (splits[followed][currentSplitPointer] - splits[followed][bestlap.Length - 1]) - bestlap[currentSplitPointer - 1];

                return new TimeSpan(0, 0, 0, (Int32)Math.Floor(delta), (Int32)Math.Abs((delta % 1) * 1000));
            }
            else
            {
                return new TimeSpan();
            }
        }

        public TimeSpan GetDelta(Int32 caridx1, Int32 caridx2)
        {
            // validate
            if (caridx1 < maxcars && caridx2 < maxcars)
            {
                // comparing latest finished split
                Int32 comparedSplit = splitPointer[caridx1];

                // catch negative index and loop it to last index
                if (comparedSplit < 0)
                    comparedSplit = splits[caridx1].Length - 1;

                Double delta = splits[caridx1][comparedSplit] - splits[caridx2][comparedSplit];

                if (delta < 0)
                    return new TimeSpan();
                else
                    return new TimeSpan(0, 0, 0, (Int32)Math.Floor(delta), (Int32)Math.Abs((delta % 1) * 1000));
            }
            else
            {
                return new TimeSpan();
            }
        }
    }
}
