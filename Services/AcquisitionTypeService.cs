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
    public interface IAcquisitionTypeService
    {
        Task<ServResp> GetAcquisitionType(string userToken);
    }

    public partial class AcquisitionTypeService(IAcquisitionTypeApiRepo acquisitionTypeApiRepo) : IAcquisitionTypeService
    {
        public async Task<ServResp> GetAcquisitionType(string userToken)
        {
            ApiResp resp = await acquisitionTypeApiRepo.GetAcquisitionType(userToken);

            return ApiRespHandler.Handler<List<AcquisitionType>>(resp);
        }
    }
}
