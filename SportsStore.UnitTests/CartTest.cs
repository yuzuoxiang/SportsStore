﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTest
    {
        /// <summary>
        /// 测试购物车添加物品功能
        /// </summary>
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //准备-创建一些测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            //准备-创建一个新的购物车
            Cart target = new Cart();
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] result = target.Lines.ToArray();
            //断言
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, p1);
            Assert.AreEqual(result[1].Product, p2);
        }

        /// <summary>
        /// 测试购物车物品是否会重复添加
        /// </summary>
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //准备-创建一些产品
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            //准备-创建一个购物车
            Cart target = new Cart();
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] result = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();
            //断言
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 11);
            Assert.AreEqual(result[1].Quantity, 1);
        }

        /// <summary>
        /// 测试购物车删除物品的功能
        /// </summary>
        [TestMethod]
        public void Can_Remove_Line()
        {
            //准备-创建测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            //准备-创建购物车
            Cart target = new Cart();
            //准备-往购物车添加产品
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);
            //动作
            target.RemoveLine(p2);
            //断言
            Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        /// <summary>
        /// 测试购物车物品结算的功能
        /// </summary>
        [TestMethod]
        public void Calculate_Can_Total()
        {
            //准备-创建测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100m };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50m };
            //准备-创建购物车
            Cart target = new Cart();
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();
            //断言
            Assert.AreEqual(result, 450m);
        }

        /// <summary>
        /// 测试购物车清空功能
        /// </summary>
        [TestMethod]
        public void Can_Clear_Contents()
        {
            //准备-创建测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100m };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50m };
            //准备-创建购物车
            Cart target = new Cart();
            //准备-添加一些物品
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            //动作
            target.Clear();
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1",Category="Apples"}
            }.AsQueryable());

            //准备-创建购物车
            Cart cart = new Cart();
            //准备-创建控制器
            CartController target = new CartController(mock.Object, null);
            //动作
            target.AddToCart(cart, 1, null);
            //断言
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Screen()
        {
            //准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID=1,Name="P1",Category="Apples"}
            }.AsQueryable);
            //准备-创建购物车
            Cart cart = new Cart();
            //准备-创建控制器
            CartController target = new CartController(mock.Object, null);
            //动作
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");
            //断言
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            //准备-创建购物车
            Cart cart = new Cart();
            //准备-创建控制器
            CartController target = new CartController(null, null);
            //动作
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            //断言
            Assert.AreSame(result.Cart, cart);
            Assert.AreSame(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            //准备-创建一个模仿的订单处理器
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //准备-创建一个空的购物车
            Cart cart = new Cart();
            //准备-创建一个控制器实例
            ShippingDetails shippingDetails = new ShippingDetails();
            //准备-创建一个控制器实例
            CartController target = new CartController(null, mock.Object);
            //动作
            ViewResult result = target.Checkout(cart, shippingDetails);
            //断言-检查，订单尚未传递给处理器
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(),
                It.IsAny<ShippingDetails>()), Times.Never());
            //断言-检查，该方法返回的是默认视图
            Assert.AreEqual("", result.ViewName);

            //断言-检查，给视图传递的是一个非法模型
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //准备-创建一个模仿的订单处理器
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //准备-创建一个空的购物车
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //准备-创建一个控制器实例
            CartController target = new CartController(null, mock.Object);
            //准备-把一个错误添加到模型
            target.ModelState.AddModelError("error", "error");
            //动作-试图结算
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            //断言-检查，订单未传递给处理器
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(),
                It.IsAny<ShippingDetails>()),
                Times.Never());
            //断言-检查，方法返回的是默认视图
            Assert.AreEqual("", result.ViewName);
            //断言-检查，我们正在把一个非法模型传递给视图
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_Add_Submit_Order()
        {
            //准备-创建一个模仿的订单处理器
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //准备-创建一个空的购物车
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //准备-创建一个控制器实例
            CartController target = new CartController(null, mock.Object);
            //动作-试图结算
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            //断言-检查，订单未传递给处理器
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(),
                It.IsAny<ShippingDetails>()),
                Times.Once());
            //断言-检查，方法返回的是"Completed"视图
            Assert.AreEqual("Completed", result.ViewName);
            //断言-检查，我们正在把一个有效的模型传递给视图
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
