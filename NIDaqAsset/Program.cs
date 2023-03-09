using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using NationalInstruments.DAQmx;
using Newtonsoft.Json;


namespace NIDaqAsset
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] getAllDaqIDs = DaqSystem.Local.Devices;
            List<DaqDevices> daqList = getAllDaqAssets(getAllDaqIDs);

            Console.WriteLine(JsonConvert.SerializeObject(daqList));
        }


        public static List<DaqDevices> getAllDaqAssets(string[] allDevicesNames)
        {
            List<DaqDevices> DAQs = new List<DaqDevices>();

            foreach (string name in allDevicesNames)
            {
                try
                {
                    DAQs.Add(new DaqDevices(name));
                }
                catch(Exception e)
                {
                    WriteLog.createJSONLog(e.Message, e.StackTrace, "NIDaqerror");
                }
            }
            return DAQs;
        }

        
    }
}
