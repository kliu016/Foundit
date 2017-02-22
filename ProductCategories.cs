using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Domain
{
    public class ProductCategory
    {
        public int ParentCategoryId { get; set; }

        public string DisplayName { get; set; }

        public string ProductCategoryKey { get; set; }

        public string UserId { get; set; }

        public string ImageUrl { get; set; }

        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }


    }
}