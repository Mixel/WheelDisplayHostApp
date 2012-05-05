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
        UsbDevice device;

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

            LapTime = 0x10,
            Delta = 0x11,

            Gear = 0x1f,
        };

        public usb()
        {
            Boolean found = false;

            // Loop until we found our device
            while (!found)
            {
                UsbRegDeviceList allDevices = UsbDevice.AllDevices;
                foreach (UsbRegistry usbRegistry in allDevices)
                {
                    UsbDevice MyUsbDevice;
                    if (usbRegistry.Open(out MyUsbDevice))
                    {
                        this.device = MyUsbDevice;
                        found = true;
                    }
                }

                System.Threading.Thread.Sleep(100); // wait
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
