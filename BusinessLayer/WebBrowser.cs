namespace BusinessLayer
{
    public class WebBrowser
	{
		public BrowserName Name { get; set; }
		public int MajorVersion { get; set; }

		public WebBrowser(string name, int majorVersion)
		{
			Name = TranslateStringToBrowserName(name);
			MajorVersion = majorVersion;
		}

		private static BrowserName TranslateStringToBrowserName(string name)
		{
		    return name.Contains("IE") ? BrowserName.InternetExplorer : BrowserName.Unknown;
		    //TODO: Add more logic for properly sniffing for other browsers.
		}

        public enum BrowserName
		{
			Unknown,
			InternetExplorer,
			Firefox,
			Chrome,
			Opera,
			Safari,
			Dolphin,
			Konqueror,
			Linx
		}
	}
}
