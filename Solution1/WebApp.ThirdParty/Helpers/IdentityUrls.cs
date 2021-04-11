namespace WebApp.ThirdParty.Helpers
{
    public class IdentityUrls
    {
        public const string Rootlocal = "https://localhost:44311";              
        public static class Identity
        {
            public const string Login = Rootlocal + "/connect/token";
            public const string Register = Rootlocal + "/api/account/registeruser";
        }

    }
}
