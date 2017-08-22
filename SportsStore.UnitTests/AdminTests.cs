using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            //准备-创建模仿数据
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"},
                new Product{ ProductID=3,Name="P3"}
            });

            //准备-创建控制器
            AdminController admin = new AdminController(mock.Object);
            //动作
            Product[] result = ((IEnumerable<Product>)admin.Index().
                ViewData.Model).ToArray();
            //断言
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            //准备-创建模仿数据
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"},
                new Product{ ProductID=3,Name="P3"}
            });
            //准备-创建控制器
            AdminController target = new AdminController(mock.Object);
            //动作
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;
            //断言
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            //准备-创建模仿数据
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"},
                new Product{ ProductID=3,Name="P3"}
            });
            //准备-创建控制器
            AdminController target = new AdminController(mock.Object);
            //动作
            Product result = (Product)target.Edit(4).ViewData.Model;
            //断言
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //准备-创建模仿数据
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            //准备-创建控制器
            AdminController target = new AdminController(mock.Object);
            //准备-创建一个产品
            Product product = new Product { Name = "Test" };
            //动作-试着保存产品
            ActionResult result = target.Edit(product);
            //断言-检查，调用数据库
            mock.Verify(m => m.SaveProduct(product));
            //断言-检查方法的结果类型
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            //准备-创建模仿数据
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            //准备-创建控制器
            AdminController target = new AdminController(mock.Object);
            //准备-创建一个产品
            Product product = new Product { Name = "Test" };
            //准备-添加一个错误到模型状态
            target.ModelState.AddModelError("error", "error");
            //动作-试着保存产品
            ActionResult result = target.Edit(product);
            //断言-检查，调用数据库
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            //断言-检查方法的结果类型
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            //准备-创建一个产品
            Product model = new Product { ProductID = 2, Name = "Test" };
            //准备-模仿数据
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID=1, Name = "P1" },
                new Product { ProductID=3, Name = "P3" }
            });

            //准备-创建控制器
            AdminController target = new AdminController(mock.Object);
            //动作-删除产品
            target.Delete(model.ProductID);
            //断言-确保删除是否成功
            mock.Verify(m => m.DeleteProduct(model.ProductID));
        }
    }
}
