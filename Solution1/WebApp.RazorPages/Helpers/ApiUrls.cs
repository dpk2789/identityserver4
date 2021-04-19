namespace WebApp.RazorPages.Helpers
{
    public static class ApiUrls
    {
       // public const string Rootlocal = "https://localhost:44363/api";
        public const string Rootlocal = "https://localhost:44363/api";
        public const string Version = "v1";
        public const string Base = Rootlocal + "/"+ Version ;

        public static class Companies
        {
            public const  string GetAll = Base +"/companies";
            public const string Get = Base + "/companies/{postId}";
            public const string Create = Base + "/companies";
            public const string Update = Base + "/companies/{postId}";
            public const string Delete = Base + "/companies/{postId}";
        }       

    }
}
