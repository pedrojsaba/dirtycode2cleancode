using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    /// <summary>
    /// Represents a single speaker
    /// </summary>
    public class Speaker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Exp { get; set; }
        public bool HasBlog { get; set; }
        public string BlogUrl { get; set; }
        public WebBrowser Browser { get; set; }
        public List<string> Certifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; set; }
        public List<Session> Sessions { get; set; }

        /// <summary>
        /// Register a speaker
        /// </summary>
        /// <returns>speakerID</returns>
        public int? Register(IRepository repository)
        {
            //lets init some vars
            int? speakerId;
            var appr = false;
            //var nt = new List<string> {"MVC4", "Node.js", "CouchDB", "KendoUI", "Dapper", "Angular"};
            var ot = new List<string> { "Cobol", "Punch Cards", "Commodore", "VBScript" };

            //DEFECT #5274 DA 12/10/2012
            //We weren't filtering out the prodigy domain so I added it.
            var domains = new List<string> { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" };

            if (string.IsNullOrWhiteSpace(FirstName)) throw new ArgumentNullException(ErrorMessage.Required.Name);
            if (string.IsNullOrWhiteSpace(LastName)) throw new ArgumentNullException(ErrorMessage.Required.Lastname);
            if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentNullException(ErrorMessage.Required.Email);

            //put list of employers in array
            var emps = new List<string> { "Microsoft", "Google", "Fog Creek Software", "37Signals" };

            //DFCT #838 Jimmy 
            //We're now requiring 3 certifications so I changed the hard coded number. Boy, programming is hard.
            const int certificationsRequired = 3;
            var good = ((Exp > 10 || HasBlog || Certifications.Count() > certificationsRequired || emps.Contains(Employer)));

            if (!good)
            {
                //need to get just the domain from the email
                var emailDomain = Email.Split('@').Last();

                if (!domains.Contains(emailDomain) && (!(Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9)))
                {
                    good = true;
                }
            }

            if (!good) throw new SpeakerDoesntMeetRequirementsException(ErrorMessage.DoesNotMeetTheStandard);

            //DEFECT #5013 CO 1/12/2012
            //We weren't requiring at least one session
            if (!Sessions.Any()) throw new ArgumentException(ErrorMessage.CantRegisterWithoutSessions);

            foreach (var session in Sessions)
            {
                foreach (var tech in ot)
                {
                    if (session.Title.Contains(tech) || session.Description.Contains(tech))
                    {
                        session.Approved = false;
                        break;
                    }
                    else
                    {
                        session.Approved = true;
                        appr = true;
                    }
                }
            }


            if (!appr) throw new NoSessionsApprovedException(ErrorMessage.NoSessionsApproved);

            RegistrationFee = GetRegistrationFee(Exp);

            //Now, save the speaker and sessions to the db.
            //try
            //{
            speakerId = repository.SaveSpeaker(this);
            //}
            //catch (Exception )
            //{
            //    //in case the db call fails 
            //    throw;
            //}

            //if we got this far, the speaker is registered.
            return speakerId;
        }

        private int GetRegistrationFee(int? exp)
        {
            //if we got this far, the speaker is approved
            //let's go ahead and register him/her now.
            //First, let's calculate the registration fee. 
            //More experienced speakers pay a lower fee.
            if (exp <= 1) return 500;
            if (exp >= 2 && Exp <= 3) return 250;
            if (exp >= 4 && Exp <= 5) return 100;
            if (exp >= 6 && Exp <= 9) return 50;
            return 0;
        }

        #region Custom Exceptions
        public class SpeakerDoesntMeetRequirementsException : Exception
        {
            public SpeakerDoesntMeetRequirementsException(string message)
                : base(message)
            {
            }

            public SpeakerDoesntMeetRequirementsException(string format, params object[] args)
                : base(string.Format(format, args)) { }
        }

        public class NoSessionsApprovedException : Exception
        {
            public NoSessionsApprovedException(string message)
                : base(message)
            {
            }
        }
        #endregion
    }
}