using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace InventoryWeb.Infra.Handlers
{
    public class CustomAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;

        // Injeta o ILocalStorageService para acessar o token persistido
        public CustomAuthorizationMessageHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Tenta obter o token do localStorage
            var token = await _localStorageService.GetItemAsync<string>("authToken");

            // Se o token existir, adiciona-o ao cabeçalho Authorization da requisição
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Continua o processamento da requisição
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
