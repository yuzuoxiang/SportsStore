using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Infrastructrue.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //准备-创建模仿认证提供器
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "123")).Returns(true);
            //准备-创建视图模型
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Passwrod = "123"
            };
            //准备-创建控制器
            AccountController target = new AccountController(mock.Object);
            //动作-使用合法凭证进行认证
            ActionResult result = target.Login(model, "/MyUrl");
            //断言
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            //准备-创建模仿认证提供器
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin2", "123")).Returns(true);
            //准备-创建视图模型
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admi2n",
                Passwrod = "123"
            };
            //准备-创建控制器
            AccountController target = new AccountController(mock.Object);
            //动作-使用合法凭证进行认证
            ActionResult result = target.Login(model, "/MyUrl");
            //断言
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
