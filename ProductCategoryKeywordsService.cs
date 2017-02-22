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
    public class ProductCategoryKeywordsService : BaseService, IProductCategoryKeywordsService

    {
        private ProductCategoryKeyword MapDataToPass(IDataReader Reader)
        {
            ProductCategoryKeyword P = new ProductCategoryKeyword();
            int StartingIndex = 0; //startingOrdinal

            P.Id = Reader.GetSafeInt32(StartingIndex++);
            P.DateCreated = Reader.GetSafeDateTime(StartingIndex++);
            P.DateModified = Reader.GetSafeDateTime(StartingIndex++);
            P.Keyword = Reader.GetSafeString(StartingIndex++);
            P.CategoryId = Reader.GetSafeInt32(StartingIndex++);


            return P;

        }
        public int Create(ProductCategoryKeywordsCreateRequest NewSearch)
        {
            int Result = -1;
            DataProvider.ExecuteNonQuery(GetConnection, "[dbo].[ProductCategoryKeywords_Insert]",
                inputParamMapper: delegate (SqlParameterCollection Parameters)
                {
                    Parameters.AddWithValue("@keyword", NewSearch.Keyword);
                    Parameters.AddWithValue("@CategoryId", NewSearch.CategoryId);

                    SqlParameter IdParam = new SqlParameter("@Id", DbType.Int32);
                    IdParam.Direction = ParameterDirection.Output;
                    Parameters.Add(IdParam);
                },
                returnParameters: delegate (SqlParameterCollection Parameters)
                {
                    Result = int.Parse(Parameters["@Id"].Value.ToString());
                }
            );
            return Result;
        }

        public void UpdateCategoryKeywords(ProductCategoryKeywordsUpdateRequest UpdateCategory)
        {
            DataProvider.ExecuteNonQuery(
                GetConnection, "[dbo].[ProductCategoryKeywords_Update]",
                inputParamMapper: delegate (SqlParameterCollection Parameters)
                {
                    Parameters.AddWithValue("@keyword", UpdateCategory.Keyword);
                    Parameters.AddWithValue("@CategoryId", UpdateCategory.CategoryId);
                    Parameters.AddWithValue("@Id", UpdateCategory.Id);

                },
                returnParameters: delegate (SqlParameterCollection parameters)
                {
                }
           );
        }

        public ProductCategoryKeyword KeywordsGetById(int id)
        {
            ProductCategoryKeyword P = null;
            DataProvider.ExecuteCmd(GetConnection, "[dbo].[ProductCategoryKeywords_selectById]",
            inputParamMapper: delegate (SqlParameterCollection Parameters)
                {
                    Parameters.AddWithValue("@Id", id);
                },
                map: delegate (IDataReader Reader, short set)
                {
                    P = MapDataToPass(Reader);
                }

                );
            return P;
        }



        public List<ProductCategoryKeyword> GetKeywordsByCategoryId(int categoryId)
        {
            List<ProductCategoryKeyword> List = null;

            DataProvider.ExecuteCmd(GetConnection, "[dbo].[ProductCategoryKeywords_SelectByCategoryId]"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@CategoryId", categoryId);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   ProductCategoryKeyword P = MapDataToPass(reader);

                   if (List == null)
                   {
                       List = new List<ProductCategoryKeyword>();
                   }

                   List.Add(P);
               }
               );


            return List;
        }
        public List<ProductCategoryKeyword> GetProductCategoryKeywords()
        {
            List<ProductCategoryKeyword> List = null;

            DataProvider.ExecuteCmd(GetConnection, "[dbo].[ProductCategoryKeywords_selectAll]"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

               }
               , map: delegate (IDataReader reader, short set)
               {
                   ProductCategoryKeyword P = MapDataToPass(reader);

                   if (List == null)
                   {
                       List = new List<ProductCategoryKeyword>();
                   }

                   List.Add(P);
               }
               );


            return List;
        }
    }
}