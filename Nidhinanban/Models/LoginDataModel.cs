using System;
using System.ComponentModel.DataAnnotations;

namespace Nidhinanban.Models
{
    public class LoginDataModel
    {
        [Required(ErrorMessage = "Please Enter Your User ID")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter Your Password")]
        public string Password { get; set; } = string.Empty;
    }
}
