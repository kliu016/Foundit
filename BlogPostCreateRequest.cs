using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests
{
    public class BlogPostCreateRequest 
    {
        [Required]
        [MaxLength(1000)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string BlogContent { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public List<string> BlogTags { get; set; }

    }
}