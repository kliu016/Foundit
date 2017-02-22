using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Models.Requests
{
    public class BlogPostUpdateRequest : BlogPostCreateRequest
    {
        [Range(1, int.MaxValue, ErrorMessage="Must choose integer from 0")]
        public int Id { get; set; }
    }
}
