namespace WheelDisplayHostApp
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FuelConsumption = new System.Windows.Forms.Label();
            this.PreviousLap = new System.Windows.Forms.Label();
            this.Delta = new System.Windows.Forms.Label();
            this.RPM = new System.Windows.Forms.Label();
            this.LapTime = new System.Windows.Forms.Label();
            this.Position = new System.Windows.Forms.Label();
            this.LapsRemaining = new System.Windows.Forms.Label();
            this.Lap = new System.Windows.Forms.Label();
            this.FuelNeeded = new System.Windows.Forms.Label();
            this.Fuel = new System.Windows.Forms.Label();
            this.Speed = new System.Windows.Forms.Label();
            this.Gear = new System.Windows.Forms.Label();
            this.FuelConsumptionValue = new System.Windows.Forms.Label();
            this.PreviousLapValue = new System.Windows.Forms.Label();
            this.DeltaValue = new System.Windows.Forms.Label();
            this.RPMValue = new System.Windows.Forms.Label();
            this.LapTimeValue = new System.Windows.Forms.Label();
            this.PositionValue = new System.Windows.Forms.Label();
            this.LapsRemainingValue = new System.Windows.Forms.Label();
            this.LapValue = new System.Windows.Forms.Label();
            this.FuelNeededValue = new System.Windows.Forms.Label();
            this.FuelValue = new System.Windows.Forms.Label();
            this.SpeedValue = new System.Windows.Forms.Label();
            this.GearValue = new System.Windows.Forms.Label();
            this.apiUpdater = new System.Windows.Forms.Timer(this.components);
            this.usbUpdater = new System.Windows.Forms.Timer(this.components);
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.USBStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.USBStatusValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.SimStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.SimStatusValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.RPMLeds = new System.Windows.Forms.Label();
            this.RPMLedsValue = new System.Windows.Forms.ProgressBar();
            this.BestLap = new System.Windows.Forms.Label();
            this.BestLapValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.BestLap);
            this.splitContainer1.Panel1.Controls.Add(this.RPMLeds);
            this.splitContainer1.Panel1.Controls.Add(this.FuelConsumption);
            this.splitContainer1.Panel1.Controls.Add(this.PreviousLap);
            this.splitContainer1.Panel1.Controls.Add(this.Delta);
            this.splitContainer1.Panel1.Controls.Add(this.RPM);
            this.splitContainer1.Panel1.Controls.Add(this.LapTime);
            this.splitContainer1.Panel1.Controls.Add(this.Position);
            this.splitContainer1.Panel1.Controls.Add(this.LapsRemaining);
            this.splitContainer1.Panel1.Controls.Add(this.Lap);
            this.splitContainer1.Panel1.Controls.Add(this.FuelNeeded);
            this.splitContainer1.Panel1.Controls.Add(this.Fuel);
            this.splitContainer1.Panel1.Controls.Add(this.Speed);
            this.splitContainer1.Panel1.Controls.Add(this.Gear);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.BestLapValue);
            this.splitContainer1.Panel2.Controls.Add(this.RPMLedsValue);
            this.splitContainer1.Panel2.Controls.Add(this.FuelConsumptionValue);
            this.splitContainer1.Panel2.Controls.Add(this.PreviousLapValue);
            this.splitContainer1.Panel2.Controls.Add(this.DeltaValue);
            this.splitContainer1.Panel2.Controls.Add(this.RPMValue);
            this.splitContainer1.Panel2.Controls.Add(this.LapTimeValue);
            this.splitContainer1.Panel2.Controls.Add(this.PositionValue);
            this.splitContainer1.Panel2.Controls.Add(this.LapsRemainingValue);
            this.splitContainer1.Panel2.Controls.Add(this.LapValue);
            this.splitContainer1.Panel2.Controls.Add(this.FuelNeededValue);
            this.splitContainer1.Panel2.Controls.Add(this.FuelValue);
            this.splitContainer1.Panel2.Controls.Add(this.SpeedValue);
            this.splitContainer1.Panel2.Controls.Add(this.GearValue);
            this.splitContainer1.Panel2MinSize = 20;
            this.splitContainer1.Size = new System.Drawing.Size(292, 313);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // FuelConsumption
            // 
            this.FuelConsumption.AutoSize = true;
            this.FuelConsumption.Location = new System.Drawing.Point(3, 104);
            this.FuelConsumption.Name = "FuelConsumption";
            this.FuelConsumption.Padding = new System.Windows.Forms.Padding(3);
            this.FuelConsumption.Size = new System.Drawing.Size(96, 19);
            this.FuelConsumption.TabIndex = 11;
            this.FuelConsumption.Text = "Fuel consumption";
            // 
            // PreviousLap
            // 
            this.PreviousLap.AutoSize = true;
            this.PreviousLap.Location = new System.Drawing.Point(3, 199);
            this.PreviousLap.Name = "PreviousLap";
            this.PreviousLap.Padding = new System.Windows.Forms.Padding(3);
            this.PreviousLap.Size = new System.Drawing.Size(71, 19);
            this.PreviousLap.TabIndex = 10;
            this.PreviousLap.Text = "Previous lap";
            // 
            // Delta
            // 
            this.Delta.AutoSize = true;
            this.Delta.Location = new System.Drawing.Point(3, 237);
            this.Delta.Name = "Delta";
            this.Delta.Padding = new System.Windows.Forms.Padding(3);
            this.Delta.Size = new System.Drawing.Size(38, 19);
            this.Delta.TabIndex = 9;
            this.Delta.Text = "Delta";
            // 
            // RPM
            // 
            this.RPM.AutoSize = true;
            this.RPM.Location = new System.Drawing.Point(3, 28);
            this.RPM.Name = "RPM";
            this.RPM.Padding = new System.Windows.Forms.Padding(3);
            this.RPM.Size = new System.Drawing.Size(37, 19);
            this.RPM.TabIndex = 8;
            this.RPM.Text = "RPM";
            // 
            // LapTime
            // 
            this.LapTime.AutoSize = true;
            this.LapTime.Location = new System.Drawing.Point(3, 180);
            this.LapTime.Name = "LapTime";
            this.LapTime.Padding = new System.Windows.Forms.Padding(3);
            this.LapTime.Size = new System.Drawing.Size(53, 19);
            this.LapTime.TabIndex = 7;
            this.LapTime.Text = "Lap time";
            // 
            // Position
            // 
            this.Position.AutoSize = true;
            this.Position.Location = new System.Drawing.Point(3, 161);
            this.Position.Name = "Position";
            this.Position.Padding = new System.Windows.Forms.Padding(3);
            this.Position.Size = new System.Drawing.Size(50, 19);
            this.Position.TabIndex = 6;
            this.Position.Text = "Position";
            // 
            // LapsRemaining
            // 
            this.LapsRemaining.AutoSize = true;
            this.LapsRemaining.Location = new System.Drawing.Point(3, 142);
            this.LapsRemaining.Name = "LapsRemaining";
            this.LapsRemaining.Padding = new System.Windows.Forms.Padding(3);
            this.LapsRemaining.Size = new System.Drawing.Size(84, 19);
            this.LapsRemaining.TabIndex = 5;
            this.LapsRemaining.Text = "Laps remaining";
            // 
            // Lap
            // 
            this.Lap.AutoSize = true;
            this.Lap.Location = new System.Drawing.Point(3, 123);
            this.Lap.Name = "Lap";
            this.Lap.Padding = new System.Windows.Forms.Padding(3);
            this.Lap.Size = new System.Drawing.Size(31, 19);
            this.Lap.TabIndex = 4;
            this.Lap.Text = "Lap";
            // 
            // FuelNeeded
            // 
            this.FuelNeeded.AutoSize = true;
            this.FuelNeeded.Location = new System.Drawing.Point(3, 85);
            this.FuelNeeded.Name = "FuelNeeded";
            this.FuelNeeded.Padding = new System.Windows.Forms.Padding(3);
            this.FuelNeeded.Size = new System.Drawing.Size(72, 19);
            this.FuelNeeded.TabIndex = 3;
            this.FuelNeeded.Text = "Fuel needed";
            // 
            // Fuel
            // 
            this.Fuel.AutoSize = true;
            this.Fuel.Location = new System.Drawing.Point(3, 66);
            this.Fuel.Name = "Fuel";
            this.Fuel.Padding = new System.Windows.Forms.Padding(3);
            this.Fuel.Size = new System.Drawing.Size(33, 19);
            this.Fuel.TabIndex = 2;
            this.Fuel.Text = "Fuel";
            // 
            // Speed
            // 
            this.Speed.AutoSize = true;
            this.Speed.Location = new System.Drawing.Point(3, 47);
            this.Speed.Name = "Speed";
            this.Speed.Padding = new System.Windows.Forms.Padding(3);
            this.Speed.Size = new System.Drawing.Size(44, 19);
            this.Speed.TabIndex = 1;
            this.Speed.Text = "Speed";
            // 
            // Gear
            // 
            this.Gear.AutoSize = true;
            this.Gear.Location = new System.Drawing.Point(3, 9);
            this.Gear.Name = "Gear";
            this.Gear.Padding = new System.Windows.Forms.Padding(3);
            this.Gear.Size = new System.Drawing.Size(36, 19);
            this.Gear.TabIndex = 0;
            this.Gear.Text = "Gear";
            // 
            // FuelConsumptionValue
            // 
            this.FuelConsumptionValue.AutoSize = true;
            this.FuelConsumptionValue.Location = new System.Drawing.Point(3, 104);
            this.FuelConsumptionValue.Name = "FuelConsumptionValue";
            this.FuelConsumptionValue.Padding = new System.Windows.Forms.Padding(3);
            this.FuelConsumptionValue.Size = new System.Drawing.Size(19, 19);
            this.FuelConsumptionValue.TabIndex = 11;
            this.FuelConsumptionValue.Text = "0";
            // 
            // PreviousLapValue
            // 
            this.PreviousLapValue.AutoSize = true;
            this.PreviousLapValue.Location = new System.Drawing.Point(3, 199);
            this.PreviousLapValue.Name = "PreviousLapValue";
            this.PreviousLapValue.Padding = new System.Windows.Forms.Padding(3);
            this.PreviousLapValue.Size = new System.Drawing.Size(19, 19);
            this.PreviousLapValue.TabIndex = 10;
            this.PreviousLapValue.Text = "0";
            // 
            // DeltaValue
            // 
            this.DeltaValue.AutoSize = true;
            this.DeltaValue.Location = new System.Drawing.Point(3, 237);
            this.DeltaValue.Name = "DeltaValue";
            this.DeltaValue.Padding = new System.Windows.Forms.Padding(3);
            this.DeltaValue.Size = new System.Drawing.Size(19, 19);
            this.DeltaValue.TabIndex = 9;
            this.DeltaValue.Text = "0";
            // 
            // RPMValue
            // 
            this.RPMValue.AutoSize = true;
            this.RPMValue.Location = new System.Drawing.Point(3, 28);
            this.RPMValue.Name = "RPMValue";
            this.RPMValue.Padding = new System.Windows.Forms.Padding(3);
            this.RPMValue.Size = new System.Drawing.Size(19, 19);
            this.RPMValue.TabIndex = 8;
            this.RPMValue.Text = "0";
            // 
            // LapTimeValue
            // 
            this.LapTimeValue.AutoSize = true;
            this.LapTimeValue.Location = new System.Drawing.Point(3, 180);
            this.LapTimeValue.Name = "LapTimeValue";
            this.LapTimeValue.Padding = new System.Windows.Forms.Padding(3);
            this.LapTimeValue.Size = new System.Drawing.Size(19, 19);
            this.LapTimeValue.TabIndex = 7;
            this.LapTimeValue.Text = "0";
            // 
            // PositionValue
            // 
            this.PositionValue.AutoSize = true;
            this.PositionValue.Location = new System.Drawing.Point(3, 161);
            this.PositionValue.Name = "PositionValue";
            this.PositionValue.Padding = new System.Windows.Forms.Padding(3);
            this.PositionValue.Size = new System.Drawing.Size(19, 19);
            this.PositionValue.TabIndex = 6;
            this.PositionValue.Text = "0";
            // 
            // LapsRemainingValue
            // 
            this.LapsRemainingValue.AutoSize = true;
            this.LapsRemainingValue.Location = new System.Drawing.Point(3, 142);
            this.LapsRemainingValue.Name = "LapsRemainingValue";
            this.LapsRemainingValue.Padding = new System.Windows.Forms.Padding(3);
            this.LapsRemainingValue.Size = new System.Drawing.Size(19, 19);
            this.LapsRemainingValue.TabIndex = 5;
            this.LapsRemainingValue.Text = "0";
            // 
            // LapValue
            // 
            this.LapValue.AutoSize = true;
            this.LapValue.Location = new System.Drawing.Point(3, 123);
            this.LapValue.Name = "LapValue";
            this.LapValue.Padding = new System.Windows.Forms.Padding(3);
            this.LapValue.Size = new System.Drawing.Size(19, 19);
            this.LapValue.TabIndex = 4;
            this.LapValue.Text = "0";
            // 
            // FuelNeededValue
            // 
            this.FuelNeededValue.AutoSize = true;
            this.FuelNeededValue.Location = new System.Drawing.Point(3, 85);
            this.FuelNeededValue.Name = "FuelNeededValue";
            this.FuelNeededValue.Padding = new System.Windows.Forms.Padding(3);
            this.FuelNeededValue.Size = new System.Drawing.Size(19, 19);
            this.FuelNeededValue.TabIndex = 3;
            this.FuelNeededValue.Text = "0";
            // 
            // FuelValue
            // 
            this.FuelValue.AutoSize = true;
            this.FuelValue.Location = new System.Drawing.Point(3, 66);
            this.FuelValue.Name = "FuelValue";
            this.FuelValue.Padding = new System.Windows.Forms.Padding(3);
            this.FuelValue.Size = new System.Drawing.Size(19, 19);
            this.FuelValue.TabIndex = 2;
            this.FuelValue.Text = "0";
            // 
            // SpeedValue
            // 
            this.SpeedValue.AutoSize = true;
            this.SpeedValue.Location = new System.Drawing.Point(3, 47);
            this.SpeedValue.Name = "SpeedValue";
            this.SpeedValue.Padding = new System.Windows.Forms.Padding(3);
            this.SpeedValue.Size = new System.Drawing.Size(19, 19);
            this.SpeedValue.TabIndex = 1;
            this.SpeedValue.Text = "0";
            // 
            // GearValue
            // 
            this.GearValue.AutoSize = true;
            this.GearValue.Location = new System.Drawing.Point(3, 9);
            this.GearValue.Name = "GearValue";
            this.GearValue.Padding = new System.Windows.Forms.Padding(3);
            this.GearValue.Size = new System.Drawing.Size(19, 19);
            this.GearValue.TabIndex = 0;
            this.GearValue.Text = "0";
            // 
            // apiUpdater
            // 
            this.apiUpdater.Enabled = true;
            this.apiUpdater.Interval = 16;
            this.apiUpdater.Tick += new System.EventHandler(this.apiUpdater_Tick);
            // 
            // usbUpdater
            // 
            this.usbUpdater.Enabled = true;
            this.usbUpdater.Tick += new System.EventHandler(this.usbUpdater_Tick);
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.USBStatus,
            this.USBStatusValue,
            this.SimStatus,
            this.SimStatusValue});
            this.StatusBar.Location = new System.Drawing.Point(0, 291);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(292, 22);
            this.StatusBar.TabIndex = 1;
            this.StatusBar.Text = "statusStrip1";
            // 
            // USBStatus
            // 
            this.USBStatus.Name = "USBStatus";
            this.USBStatus.Size = new System.Drawing.Size(30, 17);
            this.USBStatus.Text = "USB:";
            // 
            // USBStatusValue
            // 
            this.USBStatusValue.Name = "USBStatusValue";
            this.USBStatusValue.Size = new System.Drawing.Size(76, 17);
            this.USBStatusValue.Text = "not connected";
            // 
            // SimStatus
            // 
            this.SimStatus.Name = "SimStatus";
            this.SimStatus.Size = new System.Drawing.Size(27, 17);
            this.SimStatus.Text = "Sim:";
            // 
            // SimStatusValue
            // 
            this.SimStatusValue.Name = "SimStatusValue";
            this.SimStatusValue.Size = new System.Drawing.Size(76, 17);
            this.SimStatusValue.Text = "not connected";
            // 
            // RPMLeds
            // 
            this.RPMLeds.AutoSize = true;
            this.RPMLeds.Location = new System.Drawing.Point(3, 256);
            this.RPMLeds.Name = "RPMLeds";
            this.RPMLeds.Padding = new System.Windows.Forms.Padding(3);
            this.RPMLeds.Size = new System.Drawing.Size(59, 19);
            this.RPMLeds.TabIndex = 12;
            this.RPMLeds.Text = "RPM leds";
            // 
            // RPMLedsValue
            // 
            this.RPMLedsValue.Location = new System.Drawing.Point(3, 256);
            this.RPMLedsValue.Name = "RPMLedsValue";
            this.RPMLedsValue.Size = new System.Drawing.Size(70, 19);
            this.RPMLedsValue.TabIndex = 12;
            // 
            // BestLap
            // 
            this.BestLap.AutoSize = true;
            this.BestLap.Location = new System.Drawing.Point(3, 218);
            this.BestLap.Name = "BestLap";
            this.BestLap.Padding = new System.Windows.Forms.Padding(3);
            this.BestLap.Size = new System.Drawing.Size(51, 19);
            this.BestLap.TabIndex = 13;
            this.BestLap.Text = "Best lap";
            // 
            // BestLapValue
            // 
            this.BestLapValue.AutoSize = true;
            this.BestLapValue.Location = new System.Drawing.Point(3, 218);
            this.BestLapValue.Name = "BestLapValue";
            this.BestLapValue.Padding = new System.Windows.Forms.Padding(3);
            this.BestLapValue.Size = new System.Drawing.Size(19, 19);
            this.BestLapValue.TabIndex = 13;
            this.BestLapValue.Text = "0";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 313);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Main";
            this.Text = "Wheel Display Host";
            this.Load += new System.EventHandler(this.Main_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label Speed;
        private System.Windows.Forms.Label Gear;
        private System.Windows.Forms.Label SpeedValue;
        private System.Windows.Forms.Label GearValue;
        private System.Windows.Forms.Timer apiUpdater;
        private System.Windows.Forms.Label Fuel;
        private System.Windows.Forms.Label FuelValue;
        private System.Windows.Forms.Label FuelNeeded;
        private System.Windows.Forms.Label FuelNeededValue;
        private System.Windows.Forms.Label LapsRemaining;
        private System.Windows.Forms.Label Lap;
        private System.Windows.Forms.Label LapsRemainingValue;
        private System.Windows.Forms.Label LapValue;
        private System.Windows.Forms.Label Position;
        private System.Windows.Forms.Label PositionValue;
        private System.Windows.Forms.Label LapTime;
        private System.Windows.Forms.Label LapTimeValue;
        private System.Windows.Forms.Timer usbUpdater;
        private System.Windows.Forms.Label RPM;
        private System.Windows.Forms.Label RPMValue;
        private System.Windows.Forms.Label Delta;
        private System.Windows.Forms.Label DeltaValue;
        private System.Windows.Forms.Label PreviousLap;
        private System.Windows.Forms.Label PreviousLapValue;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel USBStatus;
        private System.Windows.Forms.ToolStripStatusLabel USBStatusValue;
        private System.Windows.Forms.ToolStripStatusLabel SimStatus;
        private System.Windows.Forms.ToolStripStatusLabel SimStatusValue;
        private System.Windows.Forms.Label FuelConsumption;
        private System.Windows.Forms.Label FuelConsumptionValue;
        private System.Windows.Forms.Label RPMLeds;
        private System.Windows.Forms.ProgressBar RPMLedsValue;
        private System.Windows.Forms.Label BestLap;
        private System.Windows.Forms.Label BestLapValue;
    }
}

