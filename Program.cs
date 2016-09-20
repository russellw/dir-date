using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dir_date
{
    class Program
    {
        static void help()
        {
            Console.WriteLine("Directory listing by date of contained files");
            Console.WriteLine("");
            Console.WriteLine("dir-date [pattern]");
            Console.WriteLine("");
            Console.WriteLine("Patterns as in:");
            Console.WriteLine("https://github.com/mganss/Glob.cs");
        }

        static int Main(string[] args)
        {
            var options = true;
            var files = new List<string>();
            foreach (var arg in args)
            {
                var s = arg;
                if (!options)
                {
                    files.Add(s);
                    continue;
                }
                if (s == "")
                    continue;
                if (s == "--")
                {
                    options = false;
                    continue;
                }
                if (Path.DirectorySeparatorChar == '\\' && s[0] == '/')
                    s = "-" + s.Substring(1);
                if (s[0] != '-')
                {
                    files.Add(s);
                    continue;
                }
                if (s.StartsWith("--"))
                    s = s.Substring(1);
                switch (s)
                {
                    case "-?":
                    case "-h":
                    case "-help":
                        help();
                        return 0;
                    case "-V":
                    case "-v":
                    case "-version":
                        Console.WriteLine("dir-date {0}", Assembly.GetExecutingAssembly().GetName().Version);
                        return 0;
                    default:
                        Console.WriteLine("{0}: unknown option", arg);
                        return 1;
                }
            }
            return 0;
        }
    }
}
