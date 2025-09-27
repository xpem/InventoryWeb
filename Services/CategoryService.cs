using ApiRepos;
using Models.DTO;
using Models.Resps;
using Services.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Services
{
    public interface ICategoryService
    {
        Task<ServResp> InsertCategoryAsync(CategoryDTO category, string userToken);
        Task<ServResp> UpdateCategoryAsync(CategoryDTO category, string userToken);
        Task<ServResp> DelCategoryAsync(int id, string userToken);
        Task<ServResp> GetCategoriesAsync(string userToken);
        Task<ServResp> GetCategoriesWithSubCategoriesAsync(string userToken, int? id = null);
        Task<ServResp> GetCategoryByIdAsync(string id, string userToken);
    }

    public class CategoryService(ICategoryApiRepo categoryApiRepo) : ICategoryService
    {
        public async Task<ServResp> GetCategoriesAsync(string userToken) => ApiRespHandler.Handler<List<CategoryDTO>>(await categoryApiRepo.GetCategoriesAsync(userToken));

        public async Task<ServResp> GetCategoriesWithSubCategoriesAsync(string userToken, int? id = null) => 
            ApiRespHandler.Handler<List<CategoryDTO>>(await categoryApiRepo.GetCategoriesWithSubCategoriesAsync(userToken, id));

        public async Task<ServResp> GetCategoryByIdAsync(string id, string userToken) => ApiRespHandler.Handler<CategoryDTO>(await categoryApiRepo.GetCategoryByIdAsync(id, userToken));

        public async Task<ServResp> InsertCategoryAsync(CategoryDTO category, string userToken)
        {
            ApiResp? resp = await categoryApiRepo.InsertCategoryAsync(category, userToken);

            if (resp is not null && resp.Content is not null and string)
            {
                if (resp.Success)
                {
                    return new ServResp() { Success = resp.Success, Content = null };
                }
                else return new ServResp() { Success = false, Content = resp.Content };
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> UpdateCategoryAsync(CategoryDTO category, string userToken)
        {
            ApiResp? resp = await categoryApiRepo.UpdateCategoryAsync(category, userToken);

            if (resp is not null && resp.Content is not null and string)
            {
                if (resp.Success)
                {
                    JsonNode? jResp = JsonNode.Parse(resp.Content as string);
                    if (jResp is not null)
                    {
                        CategoryDTO categoryResp = new()
                        {
                            Id = jResp["Id"]?.GetValue<int>() ?? 0,
                            Name = jResp["Name"]?.GetValue<string>(),
                            Color = jResp["Color"]?.GetValue<string>(),
                            SystemDefault = jResp["SystemDefault"]?.GetValue<bool>()
                        };

                        return new ServResp() { Success = resp.Success, Content = categoryResp };
                    }
                }
                else return new ServResp() { Success = false, Content = resp.Content };
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> DelCategoryAsync(int id, string userToken)
        {
            ApiResp? resp = await categoryApiRepo.DelCategoryAsync(id, userToken);

            return resp is not null && resp.Content is not null
                ? new ServResp() { Success = resp.Success, Content = resp.Content }
                : new ServResp() { Success = false, Content = null };
        }
    }
}
