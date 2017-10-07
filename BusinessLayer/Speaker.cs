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
            var appr = false;

            ValidParameters(FirstName, LastName, Email);
                   
            const int certificationsRequired = 3;
            const int miniumExperience = 10;
            var good = ((Experience > miniumExperience || HasBlog || Certifications.Count() > certificationsRequired || DataList.Employers.Contains(Employer)));

            if (!good)
            {         
                var emailDomain = Email.Split('@').Last();
                good = !DataList.Domains.Contains(emailDomain) &&
                        (!(Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9));
            }

            if (!good) throw new SpeakerDoesntMeetRequirementsException(ErrorMessage.DoesNotMeetTheStandard);          
            if (!Sessions.Any()) throw new ArgumentException(ErrorMessage.CantRegisterWithoutSessions);

            foreach (var session in Sessions)
            {
                foreach (var tech in DataList.OtherTech)
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

            return repository.SaveSpeaker(this);
        }

        private static int GetRegistrationFee(int? experience)
        {            
            if (experience <= 1) return 500;
            if (experience >= 2 && experience <= 3) return 250;
            if (experience >= 4 && experience <= 5) return 100;
            if (experience >= 6 && experience <= 9) return 50;
            return 0;
        }

        private static void ValidParameters(string name,string lastname, string email)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(ErrorMessage.Required.Name);
            if (string.IsNullOrWhiteSpace(lastname)) throw new ArgumentNullException(ErrorMessage.Required.Lastname);
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(ErrorMessage.Required.Email);
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