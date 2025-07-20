using ApiRepos;
using Models.Item;
using Models.Resps;
using Services.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IItemSituationService
    {
        Task<ServResp> GetItemSituation(string userToken);
    }

    public class ItemSituationService(IItemSituationApiRepo itemSituationApiRepo) : IItemSituationService
    {
        public async Task<ServResp> GetItemSituation(string userToken)
        {
            ApiResp resp = await itemSituationApiRepo.GetItemSituation(userToken);

            return ApiRespHandler.Handler<List<ItemSituation>>(resp);
        }
    }
}
