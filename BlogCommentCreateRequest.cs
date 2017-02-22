using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests
{
    public class BlogCommentsCreateRequest
    {
        
        public int? ParentCommentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(4000)]
        public string Content { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int BlogPostId { get; set; }

    }
}