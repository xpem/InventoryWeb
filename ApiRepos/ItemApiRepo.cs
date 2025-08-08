using Models;
using Models.DTO;
using Models.Item.Files;
using Models.Resps;
using System.Text.Json;

namespace ApiRepos
{
    public interface IItemApiRepo
    {
        Task<ApiResp> InsertItem(ItemDTO item, string userToken);
        Task<ApiResp> AddItemImage(int id, string userToken, ItemFilesToUpload itemFilesToUpload);
        Task<ApiResp> UpdateItem(ItemDTO item, string userToken);
        Task<ApiResp> DelItemAsync(int id);
        Task<ApiResp> DelItemImageAsync(int id, string userToken, string fileName);
        Task<ApiResp> GetItemByIdAsync(string id, string userToken);
        Task<ApiResp> GetItemImageAsync(int id, string userToken, string fileName);
        Task<ApiResp> GetPaginatedItemsAsync(int page, string userToken);
        Task<ApiResp> GetItensAsync(string userToken);
    }

    public class ItemApiRepo(IHttpClientFunctions httpClientFunctions, IHttpClientWithFileFunctions httpClientWithFileFunctions) : IItemApiRepo
    {
        public async Task<ApiResp> GetItensAsync(string userToken) => await httpClientFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/item/totals", userToken);

        public async Task<ApiResp> GetPaginatedItemsAsync(int page, string userToken) =>
            await httpClientFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/item?page=" + page, userToken);

        public async Task<ApiResp> GetItemByIdAsync(string id, string userToken) =>
           await httpClientFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/item/" + id, userToken);

        public async Task<ApiResp> GetItemImageAsync(int id, string userToken, string fileName) =>
            await httpClientWithFileFunctions.RequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/item/" + id + "/image/" + fileName, userToken);

        public async Task<ApiResp> AddItemImage(int id, string userToken, ItemFilesToUpload itemFilesToUpload) =>
            await httpClientWithFileFunctions.RequestAsync(Models.RequestsTypes.Put, ApiKeys.ApiAddress + "/Inventory/item/" + id + "/image", userToken, itemFilesToUpload);

        public async Task<ApiResp> DelItemImageAsync(int id, string userToken, string fileName) =>
           await httpClientFunctions.RequestAsync(RequestsTypes.Delete, $"{ApiKeys.ApiAddress}/Inventory/item/{id}/image/{fileName}");

        public async Task<ApiResp> InsertItem(ItemDTO item, string userToken)
        {
            string json = BuildItemJson(item);

            return await httpClientFunctions.RequestAsync(Models.RequestsTypes.Post, ApiKeys.ApiAddress + "/Inventory/item", userToken, json);
        }

        private static string BuildItemJson(ItemDTO item) =>
            JsonSerializer.Serialize(new
            {
                item.Name,
                item.TechnicalDescription,
                AcquisitionDate = DateOnly.FromDateTime(item.AcquisitionDate),
                item.PurchaseValue,
                item.PurchaseStore,
                item.ResaleValue,
                SituationId = item.Situation?.Id,
                item.Comment,
                AcquisitionType = item.AcquisitionType?.Id,
                Category = new { CategoryId = item.Category?.Id, SubCategoryId = item.Category?.SubCategory is not null ? (int?)item.Category.SubCategory.Id : null },
                WithdrawalDate = item.WithdrawalDate != null ? (DateOnly?)DateOnly.FromDateTime(item.WithdrawalDate.Value) : null,
            });

        public async Task<ApiResp> UpdateItem(ItemDTO item, string userToken)
        {
            try
            {
                string json = BuildItemJson(item);

                return await httpClientFunctions.RequestAsync(Models.RequestsTypes.Put, ApiKeys.ApiAddress + "/Inventory/item/" + item.Id, userToken, json);
            }
            catch (Exception ex) { throw; }
        }

        public async Task<ApiResp> DelItemAsync(int id)
        {
            try
            {
                return await httpClientFunctions.RequestAsync(RequestsTypes.Delete, ApiKeys.ApiAddress + "/Inventory/item/" + id);
            }
            catch (Exception ex) { throw; }
        }
    }
}
