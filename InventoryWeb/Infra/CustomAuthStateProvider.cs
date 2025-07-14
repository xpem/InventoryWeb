namespace InventoryWeb.Infra
{
    using Blazored.LocalStorage;
    using InventoryWeb.Infra.Services;
    using Microsoft.AspNetCore.Components.Authorization;
    using Models;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Cryptography;

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        private readonly CustomAuthenticationService _authService;

        public CustomAuthStateProvider(CustomAuthenticationService authService)
        {
            _authService = authService;

            _authService.UserChanged += (newUser) =>
            {
                var authenticatedUser = CreateClaimsPrincipal(newUser);
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
            };

            // NOTA: Remover NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
            // do construtor, pois GetAuthenticationStateAsync() será chamado logo em seguida
            // pelo Blazor na inicialização, e isso pode causar uma notificação dupla.
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Isso garantirá que _authService.CurrentUser esteja atualizado,
            // seja com o usuário logado ou com o usuário "Guest" se não houver token.
            await _authService.InitializeUser();

            // CreateClaimsPrincipal agora receberá um objeto User válido (mesmo que seja o "Guest")
            // e retornará um ClaimsPrincipal apropriado (autenticado ou anônimo).
            var userClaimsPrincipal = CreateClaimsPrincipal(_authService.CurrentUser);

            return new AuthenticationState(userClaimsPrincipal);
        }

        // ... (NotifyUserAuthentication e NotifyUserLogout)
        public void NotifyUserLogout()
        {
            // Define o estado de autenticação como anônimo
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
        }


        private ClaimsPrincipal CreateClaimsPrincipal(User user)
        {
            // Garante que 'user' nunca seja nulo devido à inicialização no AuthService
            // e o tratamento de token nulo/inválido lá.
            if (user == null || string.IsNullOrEmpty(user.Token)) 
            {
                return anonymousUser;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name ?? string.Empty),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new("Token", user.Token ?? string.Empty)
            };

            //if (user.Roles != null && user.Roles.Any())
            //{
            //    claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //}
            // ... adicione outras claims como permissões se seu objeto User as tiver

            // Se o usuário tem um token, ele é considerado autenticado para o ClaimsIdentity
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuth"));
        }

    }
}
