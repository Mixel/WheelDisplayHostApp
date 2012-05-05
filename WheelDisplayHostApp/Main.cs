﻿using System;
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
            ir.updateData();

            if(ir.Gear < 0)
                GearValue.Text = "R";
            else if (ir.Gear == 0)
                GearValue.Text = "N";
            else
                GearValue.Text = ir.Gear.ToString();

            RPMValue.Text = ir.RPM.ToString();
            SpeedValue.Text = ir.Speed.ToString() + " km/h";
            FuelValue.Text = ir.Fuel.ToString() + " ltr";
            FuelNeededValue.Text = ir.FuelNeeded.ToString() + " ltr";
            LapValue.Text = ir.Lap.ToString();
            LapsRemainingValue.Text = ir.LapsRemaining.ToString();
            PositionValue.Text = ir.Position.ToString();
            LapTimeValue.Text = ir.LapTime.Minutes.ToString() + ":" + ir.LapTime.Seconds.ToString("D2") + "." + ir.LapTime.Milliseconds.ToString("D3");

        }

        private void usbUpdater_Tick(object sender, EventArgs e)
        {
            if(ir.Gear < 0)
                u.updateType(usb.types.Gear, 8);
            else
                u.updateType(usb.types.Gear, (short)(ir.Gear-1));

            u.updateType(usb.types.Speed, (short)ir.Speed);
            u.updateType(usb.types.RPM, (short)ir.RPM);
            u.updateType(usb.types.Lap, (short)ir.Lap);
            u.updateType(usb.types.Fuel, (short)ir.Fuel);
        }
    }
}
