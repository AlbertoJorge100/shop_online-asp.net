using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace shop_online.Models.ViewModels
{
    public class categoriesModel
    {
        public int id;

        [Required]
        [StringLength(100)]
        [Display(Name = "name")]
        public string name;

        [Required]
        [StringLength(500)]
        [Display(Name = "description")]
        public string description;
    }
}