using Models.Resps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRepos
{
    public interface IAcquisitionTypeApiRepo
    {
        Task<ApiResp> GetAcquisitionType(string userToken);
    }

    public class AcquisitionTypeApiRepo(IHttpClientFunctions httpClientFunctions) : IAcquisitionTypeApiRepo
    {
        public async Task<ApiResp> GetAcquisitionType(string userToken) =>
            await httpClientFunctions.RequestAsync(Models.RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/acquisitiontype", userToken);
    }
}
