using System.ComponentModel.DataAnnotations;

namespace WebApp.MVC.Models
{
    public class RegisterViewModel
    {
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }
    }
}
