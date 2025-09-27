using Models;
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
        Task<ApiResp> GetCategoriesWithSubCategoriesAsync(string userToken, int? id = null);
        Task<ApiResp> GetCategoryByIdAsync(string id, string userToken);
    }

    public class CategoryApiRepo(IHttpClientFunctions httpClientFunctions) : ICategoryApiRepo
    {
        public async Task<ApiResp> GetCategoriesAsync(string userToken) =>
          await httpClientFunctions.RequestAsync(RequestsTypes.Get, "Inventory/category", userToken);

        public async Task<ApiResp> GetCategoriesWithSubCategoriesAsync(string userToken, int? id = null)
        {

            string url = "Inventory/category";

            if (id is not null)
                url += "/" + id;

            url += "/subcategory";

            return await httpClientFunctions.RequestAsync(RequestsTypes.Get, url, userToken);
        }

        public async Task<ApiResp> GetCategoryByIdAsync(string id, string userToken) =>
            await httpClientFunctions.RequestAsync(RequestsTypes.Get, "Inventory/category/" + id, userToken);

        public async Task<ApiResp> InsertCategoryAsync(CategoryDTO category, string userToken)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { category.Name, category.Color });

                return await httpClientFunctions.RequestAsync(RequestsTypes.Post, "Inventory/category", userToken, json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> UpdateCategoryAsync(CategoryDTO category, string userToken)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { category.Name, category.Color });

                return await httpClientFunctions.RequestAsync(RequestsTypes.Put, "Inventory/category/" + category.Id, userToken, json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> DelCategoryAsync(int id, string userToken)
        {
            try
            {
                return await httpClientFunctions.RequestAsync(RequestsTypes.Delete, "Inventory/category/" + id, userToken);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
