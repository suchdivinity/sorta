using System.Text.Json;
using sorta.core.Configuration;
using sorta.core.Organiser;

namespace sorta.cli.Program
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
			Config cfg;

			if (!File.Exists(configPath))
			{
				var categories = new Dictionary<string, string[]>
				{
					{ "Compressed", new[] { ".zip", ".rar", ".7z", ".tar", ".gz" } },
					{ "Executables", new[] { ".exe", ".msi", ".bat", ".cmd" } },
					{ "Pictures", new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" } },
					{ "Videos", new[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv" } },
					{ "Documents", new[] { ".pdf", ".docx", ".txt", ".xlsx", ".pptx" } },
					{ "Music", new[] { ".mp3", ".wav", ".flac", ".aac" } }
				};

				cfg = new Config(categories);

				var opts = new JsonSerializerOptions { WriteIndented = true };
				File.WriteAllText(configPath, JsonSerializer.Serialize(cfg, opts));
			}
			else
			{
				try { cfg = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath)) ?? new Config(); }
				catch { Console.WriteLine("Failed to read config.json"); return; }
			}

			if (args.Length == 0)
			{
				Console.WriteLine("Usage: sorta <directory>");
				return;
			}

			var basePath = args[0];
			if (!Directory.Exists(basePath))
			{
				Console.WriteLine("Directory does not exist: " + basePath);
				return;
			}

			var org = new Organiser(basePath, cfg);
			org.EnsureDirectories();
			org.OrganizeAll();
		}
	}
}
