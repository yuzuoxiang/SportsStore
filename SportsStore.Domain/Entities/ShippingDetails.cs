using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage ="请输入姓名")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage ="请输入收货地址")]
        [Display(Name ="地址 1")]
        public string Line1 { get; set; }
        [Display(Name = "地址 2")]
        public string Line2 { get; set; }
        [Display(Name = "地址 3")]
        public string Line3 { get; set; }

        [Required(ErrorMessage ="请输入省份")]
        [Display(Name = "省份")]
        public string City { get; set; }

        [Required(ErrorMessage ="请输入城市")]
        [Display(Name = "城市")]
        public string State { get; set; }
        [Display(Name = "邮编")]
        public string Zip { get; set; }

        [Required(ErrorMessage ="请输入县区")]
        [Display(Name = "县区")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
    }
}
