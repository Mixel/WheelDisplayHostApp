/*
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 
 * 
 * TimeDelta class
 * Calculates time difference between two drivers or previously recorded best lap
 * 
 * Constants:
 * Int32 maxcars: length of track positions array
 * Int32 splitdistance: split length, too low value will skip splits with fast cars 
 * and too big will reduce update rate. 10 meters is a good start value.
 * 
 * Interfaces:
 *  TimeDelta(Single length)
 *      Class constructor
 *      Parameters:
 *          length: Track length in some format, see splitdistance constant
 * 
 *  Update(Double timestamp, Single[] trackPosition)
 *      Updates data using timestamp and car positions. Also handles best lap if car 
 *      id is set, see SaveBestLap().
 *      Parameters:
 *          timestamp: increasing timestamp
 *          trackPosition: array of car positions indexed with car ids
 *  
 *  SaveBestLap(Int32 caridx)
 *      Sets player's car id, which will be followed and best lap will be saved for 
 *      comparison. Set -1 to disable.
 *      Parameters:
 *          caridx: car id for car to be followed
 *  
 *  GetBestLapDelta(Single trackPosition)
 *      Gets delta to previously saved best lap
 *      Parameters:
 *          trackPosition: Current trackposition to which best lap is compared to.
 *          Uses data from Update()-function to calculate current laptime.
 *          
 *  GetDelta(Int32 caridx1, Int32 caridx2)
 *      Gets delta between two cars, doesn't take care if drivers are lapped.
 *      Returns time of caridx1-caridx2
 *      Parameters:
 *          caridx1: First driver car id (car behind)
 *          caridx2: Second driver car id (car infront)
 * 
 *  BestLap
 *      Gets lap time of best lap
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelDisplayHostApp
{
    class TimeDelta
    {
        private static Int32 maxcars = 64;
        private static Int32 splitdistance = 10;

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
            Int32 arraySize = (Int32)Math.Round(length / splitdistance);

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
