using sorta.core.Configuration;
using sorta.core.Utils;

namespace sorta.core.Organiser
{
	internal class Organiser
	{
		private readonly string _basePath;
		private readonly Config _config;

		public Organiser(string basePath, Config config)
		{
			_basePath = basePath;
			_config = config;
		}

		public void EnsureDirectories()
		{
			foreach (var category in _config.Categories.Keys)
			{
				Directory.CreateDirectory(Path.Combine(_basePath, "S" + category));
			}
			Directory.CreateDirectory(Path.Combine(_basePath, "SOthers"));
		}

		public void OrganizeAll()
		{
			var files = DirUtil.GetFilePaths(_basePath)
				.Where(f => !IsInTargetDir(f))
				.Where(f => !string.Equals(Path.GetFileName(f), "config.json", StringComparison.OrdinalIgnoreCase))
				.ToArray();

			foreach (var file in files)
			{
				var ext = Path.GetExtension(file).ToLowerInvariant();
				var found = _config.Categories.FirstOrDefault(kv => kv.Value.Any(e => string.Equals(e, ext, StringComparison.OrdinalIgnoreCase)));
				var categoryName = string.IsNullOrEmpty(found.Key) ? "Others" : found.Key;
				var targetDir = Path.Combine(_basePath, "S" + categoryName);
				Directory.CreateDirectory(targetDir);

				var dest = Path.Combine(targetDir, Path.GetFileName(file));
				if (string.Equals(Path.GetFullPath(dest), Path.GetFullPath(file), StringComparison.OrdinalIgnoreCase))
					continue;

				dest = GetUniqueDestination(dest);

				try { File.Move(file, dest); } catch { }
			}
		}

		private bool IsInTargetDir(string file)
		{
			var rel = Path.GetRelativePath(_basePath, file);
			var first = rel.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
			return first != null && first.StartsWith("S", StringComparison.OrdinalIgnoreCase);
		}

		private static string GetUniqueDestination(string path)
		{
			if (!File.Exists(path))
				return path;

			var dir = Path.GetDirectoryName(path) ?? string.Empty;
			var name = Path.GetFileNameWithoutExtension(path);
			var ext = Path.GetExtension(path);
			var i = 1;
			string candidate;
			do { candidate = Path.Combine(dir, $"{name}({i}){ext}"); i++; } while (File.Exists(candidate));
			return candidate;
		}
	}
}
