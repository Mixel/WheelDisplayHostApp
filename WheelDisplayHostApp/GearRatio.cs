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
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelDisplayHostApp
{
    class GearRatio
    {
        // rpm[gear][speed][avg, count]
        private Int32[][][] data;

        public GearRatio()
        {
            // initialize data array
            data = new Int32[7][][];

            for (Int32 i = 0; i < data.Length; i++)
            {
                data[i] = new Int32[400][];

                for (Int32 j = 0; j < data[i].Length; j++)
                    data[i][j] = new Int32[2];
            }
        }

        public void Update(Single speed, Int32 gear, Int32 rpm)
        {
            // update data array only when gear > N and speed is close to even number
            Int32 i;

            if (gear > 0)
            {
                if ((speed % 1) < 0.05)
                    i = (Int32)Math.Floor(speed);
                else if ((speed % 1) > 0.95)
                    i = (Int32)Math.Ceiling(speed);
                else
                    return;

                data[gear - 1][i][1]++;
                data[gear - 1][i][0] = (data[gear - 1][i][0] + rpm) / data[gear - 1][i][1];
                return;
            }
        }

        public Single getGear(Int32 gear)
        {
            if(gear < 1)
                return 0.0f;
            else {
                Double ratio = new Double();
                Int32 ratioCount = 0;

                for (Int32 i = 1; i < data[gear - 1].Length; i++)
                {
                    if (data[gear - 1][i][1] > 0)
                    {
                        ratio += (Double)data[gear - 1][i][0] / (i * data[gear - 1][i][1]);
                        ratioCount++;
                    }
                }

                return (Single)(ratio/ratioCount);
            }
        }
    }
}
