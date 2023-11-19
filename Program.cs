internal class Program {
	static void Main(string[] args) {
		var options = true;
		var paths = new List<string>();
		foreach (var arg in args) {
			var s = arg;
			if (options) {
				if (s == "--") {
					options = false;
					continue;
				}
				if (s.StartsWith('-')) {
					while (s.StartsWith('-'))
						s = s[1..];
					switch (s) {
					case "?":
					case "h":
					case "help":
						Help();
						return;
					case "V":
					case "v":
					case "version":
						Console.WriteLine("dir-date " + typeof(Program).Assembly.GetName()?.Version?.ToString(2));
						return;
					default:
						Console.WriteLine(arg + ": unknown option");
						Environment.Exit(1);
						break;
					}
				}
			}
			paths.Add(s);
		}
		if (paths.Count == 0)
			paths.Add(".");

		foreach (var path in paths)
			Do(path);
	}

	static void Help() {
		Console.WriteLine("Usage: dir-date [options] [path...]");
		Console.WriteLine();
		Console.WriteLine("-h  Show help");
		Console.WriteLine("-V  Show version");
	}

	static void Do(string path) {
		if (Directory.Exists(path))
			foreach (var entry in Directory.GetFileSystemEntries(path))
				Do(entry);
		else
			Console.WriteLine(path);
	}
}