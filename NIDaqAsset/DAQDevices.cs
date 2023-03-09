using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.DAQmx;

namespace NIDaqAsset
{
    public class DaqDevices
    {
        private Device device;
        public string model { get; set; }
        public string serial { get; set; }

        public DaqDevices()
        {
            this.device = null;
            this.model = null;
            this.serial = null;
        }
        public DaqDevices(string name)
        {
            this.device = DaqSystem.Local.LoadDevice(name);
            this.model = GetProductNumber();
            this.serial = GetSerialNumber();
            this.device.Dispose();
        }
        private string GetProductNumber()
        {
            return this.device.ProductType;
        }

        private string GetSerialNumber()
        {
            return this.device.SerialNumber.ToString("X8");
        }
    }
}
