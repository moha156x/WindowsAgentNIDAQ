using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NationalInstruments.DAQmx;

namespace Intel.Luma.DAQUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void getAllDeviceIDs()
        {
            string[] deviceIDs = DaqSystem.Local.Devices;
            //Assert
        }

    }
}
