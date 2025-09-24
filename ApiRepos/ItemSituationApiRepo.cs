using Models.Resps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRepos
{
    public interface IItemSituationApiRepo
    {
        Task<ApiResp> GetItemSituation(string userToken);
    }

    public class ItemSituationApiRepo(IHttpClientFunctions httpClientFunctions) : IItemSituationApiRepo
    {
        public async Task<ApiResp> GetItemSituation(string userToken)
            => await httpClientFunctions.RequestAsync(Models.RequestsTypes.Get, "Inventory/itemsituation", userToken);
    }
}
