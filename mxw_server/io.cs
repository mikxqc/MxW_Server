using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mxw_server
{
    class io
    {
        public static string ahdump = "ah-dump.json";

        public static void CleanDump()
        {
            if (File.Exists(ahdump))
            {
                File.Delete(ahdump);
            }
        }

        public static void CheckUID()
        {
            if (!Directory.Exists("uid"))
            {
                Directory.CreateDirectory("uid");
            }
        }

        public static void CheckItems()
        {
            if (!Directory.Exists("items"))
            {
                Directory.CreateDirectory("items");
            }
        }

        public static void CheckRealms()
        {
            if (!Directory.Exists("realms"))
            {
                Directory.CreateDirectory("realms");
            }
        }

        public static void ClearUID()
        {
            DirectoryInfo di = new DirectoryInfo(@"uid");
            FileInfo[] files = di.GetFiles("*.json")
                                 .Where(p => p.Extension == ".json").ToArray();
            foreach (FileInfo file in files)
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    File.Delete(file.FullName);
                }
                catch { }
        }
    }
}
