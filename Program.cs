internal class Program {
	static readonly List<Item> items = new();
	static bool all;

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
					case "a":
					case "all":
						all = true;
						break;
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
				continue;
			}
			paths.Add(s);
		}
		if (paths.Count == 0)
			paths.Add(".");

		foreach (var path in paths)
			Do(path);

		items.Sort();

		var now = DateTime.Now;
		foreach (var item in items) {
			var age = now - item.WriteTime;
			if (age < TimeSpan.FromHours(1))
				Console.ForegroundColor = ConsoleColor.Red;
			else if (age < TimeSpan.FromDays(1))
				Console.ForegroundColor = ConsoleColor.Yellow;
			else if (age < TimeSpan.FromDays(7))
				Console.ForegroundColor = ConsoleColor.Green;
			else if (age < TimeSpan.FromDays(365))
				Console.ForegroundColor = ConsoleColor.Cyan;
			else
				Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write(item.WriteTime);

			Console.ResetColor();
			Console.WriteLine($"\t{item.Size}\t{item.Name}");
		}
	}

	static void Help() {
		Console.WriteLine("Usage: dir-date [options] [path...]");
		Console.WriteLine();
		Console.WriteLine("-h  Show help");
		Console.WriteLine("-V  Show version");
		Console.WriteLine("-a  Don't ignore names beginning with dot");
	}

	static void Do(string path) {
		try {
			if (Directory.Exists(path))
				foreach (var entry in Directory.GetFileSystemEntries(path)) {
					if (all || !Path.GetFileName(entry).StartsWith('.'))
						Do(entry);
				}
			else
				items.Add(new Item(path));
		} catch (UnauthorizedAccessException e) {
			Console.WriteLine(e.Message);
		}
	}
}

class Item: IComparable<Item> {
	public readonly string Name;
	public readonly long Size;
	public readonly DateTime WriteTime;

	public Item(string path) {
		var info = new FileInfo(path);
		Name = info.FullName;
		Size = info.Length;
		WriteTime = info.LastWriteTime;
	}

	public int CompareTo(Item? other) {
		// Default sort algorithm is unstable:
		// https://stackoverflow.com/questions/148074/is-the-sorting-algorithm-used-by-nets-array-sort-method-a-stable-algorithm
		// Could use a stable sort algorithm
		// to keep same-age files in the order given by the operating system
		// but it is arguably clearer to break ties by name
		if (other == null)
			return 1;
		var c = WriteTime.CompareTo(other.WriteTime);
		if (c != 0)
			return c;
		return Name.CompareTo(other.Name);
	}
}
