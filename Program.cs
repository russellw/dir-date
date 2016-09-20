using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dir_date
{
    class Program
    {
        static int Main(string[] args)
        {
            if(Path.DirectorySeparatorChar == '\\')
                for(int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "":
                        case "--":
                            continue;
                    }
                    if (args[i][0] == '/')
                        args[i] = "-" + args[i].Substring(1);
                }
            foreach(var arg in args)
                switch (arg)
                {
                    case "-v":
                        Console.WriteLine("dir-date");
                        return 0;
                }
            return 0;
        }
    }
}
