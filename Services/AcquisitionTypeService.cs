using ApiRepos;
using Models.Item;
using Models.Resps;
using Services.Handlers;

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
