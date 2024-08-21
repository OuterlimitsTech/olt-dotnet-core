namespace OLT.Constants
{
    public static class OltSerilogConstants
    {
        public static class FormatString
        {
            public const string ISO8601 = "yyyy-MM-ddTHH:mm:ss.fffZ";
        }


        public static class Properties
        {
            public const string Username = "Username";
            public const string DbUsername = "DbUsername";
            public const string EventType = "OltEventType";
            public const string Environment = "Environment";
            public const string DebuggerAttached = "DebuggerAttached";
            public const string Application = "Application";

            public static class AspNetCore
            {
                public const string AppRequestUid = "AppRequestUid";
                public const string RequestHeaders = "RequestHeaders";
                public const string ResponseHeaders = "ResponseHeaders";
                public const string RequestBody = "RequestBody";
                public const string ResponseBody = "ResponseBody";
                public const string RequestUri = "RequestUri";
            }

            public static class NgxMessage
            {
                public const string MessageAsJson = "ngx-message-json";
            }
        }



        public static class Templates
        {
            public static class AspNetCore
            {
                public const string ServerError = "{AppRequestUid}:{Message}";
                public const string Payload = "{AppRequestUid}:APP PAYLOAD LOG {RequestMethod} {RequestPath} {statusCode}";
            }

            public const string DefaultOutput =
                "[{Timestamp:HH:mm:ss} {Level:u3}] {OltEventType:x8} {Message:lj}{NewLine}{Exception}";



            public static class Email
            {
                public static string DefaultEmail => Environment.NewLine +
                                                     Environment.NewLine +
                                                     DefaultOutput +
                                                     Environment.NewLine +
                                                     Environment.NewLine;

                public const string DefaultSubject =
                    "APPLICATION {Level} on {Application} {Environment} Environment occurred at {Timestamp}";
            }

            public static class NgxMessage
            {
                public const string Template = "ngx-message: {ngx-message}";
            }
        }



    }

}
