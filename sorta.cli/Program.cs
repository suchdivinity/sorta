using sorta.core.Configuration;
using sorta.core.Organiser;
using System.Text.Json;

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
					{
						"Compressed",
						new []
                        {
                            ".zip", ".rar", ".7z", ".tar", ".gz",
							".bz2", ".cab", ".dmg", ".iso",
							".lzma", ".xz", ".zst"
						}
					},
					{
						"Executables",
						new []
						{
							".exe", ".msi", ".bat", ".cmd",
							".apk", ".appimage", ".jar",
							".ps1", ".py", ".sh", ".vbs"
						}
					},
					{
						"Pictures",
						new []
						{
							".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff",
							".arw", ".cr2", ".heic", ".heif",
							".ico", ".nef", ".raw", ".svg", ".webp"
						}
					},
					{
						"Videos",
						new []
						{
							".mp4", ".avi", ".mkv", ".mov", ".wmv",
							".3gp", ".flv", ".m4v",
							".mpeg", ".mpg", ".webm"
						}
					},
					{
						"Documents",
						new []
						{
							".pdf", ".docx", ".txt", ".xlsx", ".pptx",
							".csv", ".html", ".htm",
							".json", ".md", ".odt",
							".odp", ".ods", ".rtf",
							".xml", ".yaml", ".yml"
						}
					},
					{
						"Music",
						new []
						{
							".mp3", ".wav", ".flac", ".aac",
							".aiff", ".alac", ".m4a",
							".ogg", ".wma"
                        }
					}
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
