using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sabio.Web.Domain;

namespace Sabio.Web.Models
{
    public class ProductCategoriesUpdateRequest : ProductCategoriesCreateRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Must choose the right integer")]
        public int Id { get; set; }
    }
}