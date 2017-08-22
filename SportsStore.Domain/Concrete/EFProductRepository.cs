using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductsRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }

        public Product DeleteProduct(int productID)
        {
            Product model = context.Products.Find(productID);
            if (model != null)
            {
                context.Products.Remove(model);
                context.SaveChanges();
            }
            return model;
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product model = context.Products.Find(product.ProductID);
                if (model != null)
                {
                    model.Name = product.Name;
                    model.Description = product.Description;
                    model.Price = product.Price;
                    model.Category = product.Category;
                    model.ImageData = product.ImageData;
                    model.ImageMimeType = product.ImageMimeType;
                }
            }
            context.SaveChanges();
        }
    }
}
