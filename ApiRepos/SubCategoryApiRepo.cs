using Models;
using Models.DTO;
using Models.Resps;
using System.Text.Json;
using UsersManagement.Model;

namespace ApiRepos
{
    public interface ISubCategoryApiRepo
    {
        Task<ApiResp> CreateAsync(SubCategoryDTO subCategory, string userToken);
        Task<ApiResp> DelSubCategory(int id, string userToken);
        Task<ApiResp> GetByLastUpdateAsync(DateTime lastUpdate, int page, string userToken);
        Task<ApiResp> GetSubCategoriesByCategoryId(string subCategoryId, string userToken);
        Task<ApiResp> GetSubCategoryById(string id, string userToken);
        Task<ApiResp> UpdateAsync(SubCategoryDTO subCategory, string userToken);
        Task<ApiResp> GetById(string id, string userToken);
    }

    public class SubCategoryApiRepo(IHttpClientFunctions httpClientFunctions) : ISubCategoryApiRepo
    {
        public async Task<ApiResp> GetSubCategoriesByCategoryId(string subCategoryId, string userToken) =>
            await httpClientFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/subcategory/category/" + subCategoryId, userToken);

        public async Task<ApiResp> GetSubCategoryById(string id, string userToken) =>
            await httpClientFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/subcategory/" + id, userToken);

        public async Task<ApiResp> UpdateAsync(SubCategoryDTO subCategory, string userToken)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { subCategory.Name, subCategory.IconName, subCategory.CategoryId });

                return await httpClientFunctions.RequestAsync(RequestsTypes.Put, ApiKeys.ApiAddress + "/Inventory/subcategory/" + subCategory.Id, userToken, json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> CreateAsync(SubCategoryDTO subCategory, string userToken)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { subCategory.Name, subCategory.IconName, subCategory.CategoryId });

                return await httpClientFunctions.RequestAsync(RequestsTypes.Post, ApiKeys.ApiAddress + "/Inventory/subcategory", userToken, json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> DelSubCategory(int id, string userToken) =>
            await httpClientFunctions.RequestAsync(RequestsTypes.Delete, ApiKeys.ApiAddress + "/Inventory/subCategory/" + id, userToken);

        public async Task<ApiResp> GetByLastUpdateAsync(DateTime lastUpdate, int page, string userToken) =>
            await httpClientFunctions.RequestAsync(RequestsTypes.Get, $"{ApiKeys.ApiAddress}/Inventory/subCategory/byAfterUpdatedAt/{lastUpdate:yyyy-MM-ddThh:mm:ss.fff}/{page}", userToken);

        public async Task<ApiResp> GetById(string id, string userToken) =>
            await httpClientFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/subcategory/" + id, userToken);

    }
}
