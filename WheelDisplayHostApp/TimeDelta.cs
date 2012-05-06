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
            Int32 arraySize = (Int32)Math.Round(length / 5);

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
                    currentSplitPointer = (Int32)Math.Floor(trackPosition[i] / splitLength);
                    splits[i][currentSplitPointer] = timestamp;
                    splitPointer[i] = currentSplitPointer;
                }
            }

            prevTimestamp = timestamp;
        }

        public TimeSpan GetDelta(Int32 caridx1, Int32 caridx2)
        {
            Int32[] car = new Int32[2];
            if (splitPointer[caridx1] >= splitPointer[caridx2])
            {
                car[0] = caridx1;
                car[1] = caridx2;
            }
            else
            {
                car[0] = caridx2;
                car[1] = caridx1;
            }

            // comparing latest finished split
            Int32 comparedSplit = splitPointer[car[1]]-1;

            // catch negative index and loop it to last index
            if (comparedSplit < 0)
                comparedSplit = splits[car[0]].Length - 1;
            Double delta = splits[car[1]][comparedSplit] - splits[car[0]][comparedSplit];

            return new TimeSpan(0, 0, 0, (Int32)Math.Floor(delta), (Int32)Math.Floor((delta % 1) * 1000));
        }
    }
}
