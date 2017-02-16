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
            public string gentime { get; set; }
            public List<LIST> list { get; set; }
        }

        public class LIST
        {
            public string owner { get; set; }
            public int qty { get; set; }
            public int tg { get; set; }
            public int ts { get; set; }
            public int tc { get; set; }
            public int ug { get; set; }
            public int us { get; set; }
            public int uc { get; set; }
            public string time { get; set; }
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
                            Uri uri = new Uri(String.Format(@"https://eu.api.battle.net/wow/item/{0}?locale={1}&apikey={2}", i, main.locale, api));
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

                    var data = new List<LIST>();

                    DataRow[] result = genuid.aucds.Tables["auclist"].Select("item ='" + i + "'");
                    foreach (DataRow row in result)
                    {
                        data.Add(new LIST {
                            owner = row["owner"].ToString(),
                            qty = Convert.ToInt32(row["qty"]),
                            tg = Convert.ToInt32(row["agold"]),
                            ts = Convert.ToInt32(row["asilver"]),
                            tc = Convert.ToInt32(row["acopper"]),
                            ug = Convert.ToInt32(row["ugold"]),
                            us = Convert.ToInt32(row["usilver"]),
                            uc = Convert.ToInt32(row["ucopper"]),
                            time = (row["time"].ToString())
                        });
                    }

                    string days = "";
                    string months = "";
                    string seconds = "";
                    string minutes = "";
                    string hours = "";
                    if (DateTime.Now.Day < 10)
                    {
                        days = String.Format("0{0}", DateTime.Now.Day);
                    }
                    else
                    {
                        days = DateTime.Now.Day.ToString();
                    }
                    if (DateTime.Now.Month < 10)
                    {
                        months = String.Format("0{0}", DateTime.Now.Month);
                    }
                    else
                    {
                        months = DateTime.Now.Month.ToString();
                    }
                    if (DateTime.Now.Second < 10)
                    {
                        seconds = String.Format("0{0}", DateTime.Now.Second);
                    }
                    else
                    {
                        seconds = DateTime.Now.Second.ToString();
                    }

                    if (DateTime.Now.Minute < 10)
                    {
                        minutes = String.Format("0{0}", DateTime.Now.Minute);
                    }
                    else
                    {
                        minutes = DateTime.Now.Minute.ToString();
                    }

                    if (DateTime.Now.Hour < 10)
                    {
                        hours = String.Format("0{0}", DateTime.Now.Hour);
                    }
                    else
                    {
                        hours = DateTime.Now.Hour.ToString();
                    }

                    string gentime = string.Format("{0}/{1}/{2} {3}:{4}:{5}", days, months, DateTime.Now.Year, hours, minutes, seconds);

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
                        mic = mic,
                        list = data,
                        gentime = gentime
                    };

                    using (FileStream fs = File.Open(string.Format(@"{0}/{1}.json", main.genpath, i), FileMode.Append))
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
