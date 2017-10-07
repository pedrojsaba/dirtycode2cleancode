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

        public static List<string> OtherTechnologies
        {
            get { return new List<string> { "Cobol", "Punch Cards", "Commodore", "VBScript" }; }
        }
    }
}
