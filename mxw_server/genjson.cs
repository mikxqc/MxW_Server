using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mxw_server
{
    class genjson
    {

        public class ITEMS
        {
            public Int64 item { get; set; }
            public string name { get; set; }
            public string icon { get; set; }
            public Int64 qty { get; set; }
            public Int64 avg { get; set; }
            public Int64 mag { get; set; }
            public Int64 mig { get; set; }
            public Int64 avs { get; set; }
            public Int64 mas { get; set; }
            public Int64 mis { get; set; }
            public Int64 avc { get; set; }
            public Int64 mac { get; set; }
            public Int64 mic { get; set; }
        }

        public class RootObject
        {
            public List<ITEMS> items { get; set; }
        }

        public static void GenJSON()
        {
            //i = current item id
            for (int i = 0; i < 150000; i++)
            {
                DataRow[] rows = genuid.aucds.Tables["auclist"].Select("item ='" + i + "'");
                if (rows.Length > 0)
                {

                    bool exists = true;
                    string api = main.api;

                    if (!File.Exists(string.Format(@"items/{0}.json", i)))
                    {
                        exists = false;
                    }

                    if (!exists)
                    {
                        try
                        {
                            WebClient wc = new WebClient();
                            Uri uri = new Uri(String.Format(@"https://eu.api.battle.net/wow/item/{0}?locale=fr_FR&apikey={1}", i, api));
                            wc.DownloadFile(uri, string.Format(@"items/{0}.json", i));
                            //msg.CM(string.Format("Saved new item {0}...", i), true, 2);
                        }
                        catch (WebException wex)
                        {
                            if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)
                            {
                                msg.CM(string.Format("Item {0} doesn't exists...", i), true, 3);
                            }
                        }
                    }

                    int avg = Convert.ToInt32(genuid.aucds.Tables["auclist"].Compute("AVG(ugold)", string.Format("item = '{0}'", i)));
                    int mag = Convert.ToInt32(genuid.aucds.Tables["auclist"].Compute("MAX(ugold)", string.Format("item = '{0}'", i)));
                    int mig = Convert.ToInt32(genuid.aucds.Tables["auclist"].Compute("MIN(ugold)", string.Format("item = '{0}'", i)));

                    int avs = Convert.ToInt32(genuid.aucds.Tables["auclist"].Compute("AVG(usilver)", string.Format("item = '{0}'", i)));
                    int mas = Convert.ToInt32(genuid.aucds.Tables["auclist"].Compute("MAX(usilver)", string.Format("item = '{0}'", i)));
                    int mis = Convert.ToInt32(genuid.aucds.Tables["auclist"].Compute("MIN(usilver)", string.Format("item = '{0}'", i)));

                    int avc = Convert.ToInt32(genuid.aucds.Tables["auclist"].Compute("AVG(ucopper)", string.Format("item = '{0}'", i)));
                    int mac = Convert.ToInt32(genuid.aucds.Tables["auclist"].Compute("MAX(ucopper)", string.Format("item = '{0}'", i)));
                    int mic = Convert.ToInt32(genuid.aucds.Tables["auclist"].Compute("MIN(ucopper)", string.Format("item = '{0}'", i)));

                    int qty = genuid.aucds.Tables["auclist"].Select(string.Format("item = '{0}'", i)).Length;

                    string name = getitems.GetName(i);
                    string icon = getitems.GetIcon(i);

                    //Gen unique item json
                    ITEMS ijson = new ITEMS()
                    {
                        item = i,
                        name = name,
                        icon = icon,
                        qty = qty,
                        avg = avg,
                        mag = mag,
                        mig = mig,
                        avs = avs,
                        mas = mas,
                        mis = mis,
                        avc = avc,
                        mac = mac,
                        mic = mic
                    };

                    using (FileStream fs = File.Open(string.Format(@"uid/{0}.json", i), FileMode.CreateNew))
                    using (StreamWriter sw = new StreamWriter(fs))
                    using (JsonWriter jw = new JsonTextWriter(sw))
                    {
                        jw.Formatting = Formatting.Indented;
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jw, ijson);
                    }
                }
            }
        }
    }
}
