using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"},
                new Product{ ProductID=3,Name="P3"},
                new Product{ ProductID=4,Name="P4"},
                new Product{ ProductID=5,Name="P5"},
                new Product{ ProductID=6,Name="P6"},
            });
            ProductController controll = new ProductController(mock.Object);
            controll.PageSize = 3;

            //动作
            ProductsListViewModel result = (ProductsListViewModel)controll.List(null, 2).Model;

            //断言
            Product[] proArray = result.Products.ToArray();
            Assert.IsTrue(proArray.Length == 3);
            Assert.AreEqual(proArray[0].Name, "P4");
            Assert.AreEqual(proArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //准备-定义一个HTML辅助器，这是必须的，目的是运用扩展方法
            HtmlHelper myHelper = null;

            //准备-创建PagingInfo数据
            PagingInfo pagingIofo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsParPage = 10
            };

            //准备-用lambda表达式建立委托
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            //动作
            MvcHtmlString result = myHelper.PageLinks(pagingIofo, pageUrlDelegate);
            //断言
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
            + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
            result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"},
                new Product{ ProductID=3,Name="P3"},
                new Product{ ProductID=4,Name="P4"},
                new Product{ ProductID=5,Name="P5"},
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //动作
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            //断言
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsParPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //准备-创建模仿数据库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1",Category="Cat1"},
                new Product{ ProductID=2,Name="P2",Category="Cat2"},
                new Product{ ProductID=3,Name="P3",Category="Cat1"},
                new Product{ ProductID=4,Name="P4",Category="Cat2"},
                new Product{ ProductID=5,Name="P5",Category="Cat3"},
            });

            //准备-创建控制器，并使页面大小为3个物品
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //动作
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            //断言
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            //准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1",Category="Apples"},
                new Product{ ProductID=2,Name="P2",Category="Apples"},
                new Product{ ProductID=3,Name="P3",Category="Plums"},
                new Product{ ProductID=4,Name="P4",Category="Oranges"},
            });
            //准备-创建控制器
            NavController nav = new NavController(mock.Object);
            //动作-获取分类集合
            string[] result = ((IEnumerable<string>)nav.Menu().Model).ToArray();
            //断言
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "Apples");
            Assert.AreEqual(result[1], "Oranges");
            Assert.AreEqual(result[2], "Plums");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1",Category="Apples"},
                new Product{ ProductID=4,Name="P2",Category="Oranges"},
            });
            //准备-创建控制器
            NavController nav = new NavController(mock.Object);

            string categoryToSelect = "Apples";
            //动作
            string result = nav.Menu(categoryToSelect).ViewBag.SlectedCategory;
            //断言
            Assert.AreEqual(categoryToSelect, result);
        }
    }
}
