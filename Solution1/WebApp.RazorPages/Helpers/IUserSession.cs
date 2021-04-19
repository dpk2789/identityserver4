namespace WebApp.RazorPages.Helpers
{
    public interface IUserSession
    {
        string Username { get; }
        string BearerToken { get; }
    }
}
