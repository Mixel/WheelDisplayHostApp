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
        private iracing ir;
        private usb u;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ir = new iracing();
            u = new usb(); 
        }

        private void apiUpdater_Tick(object sender, EventArgs e)
        {
            if (ir != null && ir.isInitialized)
            {
                SimStatusValue.Text = "connected";
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
            else
            {
                SimStatusValue.Text = "not connected";
                ir.initialize();
            }
        }

        private void usbUpdater_Tick(object sender, EventArgs e)
        {
            if (u != null && u.isInitialized && ir != null)
            {
                USBStatusValue.Text = "connected";

                if (ir.Gear < 0)
                    u.updateType(usb.types.Gear, 8);
                else
                    u.updateType(usb.types.Gear, (short)(ir.Gear - 1));
                
                u.updateType(usb.types.Speed, (short)ir.Speed);
                u.updateType(usb.types.RPM, (short)ir.RPM);
                u.updateType(usb.types.Lap, (short)ir.Lap);
                u.updateType(usb.types.Fuel, (short)ir.Fuel);
                u.updateType(usb.types.FuelNeeded, (short)ir.FuelNeeded);
                u.updateType(usb.types.LapsRemaining, (short)ir.LapsRemaining);
                
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
            }
            else if (ir != null)
            {
                USBStatusValue.Text = "not connected";
                u.initialize();
            }
        }
    }
}
