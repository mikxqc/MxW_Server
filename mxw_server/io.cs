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

        public static void CleanDump()
        {
            if (File.Exists(main.ahdump))
            {
                File.Delete(main.ahdump);
            }
        }

        public static void CheckUID()
        {
            if (!Directory.Exists(main.genpath))
            {
                Directory.CreateDirectory(main.genpath);
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
            DirectoryInfo di = new DirectoryInfo(main.genpath);
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
