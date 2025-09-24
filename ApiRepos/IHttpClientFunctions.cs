using Models;
using Models.Resps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRepos
{
    public interface IHttpClientFunctions
    {
        //Task<bool> CheckServerAsync();
        Task<ApiResp> RequestAsync(RequestsTypes requestsType, string url, string? userToken = null, Object? content = null);
    }
}
