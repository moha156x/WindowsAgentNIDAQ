using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace NIDaqAsset
{
    public class WriteLog
    {
        protected static string pcHostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
        protected static string createdAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        protected static string timestamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        protected static string createdBy = "DAQ_agent";
        protected static string configFilePath = @"C:\Intel\luma\configFiles\luma_ic_autoupdate.csv";
        protected static string sessionJSONFolder = @"C:\Intel\luma\luma_ic_ssession_LogFiles\";
        protected static string csvData;
        protected static string lumaVersion;
        protected static string autoupdatePath;
        protected static string fileSharePath;
        protected static string serverName;
        protected static List<string> serverNames;
        protected static string prefix;
        protected static string default_utcTime = "1901-01-01 00:00:00.000";


        static WriteLog()
        {
            ReadConfig();
        }

        private static void ReadConfig()
        {
            if (System.IO.File.Exists(configFilePath))
            {
                using (var reader = new System.IO.StreamReader(configFilePath))
                {
                    reader.ReadLine();
                    while ((csvData = reader.ReadLine()) != null)
                    {
                        string[] rawData = csvData.Split(',');
                        autoupdatePath = rawData[0];
                        lumaVersion = rawData[1];
                        fileSharePath = rawData[2];
                        serverName = rawData[3];
                        prefix = rawData[4];
                        break;
                    }
                }

                List<string> serves = new List<string>();
                using (var reader = new System.IO.StreamReader(configFilePath))
                {
                    reader.ReadLine();
                    while ((csvData = reader.ReadLine()) != null)
                    {
                        string[] rawData = csvData.Split(',');
                        serves.Add(rawData[3]);
                    }
                }
                serverNames = serves;
            }
            else
            {
                //if configuration file not found, exit the application gracefully.
                string stack_trace = "configuration file not found.. Exiting";
                createJSONLog(stack_trace, stack_trace, "configError");
                Environment.Exit(0);
            }
        }

        public static void createJSONLog(string message, string stack_trace, string errorName)
        {
            string sessionJSONName = prefix + "_" + pcHostname +"_"+ errorName +"_"+ timestamp;
            string sessionJSONFilePath = sessionJSONFolder + sessionJSONName + ".json";
            string jsonLogText = default(string);
            string utcTime = GetUtcTime();

            string tmpText = "{"
                                      + " \"" + "created_at" + "\"" + ":" + "\"" + createdAt + "\","
                                      + " \"" + "created_by" + "\"" + ":" + "\"" + createdBy + "\","
                                      + " \"" + "message" + "\"" + ":" + "\"" + message + "\","
                                      + " \"" + "stack_trace" + "\"" + ":" + "\"" + stack_trace + "\","
                                      + " \"" + "utc_time" + "\"" + ":" + "\"" + utcTime + "\"" +
                                    "}";
            if (!(System.IO.Directory.Exists(sessionJSONFolder)))
            {
                System.IO.Directory.CreateDirectory(sessionJSONFolder);
            }
            if (!(System.IO.File.Exists(sessionJSONFilePath)))
            {
                jsonLogText = "[" + tmpText;
                jsonLogText = jsonLogText + "]";
            }
            else
            {
                string readFile = File.ReadAllText(sessionJSONFilePath);
                int index = readFile.LastIndexOf('}');
                jsonLogText = readFile.Insert(index + 1, "," + tmpText);
            }
            System.IO.File.WriteAllText(sessionJSONFilePath, jsonLogText);
        }

        private static string GetUtcTime()
        {
            string ISO_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
            string utcTime = "";
            foreach (var server in serverNames)
            {
                //fetch utc time from webservice
                System.Net.WebClient wc = new System.Net.WebClient();
                try
                {
                    //try connect to server
                    string utcServerName = "http://" + server + "/luma/api/time";
                    string webData = wc.DownloadString(utcServerName);
                    utcTime = webData;
                    utcTime = utcTime.Replace("\"", "");                   
                    utcTime = DateTime.Parse(utcTime).ToString(ISO_FORMAT);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to connect to {0} server", server);
                    Console.WriteLine(ex.ToString());
                    return default_utcTime;
                }
            }
            return utcTime;
        }


    }
}
