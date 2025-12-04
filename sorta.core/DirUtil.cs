namespace sorta.core.Utils
{
	internal class DirUtil
	{
		public static string[] GetFilePaths(string path)
		{
			return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
		}

		public static Dictionary<string,object> DirectoryInfo(string path)
		{
			string[] files = GetFilePaths(path);

			return new Dictionary<string, object> {
				{"DirectorySize", FormatSize(files.Sum(x => new FileInfo(x).Length))},
				{"TotalFiles", files.Length},
			};
		}

		private static string FormatSize(double size) {
			string[] units = { "BYTES", "KB", "MB", "GB", "TB", "PB" };
			uint index = 0;

			while (size >= 1024 && index < units.Length - 1)
			{
				size /= 1024;
				index++;
			}

			return $"{size:0.##} {units[index]}";
		}
	}
}
