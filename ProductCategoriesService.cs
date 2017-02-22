using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Models;
using Sabio.Web.Services.Interfaces;
using Sabio.Web;
using System.Web.Configuration;

namespace Sabio.Web.Services
{
    public class ProductCategoriesService : BaseService, IProductCategoriesService
    {
        private IUserService _userService;

        public ProductCategoriesService(IUserService userService)
        {
            _userService = userService;
        }

        private static ProductCategory MapProductCategories(IDataReader reader)
        {
            ProductCategory thisProductCategories = new ProductCategory();
            int startingIndex = 0;

            thisProductCategories.Id = reader.GetSafeInt32(startingIndex++);
            thisProductCategories.ParentCategoryId = reader.GetSafeInt32(startingIndex++);
            thisProductCategories.DisplayName = reader.GetSafeString(startingIndex++);
            thisProductCategories.ProductCategoryKey = reader.GetSafeString(startingIndex++);
            thisProductCategories.DateCreated = reader.GetSafeDateTime(startingIndex++);
            thisProductCategories.DateModified = reader.GetSafeDateTime(startingIndex++);
            thisProductCategories.UserId = reader.GetSafeString(startingIndex++);
            thisProductCategories.IsDeleted = reader.GetBoolean(startingIndex++);
            thisProductCategories.ImageUrl = reader.GetSafeString(startingIndex++);



            return thisProductCategories;

        }

        private static ProductCategory MapCategoryByDisplayName(IDataReader reader)
        {
            ProductCategory thisProductDisplayNanme = new ProductCategory();
            int startingIndex = 0;

            thisProductDisplayNanme.Id = reader.GetSafeInt32(startingIndex++);
            thisProductDisplayNanme.DisplayName = reader.GetSafeString(startingIndex++);

            return thisProductDisplayNanme;

        }
        public int CreateProductCategories(ProductCategoriesCreateRequest newCategories)
        {
            int result = -1;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.ProductCategories_Insert",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@ProductCategoryKey", newCategories.ProductCategoryKey);
                    parameters.AddWithValue("@DisplayName", newCategories.DisplayName);


                    if (newCategories.ImageUrl != null)
                    {
                        parameters.AddWithValue("@ImageUrl", newCategories.ImageUrl);
                    }
                    else
                    {
                        parameters.AddWithValue("@ImageUrl", DBNull.Value);
                    }
                    parameters.AddWithValue("@UserId", _userService.GetCurrentUserId());
                    if (newCategories.ParentCategoryId.HasValue)
                    {
                        parameters.AddWithValue("@ParentCategoryId", newCategories.ParentCategoryId);
                    }
                    else
                    {
                        parameters.AddWithValue("@ParentCategoryId", DBNull.Value);
                    }

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

        public void UpdateProductCategories(ProductCategoriesUpdateRequest existingProductCategories)
        {
            DataProvider.ExecuteNonQuery(
            GetConnection, "[dbo].ProductCategories_Update",
            inputParamMapper: delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@ProductCategoryKey", existingProductCategories.ProductCategoryKey);
                parameters.AddWithValue("@DisplayName", existingProductCategories.DisplayName);

                if (existingProductCategories.ImageUrl != null)
                {
                    parameters.AddWithValue("@ImageUrl", existingProductCategories.ImageUrl);
                }
                else
                {
                    parameters.AddWithValue("@ImageUrl", DBNull.Value);
                }

                if (existingProductCategories.ParentCategoryId == null)
                {
                    parameters.AddWithValue("@ParentCategoryId", DBNull.Value);
                }
                else
                {
                    parameters.AddWithValue("@ParentCategoryId", existingProductCategories.ParentCategoryId);
                }

                //parameters.AddWithValue("@UserId", "123");
                parameters.AddWithValue("@Id", existingProductCategories.Id);
            });
        }

        public List<ProductCategory> SelectAll()
        {
            List<ProductCategory> list = null;
            DataProvider.ExecuteCmd(GetConnection, "[dbo].ProductCategories_SelectAll",
                inputParamMapper: null,
                map: delegate (IDataReader reader, short set)
                {
                    ProductCategory ProductCategories = MapProductCategories(reader);
                    if (list == null)
                    {
                        list = new List<ProductCategory>();
                    }

                    list.Add(ProductCategories);
                });

            return list;
        }

        public void DeleteProductCategories(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.ProductCategories_UpdatedisDeleted",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@Id", Id);
                });
        }

        public ProductCategory GetProductCategoryById(int id)
        {
            ProductCategory cat = null;
            DataProvider.ExecuteCmd(
                GetConnection,
                "[dbo].[ProductCategories_SelectByID]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@Id", id);
                },
                map: delegate (IDataReader reader, short set)
                {
                    cat = MapProductCategories(reader);
                }
           );

            return cat;
        }

        public List<ProductCategory> GetTopLevel()
        {
            List<ProductCategory> list = null;
            DataProvider.ExecuteCmd(
                GetConnection,
                "[dbo].[ProductCategories_SelectTopLevel]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                },
                map: delegate (IDataReader reader, short set)
                {
                    ProductCategory productCategories = MapProductCategories(reader);

                    if (list == null)
                    {
                        list = new List<ProductCategory>();
                    }

                    list.Add(productCategories);
                }
           );
            return list;
        }

        public List<ProductCategory> Get(int parentId)
        {
            List<ProductCategory> list = null;
            DataProvider.ExecuteCmd(
                GetConnection,
                "[dbo].[ProductCategories_SelectSubCategory]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters.AddWithValue("@Id", parentId);
                },
                map: delegate (IDataReader reader, short set)
                {
                    ProductCategory productCategories = MapProductCategories(reader);

                    if (list == null)
                    {
                        list = new List<ProductCategory>();
                    }

                    list.Add(productCategories);
                }
           );
            return list;
        }

        public PagedList<ProductCategory> GetCategoriesByIndex(int pageIndex, int itemsPerPage)
        {
            PagedList<ProductCategory> pagedList = null;
            List<ProductCategory> productList = null;
            int totalCount;


            DataProvider.ExecuteCmd(GetConnection, "[dbo].[ProductCategories_GetByPagination]",
            inputParamMapper: delegate (SqlParameterCollection parameters)
            {
                parameters.AddWithValue("@PageIndex", pageIndex);
                parameters.AddWithValue("@NumberOfRecords", itemsPerPage);

            },
            map: delegate (IDataReader reader, short set)
            {
                ProductCategory categories = new ProductCategory();
                int startingIndex = 0;

                categories.Id = reader.GetSafeInt32(startingIndex++);
                categories.ParentCategoryId = reader.GetSafeInt32(startingIndex++);
                categories.DisplayName = reader.GetSafeString(startingIndex++);
                categories.ProductCategoryKey = reader.GetSafeString(startingIndex++);
                categories.DateCreated = reader.GetSafeDateTime(startingIndex++);
                categories.DateModified = reader.GetDateTime(startingIndex++);
                categories.UserId = reader.GetSafeString(startingIndex++);
                categories.IsDeleted = reader.GetSafeBool(startingIndex++);
                categories.ImageUrl = reader.GetSafeString(startingIndex++);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (productList == null)
                {
                    productList = new List<ProductCategory>();
                }

                productList.Add(categories);

                if (pagedList == null)
                {
                    pagedList = new PagedList<ProductCategory>(productList, pageIndex, itemsPerPage, totalCount);
                }
            }
            );
            return pagedList;

        }

        public List<ProductCategory> GetCategoriesByDisplayName()
        {
            List<ProductCategory> list = null;
            DataProvider.ExecuteCmd(
                GetConnection,
              "[dbo].[ProductCategories_GetIdAndCatName]",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    
                },
                map: delegate (IDataReader reader, short set)
                {
                    ProductCategory productCategories = MapCategoryByDisplayName(reader);

                    if (list == null)
                    {
                        list = new List<ProductCategory>();
                    }

                    list.Add(productCategories);
                }
           );
            return list;
        }

        public List<ProductCategory> GetSubCategories(int id)
        {
            List<ProductCategory> subCatsList = null;
            string connStr = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "dbo.ProductCategories_SelectSubCategoryByReqest";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter idParam = new SqlParameter("@RequestId", id); //parameters.Addwithvalue
                    cmd.Parameters.Add(idParam);

                    cmd.Connection = conn;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ProductCategory subCats = MapCategoryByDisplayName(reader);

                                if (subCatsList == null)
                                {
                                    subCatsList = new List<ProductCategory>();
                                }
                                subCatsList.Add(subCats);
                            }
                        }
                    }
                }
            }

            return subCatsList;
        }




    }
}