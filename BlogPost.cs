using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sabio.Web.Domain;

namespace Sabio.Web.Domain
{
    public class BlogPost
    { 
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string BlogContent { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public bool HasBeenEmailed { get; set; }

        public List<string> Tags { get; set; }

        public List<MetaTag> MetaTags { get; set; }
    }
}
