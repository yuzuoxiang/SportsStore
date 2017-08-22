using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入商品名称")]
        public string Name { get; set; }
        [Display(Name = "详细")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "请输入商品详细")]
        public string Description { get; set; }
        [Display(Name = "价格")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "请输入正确的类别价格")]
        public decimal Price { get; set; }
        [Display(Name = "类别")]
        [Required(ErrorMessage = "请输入商品类别")]
        public string Category { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
