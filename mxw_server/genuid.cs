using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mxw_server
{
    class genuid
    {
        public class Realm
        {
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Auction
        {
            public Int64 auc { get; set; }
            public Int64 item { get; set; }
            public string owner { get; set; }
            public string ownerRealm { get; set; }
            public Int64 bid { get; set; }
            public Int64 buyout { get; set; }
            public int quantity { get; set; }
            public string timeLeft { get; set; }
            public int rand { get; set; }
            public Int64 seed { get; set; }
            public int context { get; set; }
        }

        public class UID
        {
            public Int64 auc { get; set; }
            public Int64 item { get; set; }
            public Int64 buyout { get; set; }
        }

        public class RootObject
        {
            public List<Realm> realms { get; set; }
            public List<Auction> auctions { get; set; }
            public List<UID> uid { get; set; }
        }

        public static DataSet aucds = new DataSet();

        public static void GenUID()
        {
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(File.ReadAllText(main.ahdump));
            DataTable table = ds.Tables["auctions"];

            DataTable auctb = new DataTable("auclist");

            auctb.Columns.Add(new DataColumn("auc", typeof(Int64)));
            auctb.Columns.Add(new DataColumn("item", typeof(Int64)));
            auctb.Columns.Add(new DataColumn("owner", typeof(string)));
            auctb.Columns.Add(new DataColumn("ugold", typeof(Int64)));
            auctb.Columns.Add(new DataColumn("usilver", typeof(Int64)));
            auctb.Columns.Add(new DataColumn("ucopper", typeof(Int64)));
            auctb.Columns.Add(new DataColumn("agold", typeof(Int64)));
            auctb.Columns.Add(new DataColumn("asilver", typeof(Int64)));
            auctb.Columns.Add(new DataColumn("acopper", typeof(Int64)));
            auctb.Columns.Add(new DataColumn("qty", typeof(int)));
            auctb.Columns.Add(new DataColumn("time", typeof(string)));

            foreach (DataRow dr in table.Rows)
            {
                DataRow nr = auctb.NewRow();

                Int64 v = Convert.ToInt64(dr["buyout"]);
                Int64 fv;
                Int64 qt = Convert.ToInt64(dr["quantity"]);
                Int64 gold;
                Int64 silver;
                Int64 copper;
                Int64 agold;
                Int64 asilver;
                Int64 acopper;

                if (qt != 1)
                {
                    gold = (v / 10000) / qt;
                    silver = ((v / qt) % 10000) / 100;
                    copper = ((v / qt) % 10000) % 100;

                    agold = (v / 10000);
                    asilver = (v % 10000) / 100;
                    acopper = (v % 10000) % 100;
                    fv = v / qt;
                }
                else
                {
                    gold = v / 10000;
                    silver = (v % 10000) / 100;
                    copper = (v % 10000) % 100;

                    agold = gold;
                    asilver = silver;
                    acopper = copper;
                    fv = v;
                }

                string tauc = dr["timeLeft"].ToString();
                string time = "";
                switch (tauc)
                {
                    case "SHORT":
                        time = "Cour";
                        break;
                    case "MEDIUM":
                        time = "Moyen";
                        break;
                    case "LONG":
                        time = "Long";
                        break;
                    case "VERY_LONG":
                        time = "Très long";
                        break;
                }

                nr["auc"] = Convert.ToInt32(dr["auc"]);
                nr["item"] = Convert.ToInt32(dr["item"]);
                nr["owner"] = dr["owner"];
                nr["ugold"] = gold;
                nr["usilver"] = silver;
                nr["ucopper"] = copper;
                nr["agold"] = agold;
                nr["asilver"] = asilver;
                nr["acopper"] = acopper;
                nr["qty"] = Convert.ToInt32(dr["quantity"]);
                nr["time"] = time;

                auctb.Rows.Add(nr);
                
            }
            aucds.Tables.Add(auctb);
        }
    }
}
