namespace WebApi1.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/"+ Version ;

        public static class Companies
        {
            public const  string GetAll = Base +"/companies";
            public const string Get = Base + "/companies/{postId}";
            public const string Create = Base + "/companies";
            public const string Update = Base + "/companies/{postId}";
            public const string Delete = Base + "/companies/{postId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
           
        }

    }
}
