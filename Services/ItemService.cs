using ApiRepos;
using Models;
using Models.DTO;
using Models.Item.Files;
using Models.Resps;
using Services.Handlers;
using System.Text.Json.Nodes;

namespace Services
{
    public interface IItemService
    {
        Task<ServResp> InsertItem(ItemDTO item, string userToken);
        Task<ServResp> UpdateItem(ItemDTO item, string userToken);
        Task<ServResp> DelItemAsync(int id);
        Task<ServResp> DelItemImageAsync(int id, string filename, string userToken);
        Task<ServResp> GetItemByIdAsync(string id, string userToken);
        Task<ImageFile> GetItemImages(int itemId, string itemImage1, string userToken);
        Task<List<ItemDTO>> GetItemsAsync(string userToken, int[]? situationIds);
        Task<ServResp> AddItemImageAsync(int id, string userToken, ItemFilesToUpload itemFilesToUpload);

        Task<ServResp> GetItemsTotalPages(string userToken, int[]? situationIds);

        Task<List<ItemDTO>> GetItemsPaginatedAsync(string userToken, int[]? situationIds, int page);
    }

    public class ItemService(IItemApiRepo itemApiRepo) : IItemService
    {
        public async Task<ServResp> GetItemsTotalPages(string userToken, int[]? situationIds)
        {
            ApiResp totalsResp = await itemApiRepo.GetTotalItensAsync(userToken, situationIds);

            return ApiRespHandler.Handler<ItemTotals>(totalsResp);
        }

        public async Task<List<ItemDTO>> GetItemsPaginatedAsync(string userToken, int[]? situationIds, int page)
        {
            List<ItemDTO> items = [];

            ApiResp resp = await itemApiRepo.GetPaginatedItemsAsync(page, userToken, situationIds);
            ServResp paginatedItemsBLLResponse = ApiRespHandler.Handler<List<ItemDTO>>(resp);

            if (paginatedItemsBLLResponse.Success)
                if (paginatedItemsBLLResponse.Content is List<ItemDTO> pageItems)
                    items.AddRange(pageItems);

            return items;
        }

        public async Task<List<ItemDTO>> GetItemsAsync(string userToken, int[]? situationIds)
        {
            ApiResp totalsResp = await itemApiRepo.GetTotalItensAsync(userToken, situationIds);
            List<ItemDTO> items = [];

            ServResp itemTotalsBLLResponse = ApiRespHandler.Handler<ItemTotals>(totalsResp);

            if (itemTotalsBLLResponse.Success)
            {
                ItemTotals? itemTotals = itemTotalsBLLResponse.Content as ItemTotals;

                for (int i = 1; i <= itemTotals?.TotalPages; i++)
                {
                    ApiResp resp = await itemApiRepo.GetPaginatedItemsAsync(i, userToken, situationIds);
                    ServResp paginatedItemsBLLResponse = ApiRespHandler.Handler<List<ItemDTO>>(resp);

                    if (paginatedItemsBLLResponse.Success)
                        if (paginatedItemsBLLResponse.Content is List<ItemDTO> pageItems)
                            items.AddRange(pageItems);
                }

                return items;
            }
            else
            {
                throw new ServerOffException("totalsResp success false, error:" + itemTotalsBLLResponse.Error);
            }
        }

        public async Task<ServResp> GetItemByIdAsync(string id, string userToken)
        {
            ApiResp resp = await itemApiRepo.GetItemByIdAsync(id, userToken);
            return ApiRespHandler.Handler<ItemDTO>(resp);
        }

        public async Task<ServResp> InsertItem(ItemDTO item, string userToken)
        {
            ApiResp? resp = await itemApiRepo.InsertItem(item, userToken);

            return resp is not null && resp.Success && resp.Content is not null and string
                ? ApiRespHandler.Handler<ItemDTO>(resp)
                : new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> UpdateItem(ItemDTO item, string userToken)
        {
            ApiResp? resp = await itemApiRepo.UpdateItem(item, userToken);

            if (resp is not null && resp.Success && resp.Content is not null and string)
            {
                JsonNode? jResp = JsonNode.Parse(resp.Content as string);

                return jResp is not null
                    ? new ServResp() { Success = resp.Success, Content = null }
                    : new ServResp() { Success = false, Content = resp.Content };
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> DelItemAsync(int id)
        {
            ApiResp? resp = await itemApiRepo.DelItemAsync(id);

            return resp is not null && !resp.Success && !string.IsNullOrEmpty(resp.Content as string)
                ? new ServResp() { Success = false, Content = resp.Content.ToString() }
                : new ServResp() { Success = true, Content = null };
        }

        public async Task<ServResp> DelItemImageAsync(int id, string filename, string userToken)
        {
            ApiResp resp = await itemApiRepo.DelItemImageAsync(id, userToken, filename);

            if (resp is not null && !resp.Success && !string.IsNullOrEmpty(resp.Content as string))
            {
                return new ServResp() { Success = false, Content = resp.Content.ToString() };
            }

            //BLLResponse itemResp = ApiResponseHandler.Handler<Item>(resp);
            return new ServResp() { Success = true, Content = null };
        }

        public async Task<ImageFile> GetItemImages(int itemId, string itemImage1, string userToken)
        {
            try
            {
                ApiResp resp = await itemApiRepo.GetItemImageAsync(itemId, userToken, itemImage1);

                if (resp is { Success: true, Content: Stream stream })
                {
                    using MemoryStream ms = new();
                    await stream.CopyToAsync(ms, bufferSize: 81920, cancellationToken: CancellationToken.None);
                    return new ImageFile
                    {
                        ImageBytes = ms.ToArray(),
                        FileName = itemImage1,
                    };
                }

                return null;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ServResp> AddItemImageAsync(int id, string userToken, ItemFilesToUpload itemFilesToUpload)
        {
            ApiResp resp = await itemApiRepo.AddItemImage(id, userToken, itemFilesToUpload);

            if (resp != null && resp.Content is not null)
            {
                ServResp? respBllResp = ApiRespHandler.Handler<ItemFileNames>(resp);

                if (respBllResp is not null && respBllResp.Success)
                {
                    ItemFileNames? itemFileNames = respBllResp.Content as ItemFileNames;

                    if (itemFileNames is not null)
                    {
                        //if (itemFileNames.Image1 is not null)
                        //{
                        //    string newPath = Path.Combine(FilePaths.ImagesPath, itemFileNames.Image1);

                        //    itemFilesToUpload.Image1.ImageFilePath = Path.Combine(FilePaths.ImagesPath, itemFileNames.Image1);
                        //}

                        //if (itemFileNames.Image2 is not null)
                        //{
                        //    string newPath = Path.Combine(FilePaths.ImagesPath, itemFileNames.Image2);
                        //    System.IO.File.Move(itemFilesToUpload.Image2.ImageFilePath, newPath);

                        //    itemFilesToUpload.Image2.ImageFilePath = Path.Combine(FilePaths.ImagesPath, itemFileNames.Image2);
                        //}

                        return new ServResp() { Success = true };
                    }
                }
            }

            return new ServResp() { Success = false };
        }

        //private async Task<ImageFile?> GetImageItemAsync(int id, int idx, string fileName, string filePath, string userToken)
        //{
        //    bool exists = System.IO.Directory.Exists(filePath);

        //    if (!exists)
        //        System.IO.Directory.CreateDirectory(filePath);

        //    string filePathAndName = Path.Combine(filePath, fileName);
        //    ImageFile imageFile;

        //    if (File.Exists(filePathAndName))
        //    {
        //        using FileStream fs = new FileStream(filePathAndName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        //        using MemoryStream memoryStream = new();
        //        fs.CopyTo(memoryStream);
        //        imageFile = new() { FileName = fileName, FileId = idx, ImageFilePath = filePathAndName };

        //        return imageFile;
        //    }

        //    ApiResp resp = await itemApiRepo.GetItemImageAsync(id, userToken, fileName);

        //    if (resp is not null && resp.Content is not null and Stream)
        //    {
        //        using FileStream fs = new FileStream(filePathAndName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        //        ((Stream)resp.Content).CopyTo(fs);

        //        imageFile = new() { FileName = fs.Name, FileId = idx, ImageFilePath = filePathAndName };

        //        await ((Stream)resp.Content).DisposeAsync();

        //        return imageFile;
        //    }

        //    return null;
        //}
    }
}
