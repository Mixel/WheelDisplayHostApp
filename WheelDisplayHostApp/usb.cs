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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

namespace WheelDisplayHostApp
{
    class usb
    {
        // private
        private UsbDevice device;

        // public
        public Boolean isInitialized { get {
            if (device == null)
                return false;
            else
                return device.UsbRegistryInfo.IsAlive;
        } set { } }

        public enum types
        {
            Null = 0x00,
            Speed = 0x01,
            Fuel = 0x02,
            FuelNeeded = 0x03,
            Lap = 0x04,
            LapsRemaining = 0x05,
            Position = 0x06,
            RPM = 0x07,
            ShiftLEDs = 0x08,
            Backlight = 0x09,

            LapTime = 0x10,
            Delta = 0x11,

            Gear = 0x1f,
        };

        public void initialize()
        {
            UsbRegDeviceList allDevices = UsbDevice.AllDevices;
            foreach (UsbRegistry usbRegistry in allDevices)
            {
                UsbDevice MyUsbDevice;
                if (usbRegistry.Open(out MyUsbDevice))
                {
                    if (MyUsbDevice.Info.Descriptor.VendorID == 0x03eb && 
                        MyUsbDevice.Info.Descriptor.ProductID == 0x2047)
                    {
                        this.device = MyUsbDevice;
                    }
                }
            }
        }

        private void tx(byte bRequest, short wValue)
        {
            byte[] buffer = new byte[256];
            int transferred = 0;

            UsbSetupPacket setup = new UsbSetupPacket((byte)UsbRequestType.TypeVendor, bRequest, wValue, 0x00, 0x00);
            bool result = this.device.ControlTransfer(ref setup, buffer, 0x0000, out transferred);
        }

        public void updateType(types type, short value)
        {
            tx((byte)type, value);
        }
    }
}
