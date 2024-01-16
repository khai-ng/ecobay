namespace Web.ApiGateway.Configurations
{
    public class UrlsConfiguration
    {
        public class IdentityUrls
        {
            public static string GetUser() => "identity/getuser";
        }

        public class EmployeeUrls
        {
            public static string Get() => "employee/get";
        }

        public string Identity { get;set; }
        public string Employee { get;set; }
    }
}
