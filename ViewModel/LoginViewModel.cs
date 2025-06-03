using System.ComponentModel.DataAnnotations;
namespace MySaleApp.Admin.UI.ViewModel
{
    public class LoginViewModel
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
