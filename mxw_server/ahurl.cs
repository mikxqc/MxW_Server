using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace mxw_server
{
    class ahurl
    {
        public class JSON
        {
            public string url { get; set; }
            public long lastModified { get; set; }
        }

        public class RootObject
        {
            public List<JSON> files { get; set; }
        }

        public static string GetURL(string region, string realm, string api)
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(@"https://" + region + ".api.battle.net/wow/auction/data/" + realm + "?locale=en_US&apikey=" + api);
                RootObject j = JsonConvert.DeserializeObject<RootObject>(json);
                return j.files[0].url;
            }
        }
    }
}
