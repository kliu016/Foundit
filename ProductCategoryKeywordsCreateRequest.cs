using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests
{
    public class ProductCategoryKeywordsCreateRequest
    {

        [Required]
        [StringLength(50)]
        public string Keyword { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}