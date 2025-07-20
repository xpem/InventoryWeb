using ApiRepos;
using Models.DTO;
using Models.Resps;
using Services.Handlers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Services
{
    public interface ISubCategoryService
    {
        Task<ServResp> GetSubCategoriesByCategoryId(string userToken, int categoryId);
        Task<ServResp> GetByIdAsync(string id, string userToken);
        Task<ServResp> UpdateAsync(SubCategoryDTO subCategory, string userToken);
        Task<ServResp> CreateAsync(SubCategoryDTO subCategory, string userToken);
        Task<ServResp> DelSubCategory(int id, string userToken);
    }

    public class SubCategoryService(ISubCategoryApiRepo subCategoryApiRepo) : ISubCategoryService
    {
        public async Task<ServResp> GetSubCategoriesByCategoryId(string userToken, int categoryId)
        {
            ApiResp resp = await subCategoryApiRepo.GetSubCategoriesByCategoryId(categoryId.ToString(), userToken);

            return ApiRespHandler.Handler<List<SubCategoryDTO>>(resp);
        }


        public async Task<ServResp> GetByIdAsync(string id, string userToken)
        {
            var resp = await subCategoryApiRepo.GetById(id, userToken);

            return ApiRespHandler.Handler<SubCategoryDTO>(resp);
        }

        public async Task<ServResp> CreateAsync(SubCategoryDTO subCategory, string userToken)
        {
            ApiResp? resp = await subCategoryApiRepo.CreateAsync(subCategory, userToken);

            if (resp is not null && resp.Success && resp.Content is not null and string)
            {
                //JsonNode? jResp = JsonNode.Parse((string)resp.Content);
                //if (jResp is not null)
                //{
                //int id = jResp["id"]?.GetValue<int>() ?? 0;

                return new ServResp() { Success = resp.Success };
                //}
                //else return new ServResp() { Success = false, Content = resp.Content };
            }
            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> UpdateAsync(SubCategoryDTO subCategory, string userToken)
        {
            ApiResp? resp = await subCategoryApiRepo.UpdateAsync(subCategory, userToken);

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

        public async Task<ServResp> DelSubCategory(int id, string userToken)
        {
            ApiResp? resp = await subCategoryApiRepo.DelSubCategory(id, userToken);

            return resp is not null && resp.Content is not null
                ? new ServResp() { Success = resp.Success, Content = resp.Content }
                : new ServResp() { Success = false, Content = null };
        }
    }
}
