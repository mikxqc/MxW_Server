using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace mxw_server
{
    class msg
    {
        public static void CM(string msg, bool time, int color)
        {
            string date = String.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            //color switch
            ConsoleColor cc = ConsoleColor.White;
            switch (color)
            {
                case 0:
                    //nothing
                    break;
                case 1:
                    cc = ConsoleColor.Cyan;
                    break;
                case 2:
                    cc = ConsoleColor.Green;
                    break;
                case 3:
                    cc = ConsoleColor.Red;
                    break;
            }

            if (time)
            {
                Console.ForegroundColor = cc;
                Console.WriteLine(String.Format("[{0}] {1}", date, msg));
            }
            else
            {
                Console.ForegroundColor = cc;
                Console.WriteLine(String.Format("{0}", msg));
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ConsoleMessageWrite(string msg, bool time, int color)
        {
            string date = String.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            //color switch
            ConsoleColor cc = ConsoleColor.White;
            switch (color)
            {
                case 0:
                    //nothing
                    break;
                case 1:
                    cc = ConsoleColor.Cyan;
                    break;
                case 2:
                    cc = ConsoleColor.Green;
                    break;
                case 3:
                    cc = ConsoleColor.Red;
                    break;
            }

            if (time)
            {
                Console.ForegroundColor = cc;
                Console.WriteLine(String.Format("[{0}] {1}", date, msg));
            }
            else
            {
                Console.ForegroundColor = cc;
                Console.WriteLine(String.Format(""));
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Splash()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("");
            Console.WriteLine(@"  __  __     __        __   ");
            Console.WriteLine(@" |  \/  |_  _\ \      / /__ ");
            Console.WriteLine(@" | |\/| \ \/ /\ \ /\ / / __|");
            Console.WriteLine(@" | |  | |>  <  \ V  V /\__ \");
            Console.WriteLine(@" |_|  |_/_/\_\  \_/\_/ |___/");
            Console.WriteLine(String.Format("  MxW Server {0}[Build {1}][Git {2}][Branch {3}]", main.version, main.build, main.commit, main.branch));
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
