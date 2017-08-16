﻿using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        /// <summary>
        /// 给购物车添加物品
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        /// <summary>
        /// 删除物品
        /// </summary>
        /// <param name="product"></param>
        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }
        /// <summary>
        /// 购物车结算
        /// </summary>
        /// <returns></returns>
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }
        /// <summary>
        /// 清空购物车
        /// </summary>
        public void Clear()
        {
            lineCollection.Clear();
        }
        /// <summary>
        /// 展示购物车中的物品
        /// </summary>
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
