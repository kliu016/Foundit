using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Domain.Tests;
using Sabio.Web.Models.Requests;
using Sabio.Web.Services.Interfaces;
using Ganss.XSS;
using Sabio.Web.Models;

namespace Sabio.Web.Services
{
    public class BlogPostsService : BaseService, IBlogPostsService, IMetaTagsService
    {
        private IUserService _userService;

        public BlogPostsService(IUserService userService)
        {
            _userService = userService;
        }

        private static BlogPost MapBlogPost(IDataReader reader)
        {
            BlogPost thisBlogPost = new BlogPost();
            int startingIndex = 0;

            thisBlogPost.ImageUrl = reader.GetSafeString(startingIndex++);
            thisBlogPost.Title = reader.GetSafeString(startingIndex++);
            thisBlogPost.BlogContent = reader.GetSafeString(startingIndex++);
            thisBlogPost.Name = reader.GetSafeString(startingIndex++);
            thisBlogPost.Date = reader.GetSafeDateTime(startingIndex++);
            thisBlogPost.UserId = reader.GetSafeString(startingIndex++);
            thisBlogPost.Id = reader.GetSafeInt32(startingIndex++);
            thisBlogPost.DateCreated = reader.GetSafeDateTime(startingIndex++);
            thisBlogPost.DateModified = reader.GetSafeDateTime(startingIndex++);
            thisBlogPost.IsDeleted = reader.GetSafeBool(startingIndex++);
            thisBlogPost.HasBeenEmailed = reader.GetSafeBool(startingIndex++);

            return thisBlogPost;
        }

        private static MetaTag MapMetaTags(IDataReader reader)
        {
            MetaTag thisMetaTags = new MetaTag();
            int startingIndex = 0;

            thisMetaTags.Id = reader.GetSafeInt32(startingIndex++);
            thisMetaTags.Name = reader.GetSafeString(startingIndex++);
            thisMetaTags.Description = reader.GetSafeString(startingIndex++);
            thisMetaTags.Example = reader.GetSafeString(startingIndex++);
            thisMetaTags.Template = reader.GetSafeString(startingIndex++);
            thisMetaTags.DateAdded = reader.GetSafeDateTime(startingIndex++);
            thisMetaTags.DateModified = reader.GetSafeDateTime(startingIndex++);
            thisMetaTags.LanguageCode = reader.GetSafeString(startingIndex++);
            thisMetaTags.UserId = reader.GetSafeString(startingIndex++);

            return thisMetaTags;
        }

        private static string GetDate(int year, int month, int day)
        {
            string yearString = Convert.ToString(year);
            string yearMonth = Convert.ToString(month);
            string yearDay = Convert.ToString(day);

            string datePublished = yearString + "/" + yearMonth + "/" + yearDay;
            return datePublished;
        }

        private static void MpagePagedList(int pageIndex, int itemsPerPage, IDataReader reader, ref PagedList<BlogPost> pagedList, ref List<BlogPost> blogsList, ref int totalCount)
        {
            BlogPost pageBlogs = MapBlogPost(reader);
            if (blogsList == null)
            {
                blogsList = new List<BlogPost>();
            }

            blogsList.Add(pageBlogs);

            if (totalCount == 0)
            {
                totalCount = reader.GetSafeInt32(11);
            }

            if (pagedList == null)
            {
                pagedList = new PagedList<BlogPost>(blogsList, pageIndex, itemsPerPage, totalCount);
            }
        }




        private static MetaTagKey MapMetaTagsKey(IDataReader reader)
        {
            MetaTagKey thisMetaTagsKey = new MetaTagKey();
            int startingIndex = 0;

            thisMetaTagsKey.Id = reader.GetSafeInt32(startingIndex++);
            thisMetaTagsKey.BlogId = reader.GetSafeInt32(startingIndex++);
            thisMetaTagsKey.Value = reader.GetSafeString(startingIndex++);


            return thisMetaTagsKey;
        }

        private static string SanitizeHtml(BlogPostCreateRequest blogModel)
        {
            var sanitizer = new HtmlSanitizer();
            var blogContent = blogModel.BlogContent;
            var sanitizedBlogContent = sanitizer.Sanitize(blogContent);

            return sanitizedBlogContent;
        }

           

        public int CreateTags(TagsCreateRequest newTags)
        {
            int result = -1;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.MetaTagsKey_Insert",
            inputParamMapper: delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@MetaTagId", newTags.MetaTagId);
                parameters.AddWithValue("@BlogId", newTags.BlogId);
                parameters.AddWithValue("@Value", newTags.Value);

                SqlParameter idParam = new SqlParameter("@Id", DbType.Int32);
                idParam.Direction = ParameterDirection.Output;
                parameters.Add(idParam);
            },
            returnParameters: delegate (SqlParameterCollection parameters)
            {
                result = int.Parse(parameters["@Id"].Value.ToString());
            }
        );
             return result;
        }

        public void UpdateTags(TagsUpdateRequest existingTag)
        {
            DataProvider.ExecuteNonQuery(
            GetConnection, "[dbo].MetaTagsKey_Update",
            inputParamMapper: delegate (SqlParameterCollection parameters)
            {

                parameters.AddWithValue("@MetaTagId", existingTag.MetaTagId);
                parameters.AddWithValue("@BlogId", existingTag.BlogId);
                parameters.AddWithValue("@Value", existingTag.Value);
                parameters.AddWithValue("@Id", existingTag.Id);

            });
        }

        public List<MetaTag> Get()
        {
            List<MetaTag> list = null;
            DataProvider.ExecuteCmd(
            GetConnection, "[dbo].MetaTags_SelectAll",
            inputParamMapper: delegate (SqlParameterCollection parameters)
            {

            }, map: delegate (IDataReader reader, short set)
            {
                MetaTag MetaTag = MapMetaTags(reader);
                if (list == null)
                {
                    list = new List<MetaTag>();
                }

                list.Add(MetaTag);
            });
            return list;
        }

        public void DeleteTags(int BlogId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.MetaTagsKey_Delete",
            inputParamMapper: delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@BlogId", BlogId);
            });
        }


        public List<MetaTagKey> SelectBlogId(int blogid)
        {
            List<MetaTagKey> mtk = null;

            DataProvider.ExecuteCmd(GetConnection, "MetaTagsKey_SelectMetaTagId",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@BlogId", blogid);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    if (mtk == null)
                    {
                        mtk = new List<MetaTagKey>();
                    }

                    MetaTagKey metaT = new MetaTagKey();
                    int startingIndex = 0;
                    metaT.Id = reader.GetSafeInt32(startingIndex++);
                    metaT.Name = reader.GetSafeString(startingIndex++);
                    metaT.Description = reader.GetSafeString(startingIndex++);
                    metaT.Example = reader.GetSafeString(startingIndex++);
                    metaT.Template = reader.GetSafeString(startingIndex++);
                    metaT.DateAdded = reader.GetSafeDateTime(startingIndex++);
                    metaT.DateModified = reader.GetSafeDateTime(startingIndex++);
                    metaT.LanguageCode = reader.GetSafeString(startingIndex++);
                    metaT.UserId = reader.GetSafeString(startingIndex++);
                    metaT.BlogId = reader.GetSafeInt32(startingIndex++);
                    metaT.Value = reader.GetSafeString(startingIndex++);

                    mtk.Add(metaT);
                });
            return mtk;
        }



        public int CreateBlog(BlogPostCreateRequest newBlog)
        {
            int result = -1;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.BlogPosts_Insert",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@ImageUrl", newBlog.ImageUrl);
                    parameters.AddWithValue("@Title", newBlog.Title);
                    parameters.AddWithValue("@BlogContent", SanitizeHtml(newBlog));
                    parameters.AddWithValue("@Name", newBlog.Name);
                    parameters.AddWithValue("@Date", newBlog.PublishDate);
                    parameters.AddWithValue("@UserId", _userService.GetCurrentUserId());

                    SqlParameter idParam = new SqlParameter("@Id", DbType.Int32);
                    idParam.Direction = ParameterDirection.Output;
                    parameters.Add(idParam);

                    SqlParameter t = new SqlParameter("@BlogTags", System.Data.SqlDbType.Structured);
                    if (newBlog.BlogTags != null && newBlog.BlogTags.Any())
                    {
                        t.Value = new Sabio.Data.NVarcharTable(newBlog.BlogTags);
                    }
                    parameters.Add(t);
                },
                returnParameters: delegate (SqlParameterCollection parameters)
                {
                    result = int.Parse(parameters["@Id"].Value.ToString());
                }
            );
            return result;
        }

        public void UpdateBlog(BlogPostUpdateRequest existingBlog)
        {
            DataProvider.ExecuteNonQuery(
            GetConnection, "[dbo].BlogPosts_UpdateTag",
            inputParamMapper: delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@ImageUrl", existingBlog.ImageUrl);
                parameters.AddWithValue("@Title", existingBlog.Title);
                parameters.AddWithValue("@BlogContent", SanitizeHtml(existingBlog));
                parameters.AddWithValue("@Name", existingBlog.Name);
                parameters.AddWithValue("@Date", existingBlog.PublishDate);
                parameters.AddWithValue("@Id", existingBlog.Id);
                parameters.AddWithValue("@UserId", _userService.GetCurrentUserId());

                SqlParameter t = new SqlParameter("@BlogTags", System.Data.SqlDbType.Structured);
                if (existingBlog.BlogTags != null && existingBlog.BlogTags.Any())
                {
                    t.Value = new Sabio.Data.NVarcharTable(existingBlog.BlogTags);
                }
                parameters.Add(t);
            });
        }

        //public List<BlogPost> SelectAll()
        //{
        //    List<BlogPost> list = null;
        //    DataProvider.ExecuteCmd(
        //    GetConnection, "[dbo].BlogPosts_SelectAll",
        //    inputParamMapper: delegate (SqlParameterCollection parameters)
        //    {

        //    }, map: delegate (IDataReader reader, short set)
        //    {
        //        BlogPost BlogPosts = MapBlogPost(reader);
        //        if (list == null)
        //        {
        //            list = new List<BlogPost>();
        //        }

        //        list.Add(BlogPosts);
        //    });
        //    return list;
        //}

        public void Delete(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.BlogPosts_UpdatedisDeleted",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@Id", Id);
                });
        }

        public void Restore(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.BlogPosts_UpdateisDeletedRestore",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@Id", Id);
                });
        }

        public BlogPost GetById(int id) //--------------------WORK ON THIS--------------------
        {
            BlogPost blogPost = null;
            List<string> tagsList = null;
            List<MetaTag> metaTagsList = null;


            DataProvider.ExecuteCmd(
                GetConnection,
                "[dbo].[BlogPosts_SelectById_V2]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@Id", id);
                },
                map: delegate (IDataReader reader, short set)
                {
                    if (set == 0)
                    {
                        blogPost = MapBlogPost(reader);
                    }
                    else if (set == 1)
                    {
                        string tag = reader.GetSafeString(0);

                        if (tagsList == null)
                        {
                            tagsList = new List<string>();
                        }
                        tagsList.Add(tag);
                    }
                    else if (set == 2)
                    {
                        MetaTag metaTag = MapMetaTags(reader);

                        if (metaTagsList == null)
                        {
                            metaTagsList = new List<MetaTag>();
                        }
                        metaTagsList.Add(metaTag);
                    }
                }
           );
            if (blogPost != null)
            {
                blogPost.Tags = tagsList;
                blogPost.MetaTags = metaTagsList;
            }

            return blogPost;
        }

        public BlogPost GetByDateAndTitle(int year, int month, int day, string title) //--------------------WORK ON THIS TOO--------------------
        {
            string datePublished = GetDate(year, month, day);

            BlogPost blogPost = null;
            List<string> tagsList = null;
            List<MetaTag> metaTagsList = null;

            DataProvider.ExecuteCmd(
                GetConnection,
                "[dbo].[BlogPosts_SelectByDateAndTitle_V2]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@DatePublished", datePublished);
                    parameters.AddWithValue("@Title", title);
                },
                map: delegate (IDataReader reader, short set)
                {
                    if (set == 0)
                    {
                        blogPost = MapBlogPost(reader);
                    }
                    else if (set == 1)
                    {
                        string tag = reader.GetSafeString(0);

                        if (tagsList == null)
                        {
                            tagsList = new List<string>();
                        }
                        tagsList.Add(tag);
                    }
                    else if (set == 2 && blogPost != null)
                    {
                        MetaTag metaTag = MapMetaTags(reader);

                        if (metaTagsList == null)
                        {
                            metaTagsList = new List<MetaTag>();
                        }
                        metaTagsList.Add(metaTag);
                    }
                }
           );

            if (blogPost != null)
            {
                blogPost.Tags = tagsList;
                blogPost.MetaTags = metaTagsList;
            }

            return blogPost;
        }

        public List<MetaTag> GetMetaTags(int year, int month, int day, string title) //--------------------WORK ON THIS TOO--------------------
        {
            string datePublished = GetDate(year, month, day);

            List<MetaTag> metaTagsList = null;

            DataProvider.ExecuteCmd(
                GetConnection,
                "[dbo].[BlogPosts_SelectMetaTags]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@DatePublished", datePublished);
                    parameters.AddWithValue("@Title", title);
                },
                map: delegate (IDataReader reader, short set)
                {
                    MetaTag metaTag = MapMetaTags(reader);

                    if (metaTagsList == null)
                    {
                        metaTagsList = new List<MetaTag>();
                    }
                    metaTagsList.Add(metaTag);
                }
           );

            return metaTagsList;
        }

        public PagedList<BlogPost> GetTop(int pageIndex, int itemsPerPage)
        {
            PagedList<BlogPost> pagedList = null;
            List<BlogPost> blogsList = null;
            int totalCount = 0;

            DataProvider.ExecuteCmd(
                GetConnection,
                "[dbo].[BlogPosts_SelectForConsumer]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@PageIndex", pageIndex);
                    parameters.AddWithValue("@NumberOfRecords", itemsPerPage);
                },
                map: delegate (IDataReader reader, short set)
                {
                    MpagePagedList(pageIndex, itemsPerPage, reader, ref pagedList, ref blogsList, ref totalCount);
                }
                );
            return pagedList;
        }

        public PagedList<BlogPost> GetConsumer(int pageIndex, int itemsPerPage)
        {
            return GetPage(pageIndex, itemsPerPage, false);
        }

        public PagedList<BlogPost> GetAdmin(int pageIndex, int itemsPerPage)
        {
            return GetPage(pageIndex, itemsPerPage, null);
        }


        private PagedList<BlogPost> GetPage(int pageIndex, int itemsPerPage, bool? isDeleted = null)
        {
            PagedList<BlogPost> pagedList = null;
            List<BlogPost> blogsList = null;
            int totalCount = 0;

            DataProvider.ExecuteCmd(
                GetConnection,
                "[dbo].[BlogPosts_SelectPage]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@PageIndex", pageIndex);
                    parameters.AddWithValue("@NumberOfRecords", itemsPerPage);

                    if(!isDeleted.HasValue)
                    {
                        parameters.AddWithValue("@isDeleted", DBNull.Value );
                    }
                    else
                    {
                        parameters.AddWithValue("@isDeleted", isDeleted);
                    }
                   
                },
                map: delegate (IDataReader reader, short set)
                {
                    MpagePagedList(pageIndex, itemsPerPage, reader, ref pagedList, ref blogsList, ref totalCount);
                }
                );

            return pagedList;
        }

        public PagedList<BlogPost> GetByTag(string blogTag, int pageIndex, int itemsPerPage)
        {
            PagedList<BlogPost> pagedList = null;
            List<BlogPost> blogsList = null;
            int totalCount = 0;

            DataProvider.ExecuteCmd(
                GetConnection,
                "[dbo].[BlogPosts_SelectByTag]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@BlogTag", blogTag);
                    parameters.AddWithValue("@PageIndex", pageIndex);
                    parameters.AddWithValue("@NumberOfRecords", itemsPerPage);
                },
                map: delegate (IDataReader reader, short set)
                {
                    MpagePagedList(pageIndex, itemsPerPage, reader, ref pagedList, ref blogsList, ref totalCount);
                }
                );
            return pagedList;
        }
    }
}
