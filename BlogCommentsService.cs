using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class BlogCommentsService : BaseService, IBlogCommentsService
    {
        private IUserService _userService;

        public BlogCommentsService(IUserService userService)
        {
            _userService = userService;
        }
        private static BlogComment MapProfile(IDataReader reader)
        {
            BlogComment newComment = new BlogComment();
            int startingIndex = 0;

            newComment.Id = reader.GetSafeInt32(startingIndex++);
            newComment.ParentCommentId = reader.GetSafeInt32(startingIndex++);
            newComment.Name = reader.GetSafeString(startingIndex++);
            newComment.Content = reader.GetSafeString(startingIndex++);
            newComment.DateCreated = reader.GetSafeDateTime(startingIndex++);
            newComment.DateModified = reader.GetSafeDateTime(startingIndex++);
            newComment.UserId = reader.GetSafeString(startingIndex++);
            newComment.BlogPostId = reader.GetSafeInt32(startingIndex++);

            return newComment;
        }


        public int CreateBlog(BlogCommentsCreateRequest newBlog)
        {
            int result = -1;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.BlogComments_Insert",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    if (newBlog.ParentCommentId.HasValue)
                    {
                        parameters.AddWithValue("@ParentCommentId", newBlog.ParentCommentId);
                    } else
                    {
                        parameters.AddWithValue("@ParentCommentId", DBNull.Value);
                    }
                    parameters.AddWithValue("@Name", newBlog.Name);
                    parameters.AddWithValue("@Content", newBlog.Content);
                    parameters.AddWithValue("@UserId", _userService.GetCurrentUserId());
                    parameters.AddWithValue("@BlogPostId", newBlog.BlogPostId);

                    parameters.Add(new SqlParameter
                    {
                        DbType = DbType.Int32,
                        Direction = ParameterDirection.Output,
                        ParameterName = "@Id"
                    });
                },
                returnParameters: delegate (SqlParameterCollection parameters)
                {
                    result = int.Parse(parameters["@Id"].Value.ToString());
                }
            );
            return result;
        }

        public void UpdateBlog(BlogCommentsUpdateRequest newBlog)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.BlogComments_Update",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@Id", newBlog.Id);
                    if (newBlog.ParentCommentId.HasValue)
                    {
                        parameters.AddWithValue("@ParentCommentId", newBlog.ParentCommentId);
                    }
                    else
                    {
                        parameters.AddWithValue("@ParentCommentId", DBNull.Value);
                    }
                    parameters.AddWithValue("@Name", newBlog.Name);
                    parameters.AddWithValue("@Content", newBlog.Content);
                    parameters.AddWithValue("@UserId", _userService.GetCurrentUserId());
                }
            );

        }

        public List<Domain.BlogComment> GetAll()
        {
            List<BlogComment> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.BlogComments_SelectAll"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
               }
               , map: delegate (IDataReader reader, short set)
               {
                   BlogComment newComment = MapProfile(reader);

                   if (list == null)
                   {
                       list = new List<BlogComment>();
                   }

                   list.Add(newComment);
               }
               );

            return list;
        }

        public void DeleteComment(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.BlogComments_UpdateIsDeleted",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@Id", Id);
                });
        }

        public Domain.BlogComment GetById(int Id)
        {
            BlogComment edit = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.BlogComments_SelectById"
               , inputParamMapper: delegate (SqlParameterCollection parameters)
               {
                   parameters.AddWithValue("@Id", Id);

               }
               , map: delegate (IDataReader reader, short set)
               {
                   edit = MapProfile(reader);
               }
               );
            return edit;
        }
    }
}
