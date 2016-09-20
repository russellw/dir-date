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
            Console.WriteLine("dir-date [patterns]");
            Console.WriteLine("");
            Console.WriteLine("Patterns as in:");
            Console.WriteLine("https://github.com/mganss/Glob.cs");
        }

        static Dictionary<string, DateTime> times = new Dictionary<string, DateTime>();

        static DateTime time(string path)
        {
            DateTime t;
            if (times.TryGetValue(path, out t))
                return t;
            var info = new FileInfo(path);
            if ((info.Attributes & FileAttributes.Directory) == 0)
                t = info.LastWriteTime;
            else
            {
                t = DateTime.MinValue;
                foreach (string entry in Directory.GetDirectories(path))
                {
                    var u = time(entry);
                    if (u > t)
                        t = u;
                }
                foreach (string entry in Directory.GetFiles(path))
                {
                    var u = time(entry);
                    if (u > t)
                        t = u;
                }
            }
            times.Add(path, t);
            return t;
        }

        static int Main(string[] args)
        {
            var options = true;
            var patterns = new List<string>();
            foreach (var arg in args)
            {
                var s = arg;
                if (!options)
                {
                    patterns.Add(s);
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
                    patterns.Add(s);
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
            if (patterns.Count == 0)
                patterns.Add("*");
            var files = new List<string>();
            foreach (var pattern in patterns)
                foreach (var file in Glob.Glob.ExpandNames(pattern))
                    files.Add(file);
            files.Sort((a, b) => time(a).CompareTo(time(b)));
            foreach (var file in files)
                Console.WriteLine("{0}  {1}", time(file), file);
            return 0;
        }
    }
}
