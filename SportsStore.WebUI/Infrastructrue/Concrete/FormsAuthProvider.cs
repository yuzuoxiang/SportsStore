using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using SportsStore.WebUI.Infrastructrue.Abstract;

namespace SportsStore.WebUI.Infrastructrue.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string passwrod)
        {
            bool result = FormsAuthentication.Authenticate(username, passwrod);
            //bool result = Membership.ValidateUser(username, passwrod);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }
    }
}