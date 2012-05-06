using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelDisplayHostApp
{
    class TimeDelta
    {
        private Double[][] splits = new Double[64][];
        private Int32[] splitPointer = new Int32[64];
        private Single splitLength;
        private Double prevTimestamp;

        public TimeDelta(Single length)
        {
            // split times every 5 meters
            Int32 arraySize = (Int32)Math.Round(length / 10);

            // set split length
            splitLength = (Single)(1.0 / (Double)arraySize);

            // initialize array
            for (Int32 i = 0; i < 64; i++)
                splits[i] = new Double[arraySize];
        }

        public void Update(Double timestamp, Single[] trackPosition) 
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
                        Single distance = trackPosition[i] - (currentSplitPointer * splitLength);
                        Single correction = distance / splitLength;
                        splits[i][currentSplitPointer] = timestamp - ((timestamp - prevTimestamp)* correction);
                        splitPointer[i] = currentSplitPointer;
                    }
                }
            }

            prevTimestamp = timestamp;
        }

        public TimeSpan GetDelta(Int32 caridx1, Int32 caridx2)
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
    }
}
