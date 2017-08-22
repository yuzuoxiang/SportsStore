﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using System.Linq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class ImageTest
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            //准备-创建一个带图片数据的Product
            Product prod = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            //准备-创建模仿数据
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"}
            }.AsQueryable());
            //准备创建控制器
            ProductController target = new ProductController(mock.Object);
            //动作-调用GetImage动作方法
            ActionResult result = target.GetImage(2);
            //断言
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            //准备-创建模仿数据
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"}
            }.AsQueryable());

            //准备创建控制器
            ProductController target = new ProductController(mock.Object);
            //动作-调用GetImage动作方法
            ActionResult result = target.GetImage(100);
            //断言
            Assert.IsNull(result);
        }
    }
}
