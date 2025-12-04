namespace sorta.core.Configuration
{
	internal class Config
	{
		public Dictionary<string, string[]> Categories { get; set; } = new Dictionary<string, string[]>();

		public Config() { }

		public Config(Dictionary<string, string[]> categories)
		{
			Categories = categories ?? new Dictionary<string, string[]>();
		}
	}
}
