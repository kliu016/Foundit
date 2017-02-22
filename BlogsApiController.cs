using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sabio.Web.Domain;
using Sabio.Web.Models;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;

namespace Sabio.Web.Controllers.Api
{
    //done. still in use. -- for public use
    [RoutePrefix("api/blogs")]
    [AllowAnonymous]
    public class BlogsApiController : BaseApiController
    {
        private IBlogPostsService _blogPostsService;
        private IMetaTagsService _metaTagsService;
        private IBlogTagsService _blogTagsService;
        private IBlogSubscribersService _blogSubscribersService;

        public BlogsApiController(IBlogPostsService blogPostsService, IMetaTagsService metaTagsService, IBlogTagsService blogTagsService, IBlogSubscribersService blogSubscribersService)
        {
            _blogPostsService = blogPostsService;
            _metaTagsService = metaTagsService;
            _blogTagsService = blogTagsService;
            _blogSubscribersService = blogSubscribersService;
        }

        #region -Blog Posts-

        [Route("posts/{id:int}"), HttpGet]
        public HttpResponseMessage Get(int id)
        {
            ItemResponse<BlogPost> responseData = new ItemResponse<BlogPost>();
            responseData.Item = _blogPostsService.GetById(id);
            return Request.CreateResponse(HttpStatusCode.OK, responseData);
        }

        [Route("posts/{year:int}/{month:int}/{day:int}/{title}"), HttpGet]
        public HttpResponseMessage Get(int year, int month, int day, string title)
        {
            ItemResponse<BlogPost> responseData = new ItemResponse<BlogPost>();
            responseData.Item = _blogPostsService.GetByDateAndTitle(year, month, day, title);
            return Request.CreateResponse(HttpStatusCode.OK, responseData);
        }

        [Route("posts/top"), HttpGet]
        public HttpResponseMessage GetTop()
        {
            ItemResponse<PagedList<BlogPost>> responseData = new ItemResponse<PagedList<BlogPost>>();
            responseData.Item = _blogPostsService.GetTop(0, 3);
            return Request.CreateResponse(HttpStatusCode.OK, responseData);
        }

        [Route("{pageIndex:int}/{itemsPerPage:int}"), HttpGet]
        public HttpResponseMessage GetConsumer(int pageIndex, int itemsPerPage)
        {
            ItemResponse<PagedList<BlogPost>> responseData = new ItemResponse<PagedList<BlogPost>>();
            responseData.Item = _blogPostsService.GetConsumer(pageIndex, itemsPerPage);
            return Request.CreateResponse(HttpStatusCode.OK, responseData);
        }

        [Route("{blogTag}/{pageIndex:int}/{itemsPerPage:int}")]
        public HttpResponseMessage GetByTag(string blogTag, int pageIndex, int itemsPerPage)
        {
            ItemResponse<PagedList<BlogPost>> responseData = new ItemResponse<PagedList<BlogPost>>();
            responseData.Item = _blogPostsService.GetByTag(blogTag, pageIndex, itemsPerPage);
            return Request.CreateResponse(HttpStatusCode.OK, responseData);
        }

        #endregion

        #region - Meta Tags -
        //api/v2/admin/blogs
        [Route("{year:int}/{month:int}/{day:int}/{title}/metaTags"), HttpGet] //--------------------Get MetaTags--------------------
        public HttpResponseMessage GetMetaTags(int year, int month, int day, string title)
        {
            ItemsResponse<MetaTag> responseData = new ItemsResponse<MetaTag>();
            responseData.Items = _blogPostsService.GetMetaTags(year, month, day, title);
            return Request.CreateResponse(HttpStatusCode.OK, responseData);
        }
        #endregion

        #region - Tags -

        [Route("tags")]
        public HttpResponseMessage GetAllActive()
        {
            List<BlogTags> list = _blogTagsService.GetAllActiveBlogTags();

            ItemsResponse<BlogTags> responseData = new ItemsResponse<BlogTags>();
            responseData.Items = list;

            return Request.CreateResponse(HttpStatusCode.OK, responseData);
        }

        [Route("tags/{id:int}")]
        public HttpResponseMessage GetById(int id)
        {
            ItemResponse<BlogTags> responseData = new ItemResponse<BlogTags>();
            responseData.Item = _blogTagsService.GetBlogTagById(id);

            return Request.CreateResponse(HttpStatusCode.OK, responseData);
        }
        #endregion

        #region -Subscribers-

        /*-------------Post-----------------*/


        [Route("subscribe/create"), HttpPost]
        public HttpResponseMessage Create(BlogSubscriberCreateRequest model)
        {
            if (!IsValidRequest(model))
            {
                return GetErrorResponse(model);
            }


            try
            {
                int id = _blogSubscribersService.CreateBlogSubscriber(model);
                ItemResponse<int> responseData = new ItemResponse<int>();
                responseData.Item = id;

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)

            {

                ErrorResponse errMsg = new ErrorResponse("This email already exists in the database." + ex.Message);

                return Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);
            }


        }


        #endregion

    }

}



