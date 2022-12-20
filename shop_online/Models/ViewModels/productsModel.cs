using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace shop_online.Models.ViewModels
{
    public class productsModel
    {
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "name")]
        public string name { get; set; }

        [Required]        
        [Display(Name = "price")]
        public double price { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "description")]
        public string description { get; set; }

        [Required]
        public int id_categoria { get; set; }

        public categories category { get; set; }
        public order_details order_detail { get; set; }
    }
}