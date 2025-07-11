﻿using Models;
using Models.Resps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiRepos
{
    public interface IHttpClientFunctions
    {
        Task<bool> CheckServerAsync();
        Task<ApiResp> RequestAsync(RequestsTypes requestsType, string url, string? userToken = null, Object? content = null);
    }

    public class HttpClientFunctions() : HttpClient, IHttpClientFunctions
    {
        public async Task<bool> CheckServerAsync()
        {
            try
            {
                HttpClient httpClient = new();

                HttpResponseMessage httpResponse = await httpClient.GetAsync(ApiKeys.ApiAddress + "/imalive");

                return httpResponse != null && httpResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return ex.InnerException is not null && (ex.InnerException.Message == "Nenhuma conexão pôde ser feita porque a máquina de destino as recusou ativamente." || ex.InnerException.Message.Contains("Este host não é conhecido."))
                    ? false
                    : throw ex;
            }
        }

        public virtual async Task<ApiResp> RequestAsync(RequestsTypes requestsType, string url, string? userToken = null, Object? content = null)
        {
            try
            {
                HttpClient httpClient = new();

                if (userToken is not null)
                    httpClient.DefaultRequestHeaders.Add("authorization", "bearer " + userToken);

                HttpResponseMessage httpResponse = new();

                switch (requestsType)
                {
                    case RequestsTypes.Get:
                        httpResponse = await httpClient.GetAsync(url);
                        break;
                    case RequestsTypes.Post:
                        if (content is not null)
                        {
                            string jsonContent = content as string;

                            StringContent bodyContent = new(jsonContent, Encoding.UTF8, "application/json");
                            httpResponse = await httpClient.PostAsync(url, bodyContent);
                        }
                        else return new ApiResp() { Success = false, Content = null, Error = ErrorTypes.BodyContentNull };
                        break;
                    case RequestsTypes.Put:
                        if (content is not null)
                        {
                            string jsonContent = content as string;
                            StringContent bodyContent = new(jsonContent, Encoding.UTF8, "application/json");
                            httpResponse = await httpClient.PutAsync(url, bodyContent);
                        }
                        else return new ApiResp() { Success = false, Content = null, Error = ErrorTypes.BodyContentNull };
                        break;
                    case RequestsTypes.Delete:
                        httpResponse = await httpClient.DeleteAsync(url);
                        break;
                }

                return new ApiResp()
                {
                    Success = httpResponse.IsSuccessStatusCode,
                    Error = httpResponse.StatusCode == HttpStatusCode.Unauthorized ? ErrorTypes.Unauthorized : null,
                    TryRefreshToken = httpResponse.StatusCode == HttpStatusCode.Unauthorized,
                    Content = await httpResponse.Content.ReadAsStringAsync()
                };
            }
            catch (Exception ex)
            {
                return ex.InnerException is not null && (ex.InnerException.Message == "Nenhuma conexão pôde ser feita porque a máquina de destino as recusou ativamente." || ex.InnerException.Message.Contains("Este host não é conhecido."))
                    ? new ApiResp() { Success = false, Content = null, Error = ErrorTypes.ServerUnavaliable }
                    : throw ex;
            }
        }
    }
}
