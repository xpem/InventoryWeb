﻿using Models;
using Models.DTO;
using Models.Resps;
using System.Text.Json;

namespace ApiRepos
{
    public interface ICategoryApiRepo
    {
        Task<ApiResp> InsertCategoryAsync(CategoryDTO category, string userToken);
        Task<ApiResp> UpdateCategoryAsync(CategoryDTO category, string userToken);
        Task<ApiResp> DelCategoryAsync(int id, string userToken);
        Task<ApiResp> GetCategoriesAsync(string userToken);
        Task<ApiResp> GetCategoriesWithSubCategoriesAsync(string userToken);
        Task<ApiResp> GetCategoryByIdAsync(string id, string userToken);
    }

    public class CategoryApiRepo(IHttpClientFunctions httpClientFunctions) : ICategoryApiRepo
    {
        public async Task<ApiResp> GetCategoriesAsync(string userToken) =>
          await httpClientFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/category", userToken);

        public async Task<ApiResp> GetCategoriesWithSubCategoriesAsync(string userToken) =>
            await httpClientFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/category/subcategory", userToken);

        public async Task<ApiResp> GetCategoryByIdAsync(string id, string userToken) =>
            await httpClientFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/category/" + id, userToken);

        public async Task<ApiResp> InsertCategoryAsync(CategoryDTO category, string userToken)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { category.Name, category.Color });

                return await httpClientFunctions.RequestAsync(RequestsTypes.Post, ApiKeys.ApiAddress + "/Inventory/category", userToken, json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> UpdateCategoryAsync(CategoryDTO category, string userToken)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { category.Name, category.Color });

                return await httpClientFunctions.RequestAsync(RequestsTypes.Put, ApiKeys.ApiAddress + "/Inventory/category/" + category.Id, userToken, json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> DelCategoryAsync(int id, string userToken)
        {
            try
            {
                return await httpClientFunctions.RequestAsync(RequestsTypes.Delete, ApiKeys.ApiAddress + "/Inventory/category/" + id, userToken);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
