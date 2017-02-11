using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxw_server
{
    class main
    {
        internal static string region = "";
        internal static string realm = "";
        internal static string api = "";
        internal static string locale = "";
        internal static string genpath = "";
        internal static string ahdump = "";

        public static string version = "1.4.0";
        public static string build = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileBuildPart.ToString();
        public static string commit = ThisAssembly.Git.Commit;
        public static string branch = ThisAssembly.Git.Branch;

        public static bool loop = true;
        public static string st = "00";
        public static string lst = "";
        public static int retry = 0;

        private static int sleep = 10000;

        public class RootObject
        {
            public string region { get; set; }
            public string realm { get; set; }
            public string api { get; set; }
            public string locale { get; set; }
        }

        public class SETTINGS
        {
            public string region { get; set; }
            public string realm { get; set; }
            public string api { get; set; }
            public string locale { get; set; }
        }

        static void Main(string[] args)
        {
            while (loop)
            {
                switch(st)
                {
                    //STATE 01 | Clean up and init the basic stuff
                    case "00":
                        msg.Splash();
                        if (args.Length > 0)
                        {
                            if (args[0] != "" && args[1] != "")
                            {
                                region = args[0];
                                realm = args[1];
                                st = "01";
                            }
                            else
                            {
                                msg.CM("Usage: mxw_server.exe {region} {realm}", true, 3);
                                msg.CM("Exemple: mxw_server.exe us hyjal", true, 3);
                                msg.CM("Press any key to exit...", true, 1);
                                Console.ReadKey();
                                loop = false;
                            }
                        }
                        else
                        {
                            msg.CM("Usage: mxw_server.exe {region} {realm}", true, 3);
                            msg.CM("Exemple: mxw_server.exe us hyjal", true, 3);
                            msg.CM("Press any key to exit...", true, 1);
                            Console.ReadKey();
                            loop = false;
                        }                  
                        break;
                    case "01":
                        lst = "01";                       
                        msg.CM("Cleaning the latest ah dump...", true, 1);
                        if (File.Exists("settings.json"))
                        {
                            if (CheckSettings())
                            {
                                msg.CM("Reading the settings...", true, 1);
                                ReadSettings();
                                msg.CM(string.Format("Region: {0}", region), true, 2);
                                msg.CM(string.Format("Realm: {0}", realm), true, 2);
                                msg.CM(string.Format("API: {0}", api), true, 2);
                                msg.CM(string.Format("Locale: {0}", locale), true, 2);
                                msg.CM("Checking if worldofwarcraft.com is alive...", true, 1);
                                if (!web.CheckWeb("http://worldofwarcraft.com"))
                                {
                                    st = "sleep_error";
                                }
                                else
                                {
                                    msg.CM("worldofwarcraft.com is online.", true, 2);
                                    st = "02";
                                    //Console.ReadKey();
                                }
                            }
                            else
                            {
                                st = "settings_blank_error";
                            }                            
                        }
                        else
                        {
                            st = "settings_error";
                        }                                                             
                        break;

                    //STATE 02 | Get latest AH dump url and download it
                    case "02":
                        lst = "02";
                        msg.CM("Requesting latest json url from BNet...", true, 1);
                        string jsonurl = ahurl.GetURL(region, realm, api);
                        msg.CM(string.Format("URL: {0}", jsonurl), true, 2);
                        msg.CM("Downloading the latest AH dump from BNet...", true, 1);
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFile(jsonurl, ahdump);
                        }
                        st = "03";
                        break;

                    //STATE 03 | Extract every unique item
                    case "03":
                        lst = "03";
                        msg.CM("Checking/Creating unique id folder...", true, 1);
                        io.CheckUID();
                        io.ClearUID();
                        msg.CM("Checking/Creating items folder...", true, 1);
                        io.CheckItems();
                        msg.CM("Preparing the auctions data...", true, 1);
                        genuid.GenUID();
                        msg.CM("Generating the items jsons...", true, 1);
                        genjson.GenJSON();
                        loop = false;
                        break;

                    //STATE SLEEP_ERROR | Wait before retrying the last failing state
                    case "sleep_error":
                        msg.CM(string.Format("Waiting for {0} seconds before retrying...", sleep / 1000), true, 3);
                        Thread.Sleep(sleep);
                        st = lst;
                        break;

                    //STATE SETTINGS_ERROR | No settings.json found. Downloading a blank one.
                    case "settings_error":
                        msg.CM("No settings.json found. Creating a blank one...", true, 3);
                        CreateSettings();
                        msg.CM("Edit the blank settings.json and run the server again.", true, 3);
                        msg.CM("Press any key to exit...", true, 1);
                        Console.ReadKey();
                        loop = false;
                        break;

                    //STATE SETTINGS_BLANK_ERROR | Blank settings.json found.
                    case "settings_blank_error":
                        msg.CM("Blank settings.json found. Fill it before executing the server.", true, 3);
                        msg.CM("Edit the blank settings.json and run the server again.", true, 3);
                        msg.CM("Press any key to exit...", true, 1);
                        Console.ReadKey();
                        loop = false;
                        break;
                }
            }
        }

        private static void ReadSettings()
        {
            RootObject j = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText("settings.json"));
            api = j.api;
            locale = j.locale;
            genpath = string.Format("{0}-{1}", region, realm);
            ahdump = string.Format("{0}-{1}_dump.json", main.region, main.realm);
        }

        private static bool CheckSettings()
        {
            RootObject j = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText("settings.json"));

            if (j.api == "fill me")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void CreateSettings()
        {
            SETTINGS sjson = new SETTINGS()
            {
                api = "fill me",
                locale = "fill me"
            };

            using (FileStream fs = File.Open("settings.json", FileMode.CreateNew))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, sjson);
            }
        }
    }
}
