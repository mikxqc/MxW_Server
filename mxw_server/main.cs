using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxw_server
{
    class main
    {
        internal static string region = "us";
        internal static string realm = "hyjal";
        internal static string api = "kx6ytjwqe5t5abjmuxdsymu2a7q2dedg";

        public static string version = "1.0.1";
        public static string version_type = "beta";

        public static bool loop = true;
        public static string st = "01";
        public static string lst = "";
        public static int retry = 0;

        private static int sleep = 10000;

        static void Main(string[] args)
        {
            while (loop)
            {
                switch(st)
                {
                    //STATE 01 | Clean up and init the basic stuff
                    case "01":
                        lst = "01";
                        msg.Splash();
                        msg.CM("Cleaning the latest ah dump...", true, 1);
                        //io.CleanDump();
                        msg.CM("Checking if worldofwarcraft.com is alive...", true, 1);                
                        if (!web.CheckWeb("http://worldofwarcraft.com"))
                        {
                            st = "sleep_error";
                        }
                        else
                        {
                            msg.CM("worldofwarcraft.com is online.", true, 2);
                            st = "02";
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
                            wc.DownloadFile(jsonurl, io.ahdump);
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
                }
            }
        }
    }
}
