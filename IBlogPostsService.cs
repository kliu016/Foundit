using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;

namespace Sabio.Web.Services.Interfaces
{
    public interface IBlogPostsService
    {
        int CreateBlog(BlogPostCreateRequest model);
        void UpdateBlog(BlogPostUpdateRequest model);
        //List<BlogPost> SelectAll();
        void Delete(int id);
        void Restore(int Id);
        BlogPost GetById(int id);
        BlogPost GetByDateAndTitle(int year, int month, int day, string title);
        List<MetaTag> GetMetaTags(int year, int month, int day, string title);
        PagedList<BlogPost> GetTop(int pageIndex, int itemsPerPage);
        PagedList<BlogPost> GetConsumer(int pageIndex, int numberOfRecords);
        PagedList<BlogPost> GetAdmin(int pageIndex, int numberOfRecords);
        PagedList<BlogPost> GetByTag(string blogTag, int pageIndex, int itemsPerPage);
    }
}
