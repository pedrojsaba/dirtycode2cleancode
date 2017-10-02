using System.Collections.Generic;

namespace BusinessLayer
{
    public class DataList
    {
        public static List<string> Employers
        {
            get { return new List<string> { "Microsoft", "Google", "Fog Creek Software", "37Signals" }; }
        }

        public static List<string> Domains
        {
            get { return new List<string> { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" }; }
        }
    }
}
