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
        public int? Experience { get; set; }
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
            
            var ot = new List<string> { "Cobol", "Punch Cards", "Commodore", "VBScript" };
            
            if (string.IsNullOrWhiteSpace(FirstName)) throw new ArgumentNullException(ErrorMessage.Required.Name);
            if (string.IsNullOrWhiteSpace(LastName)) throw new ArgumentNullException(ErrorMessage.Required.Lastname);
            if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentNullException(ErrorMessage.Required.Email);
          
            //DFCT #838 Jimmy 
            //We're now requiring 3 certifications so I changed the hard coded number. Boy, programming is hard.
            const int certificationsRequired = 3;
            const int miniumExperience = 10;
            var good = ((Experience > miniumExperience || HasBlog || Certifications.Count() > certificationsRequired || DataList.Employers.Contains(Employer)));

            if (!good)
            {
                //need to get just the domain from the email
                var emailDomain = Email.Split('@').Last();

                good = !DataList.Domains.Contains(emailDomain) &&
                        (!(Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9));
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
                    session.Approved = true;
                    appr = true;
                }
            }


            if (!appr) throw new NoSessionsApprovedException(ErrorMessage.NoSessionsApproved);

            RegistrationFee = GetRegistrationFee(Experience);

            //Now, save the speaker and sessions to the db.           
            speakerId = repository.SaveSpeaker(this);
           
            //if we got this far, the speaker is registered.
            return speakerId;
        }

        private static int GetRegistrationFee(int? experience)
        {
            //if we got this far, the speaker is approved
            //let's go ahead and register him/her now.
            //First, let's calculate the registration fee. 
            //More experienced speakers pay a lower fee.
            if (experience <= 1) return 500;
            if (experience >= 2 && experience <= 3) return 250;
            if (experience >= 4 && experience <= 5) return 100;
            if (experience >= 6 && experience <= 9) return 50;
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