namespace BusinessLayer
{
    public class ErrorMessage
    {
        public class Required
        {
            public static string Name
            {
                get { return "First Name is required"; }
            }

            public static string Lastname
            {
                get { return "Last name is required"; }
            }
            public static string Email
            {
                get { return "Email is required"; }
            }
        }

        public static string DoesNotMeetTheStandard
        {
            get { return "Speaker doesn't meet our abitrary and capricious standards."; }
        }

        public static string CantRegisterWithoutSessions
        {
            get { return "Can't register speaker with no sessions to present."; }
        }

        public static string NoSessionsApproved
        {
            get { return "No sessions approved."; }
        }
    }
}
