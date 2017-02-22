using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Sabio.Web.Domain
{
    public class ProductCategoryKeyword
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Keyword { get; set; }
        public int CategoryId { get; set; }
    }
}