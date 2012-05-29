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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WheelDisplayHostApp
{
    public partial class Main : Form
    {
        private iRacing ir;
        private rFactor2 rf;
        private usb u;

        private DateTime ledBlink;
        private static Int32 blinkDuration = 500;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ir = new iRacing();
            rf = new rFactor2();
            u = new usb();
            ledBlink = DateTime.Now;
        }

        private void apiUpdater_Tick(object sender, EventArgs e)
        {
            if (ir != null && ir.isInitialized)
            {
                SimStatusValue.Text = "iRacing";
                ir.updateData();

                if (ir.Gear < 0)
                    GearValue.Text = "R";
                else if (ir.Gear == 0)
                    GearValue.Text = "N";
                else
                    GearValue.Text = ir.Gear.ToString();

                RPMValue.Text = ir.RPM.ToString();
                SpeedValue.Text = ir.Speed.ToString() + " km/h";
                FuelValue.Text = ir.Fuel.ToString() + " ltr";
                FuelNeededValue.Text = ir.FuelNeeded.ToString() + " ltr";
                FuelConsumptionValue.Text = Math.Round(ir.fuelConsumption, 1).ToString() + " ltr/lap";
                LapValue.Text = ir.Lap.ToString();
                LapsRemainingValue.Text = ir.LapsRemaining.ToString();
                PositionValue.Text = ir.Position.ToString();
                LapTimeValue.Text = ir.LapTime.Minutes.ToString() + ":" + ir.LapTime.Seconds.ToString("D2") + "." + ir.LapTime.Milliseconds.ToString("D3");
                BestLapValue.Text = ir.BestLap.Minutes.ToString() + ":" + ir.BestLap.Seconds.ToString("D2") + "." + ir.BestLap.Milliseconds.ToString("D3");
                PreviousLapValue.Text = ir.PreviousLap.Minutes.ToString() + ":" + ir.PreviousLap.Seconds.ToString("D2") + "." + ir.PreviousLap.Milliseconds.ToString("D3");
                RPMLedsValue.Value = (Int32)Math.Floor(ir.ShiftIndicator * 100);

                if (ir.Delta.TotalMilliseconds < 0)
                    DeltaValue.Text = "-" + Math.Abs(ir.Delta.Minutes).ToString() + ":" + Math.Abs(ir.Delta.Seconds).ToString("D2") + "." + Math.Abs(ir.Delta.Milliseconds).ToString("D3");
                else

                    DeltaValue.Text = ir.Delta.Minutes.ToString() + ":" + ir.Delta.Seconds.ToString("D2") + "." + ir.Delta.Milliseconds.ToString("D3");
            }
            else if (rf != null && rf.isInitialized)
            {
                rf.updateData();
                SimStatusValue.Text = "rFactor2";

                if (rf.Gear < 0)
                    GearValue.Text = "R";
                else if (rf.Gear == 0)
                    GearValue.Text = "N";
                else
                    GearValue.Text = rf.Gear.ToString();

                RPMValue.Text = rf.RPM.ToString();
                SpeedValue.Text = rf.Speed.ToString() + " km/h";
                FuelValue.Text = rf.Fuel.ToString() + " ltr";
                FuelNeededValue.Text = rf.FuelNeeded.ToString() + " ltr";
                //FuelConsumptionValue.Text = Math.Round(rf.fuelConsumption, 1).ToString() + " ltr/lap";
                LapValue.Text = rf.Lap.ToString();
                LapsRemainingValue.Text = rf.LapsRemaining.ToString();
                PositionValue.Text = rf.Position.ToString();
                //LapTimeValue.Text = rf.LapTime.Minutes.ToString() + ":" + ir.LapTime.Seconds.ToString("D2") + "." + ir.LapTime.Milliseconds.ToString("D3");
                //BestLapValue.Text = rf.BestLap.Minutes.ToString() + ":" + ir.BestLap.Seconds.ToString("D2") + "." + ir.BestLap.Milliseconds.ToString("D3");
                PreviousLapValue.Text = rf.PreviousLap.Minutes.ToString() + ":" + ir.PreviousLap.Seconds.ToString("D2") + "." + ir.PreviousLap.Milliseconds.ToString("D3");
                RPMLedsValue.Value = (Int32)Math.Floor(rf.ShiftIndicator * 100);
            }
            else
            {
                SimStatusValue.Text = "not connected";
                ir.initialize();
                rf.initialize();
            }
        }

        private void usbUpdater_Tick(object sender, EventArgs e)
        {
            if (u != null && u.isInitialized && ir != null && ir.isInitialized)
            {
                USBStatusValue.Text = "connected";

                if (ir.Gear < 0)
                    u.updateType(usb.types.Gear, 7);
                else if (ir.Gear == 0)
                    u.updateType(usb.types.Gear, 8);
                else
                    u.updateType(usb.types.Gear, (short)(ir.Gear - 1));
                
                u.updateType(usb.types.Speed, (short)ir.Speed);
                u.updateType(usb.types.RPM, (short)ir.RPM);
                u.updateType(usb.types.Lap, (short)ir.Lap);
                u.updateType(usb.types.Fuel, (short)ir.Fuel);
                u.updateType(usb.types.FuelNeeded, (short)ir.FuelNeeded);
                u.updateType(usb.types.LapsRemaining, (short)ir.LapsRemaining);
                u.updateType(usb.types.Position, (short)ir.Position);
                
                ushort deltabytes = 0;
                if (ir.Delta.TotalMilliseconds < 0)
                    deltabytes |= 1 << 15;

                deltabytes |= (ushort)((UInt16)(Math.Abs(ir.Delta.Minutes)) << 9);
                deltabytes |= (ushort)((UInt16)(Math.Abs(ir.Delta.Seconds)) << 3);
                deltabytes |= (ushort)((UInt16)(Math.Abs(ir.Delta.Milliseconds/100)));

                u.updateType(usb.types.Delta, unchecked((short)deltabytes));
                
                ushort laptimebytes = 0;
                if (ir.LapTime.TotalMilliseconds < 0)
                    laptimebytes |= 1 << 15;

                laptimebytes |= (ushort)((UInt16)(Math.Abs(ir.LapTime.Minutes)) << 9);
                laptimebytes |= (ushort)((UInt16)(Math.Abs(ir.LapTime.Seconds)) << 3);
                laptimebytes |= (ushort)((UInt16)(Math.Abs(ir.LapTime.Milliseconds / 100)));

                u.updateType(usb.types.LapTime, unchecked((short)laptimebytes));



                if (ir.PitLimiter)
                {
                    if ((DateTime.Now - ledBlink).TotalMilliseconds > blinkDuration)
                    {
                        if (((DateTime.Now.Millisecond / blinkDuration) % 2) > 0)
                            u.updateType(usb.types.ShiftLEDs, 0x00);
                        else
                        {
                            u.updateType(usb.types.ShiftLEDs, 0xff);
                        }
                    }
                }
                else
                {
                    ushort shiftleds = (ushort)((1 << (Int16)Math.Floor(ir.ShiftIndicator * 7.1f)) - 1);
                    u.updateType(usb.types.ShiftLEDs, unchecked((short)shiftleds));
                }
            }
            else if (u != null && u.isInitialized && rf != null && rf.isInitialized)
            {
                USBStatusValue.Text = "connected";

                if (rf.Gear < 0)
                    u.updateType(usb.types.Gear, 7);
                else if (rf.Gear == 0)
                    u.updateType(usb.types.Gear, 8);
                else
                    u.updateType(usb.types.Gear, (short)(rf.Gear - 1));

                u.updateType(usb.types.Speed, (short)rf.Speed);
                u.updateType(usb.types.RPM, (short)rf.RPM);
                u.updateType(usb.types.Lap, (short)rf.Lap);
                u.updateType(usb.types.Fuel, (short)rf.Fuel);
                u.updateType(usb.types.FuelNeeded, (short)rf.FuelNeeded);
                u.updateType(usb.types.LapsRemaining, (short)rf.LapsRemaining);
                u.updateType(usb.types.Position, (short)rf.Position);

                ushort deltabytes = 0;
                if (rf.Delta.TotalMilliseconds < 0)
                    deltabytes |= 1 << 15;

                deltabytes |= (ushort)((UInt16)(Math.Abs(rf.Delta.Minutes)) << 9);
                deltabytes |= (ushort)((UInt16)(Math.Abs(rf.Delta.Seconds)) << 3);
                deltabytes |= (ushort)((UInt16)(Math.Abs(rf.Delta.Milliseconds / 100)));

                u.updateType(usb.types.Delta, unchecked((short)deltabytes));

                /*
                ushort laptimebytes = 0;
                if (rf.LapTime.TotalMilliseconds < 0)
                    laptimebytes |= 1 << 15;

                laptimebytes |= (ushort)((UInt16)(Math.Abs(rf.LapTime.Minutes)) << 9);
                laptimebytes |= (ushort)((UInt16)(Math.Abs(rf.LapTime.Seconds)) << 3);
                laptimebytes |= (ushort)((UInt16)(Math.Abs(rf.LapTime.Milliseconds / 100)));
                 * 
                u.updateType(usb.types.LapTime, unchecked((short)laptimebytes));
                */

                if (rf.PitLimiter)
                {
                    if ((DateTime.Now - ledBlink).TotalMilliseconds > blinkDuration)
                    {
                        if (((DateTime.Now.Millisecond / blinkDuration) % 2) > 0)
                            u.updateType(usb.types.ShiftLEDs, 0x00);
                        else
                        {
                            u.updateType(usb.types.ShiftLEDs, 0xff);
                        }
                    }
                }
                else
                {
                    ushort shiftleds = (ushort)((1 << (Int16)Math.Floor(rf.ShiftIndicator * 7.1f)) - 1);
                    u.updateType(usb.types.ShiftLEDs, unchecked((short)shiftleds));
                }
            }
            else if (ir != null || rf != null)
            {
                USBStatusValue.Text = "not connected";
                u.initialize();
                if(u.isInitialized)
                    u.updateType(usb.types.Backlight, unchecked((short)(1024 - ((Single)BacklightValue.Value * 10.24))));
            }
        }

        private void BacklightValue_ValueChanged(object sender, EventArgs e)
        {
            u.updateType(usb.types.Backlight, unchecked((short)(1024 - ((Single)BacklightValue.Value * 10.24))));
        }
    }
}
