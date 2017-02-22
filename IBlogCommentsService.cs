using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Web.Services.Interfaces
{
   public interface IBlogCommentsService
    {
       int CreateBlog(BlogCommentsCreateRequest newBlog);

       void UpdateBlog(BlogCommentsUpdateRequest model);

        void DeleteComment(int Id);

        BlogComment GetById(int Id);

        List<BlogComment> GetAll();

    }
}
