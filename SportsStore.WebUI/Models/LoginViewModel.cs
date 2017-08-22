using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace SportsStore.WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="用户名不能为空")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string Passwrod { get; set; }
    }
}