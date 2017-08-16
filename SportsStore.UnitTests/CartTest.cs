﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;

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
    }
}
