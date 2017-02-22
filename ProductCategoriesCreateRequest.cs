using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models
{
    public class ProductCategoriesCreateRequest
    {
        [Required]
        [MaxLength(50)]
        public string DisplayName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductCategoryKey { get; set; }

        public int? ParentCategoryId { get; set; }

        public string ImageUrl { get; set; }
    }
}