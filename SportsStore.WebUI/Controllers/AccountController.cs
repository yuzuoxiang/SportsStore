using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.WebUI.Infrastructrue.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;
        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View();

            if (authProvider.Authenticate(model.UserName, model.Passwrod))
            {
                return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
            }
            else
            {
                ModelState.AddModelError("", "用户名或密码不正确");
                return View();
            }
        }
    }
}