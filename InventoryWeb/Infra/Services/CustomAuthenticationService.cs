using Blazored.LocalStorage;
using Models;
using System.Security.Claims;
using System.Text.Json;

namespace InventoryWeb.Infra.Services
{
    public class CustomAuthenticationService
    {
        private User _currentUser;
        public User CurrentUser => _currentUser;

        public event Action<User> UserChanged;
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthenticationService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            // Inicializa _currentUser como um usuário anônimo/não autenticado
            // Isso é importante para que CurrentUser nunca seja nulo.
            _currentUser = new User { Name = "", Email = "" };
        }

        public async Task Login(User user)
        {
            await _localStorageService.SetItemAsync("Token", user.Token);
            await _localStorageService.SetItemAsync("Email", user.Email);
            NotifyUserChanged();
        }

        public async Task Logout()
        {
            _currentUser = new User();
            // **AQUI: Remove o token**
            await _localStorageService.RemoveItemAsync("Token");
            NotifyUserChanged();
        }

        public async Task InitializeUser()
        {
            // **PASSO 1: Tenta obter o token do localStorage**
            var token = await _localStorageService.GetItemAsync<string>("Token");
            var email = await _localStorageService.GetItemAsync<string>("Email");

            // **PASSO 2: Verifica se o token é nulo ou vazio**
            if (string.IsNullOrEmpty(token))
            {
                // Se não há token, o usuário não está autenticado ou o token expirou/foi removido.
                // Garante que o CurrentUser seja o usuário anônimo.
                _currentUser = new User();
                NotifyUserChanged(); // Notifica que o usuário é anônimo
                return; // Sai do método, não há token para processar
            }

            try
            {
                // **PASSO 3: Se o token existe, tenta decodificá-lo e processá-lo**
                //
                var claims = new List<Claim>();
                //var claims = ParseClaimsFromJwt(token); // Sua função para extrair claims

                // Exemplo de como extrair informações (ajuste conforme suas claims)
                //var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                //var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                //var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                // ... outras claims (permissions, id, etc.)

                // Verifica se as claims mínimas para um usuário válido estão presentes
                if (!string.IsNullOrEmpty(token))
                {
                    _currentUser = new User { Email = email, Token = token };
                    NotifyUserChanged(); // Notifica que o usuário foi restaurado
                }
                else
                {
                    // Se o token existe mas as claims essenciais estão faltando,
                    // pode ser um token malformado ou inválido.
                    // Limpa o token e força o logout.
                    await Logout();
                }
            }
            catch (Exception ex)
            {
                // **PASSO 4: Captura exceções se a decodificação do token falhar**
                // Isso pode acontecer se o token for inválido, malformado ou corrompido.
                Console.WriteLine($"Erro ao inicializar usuário do token: {ex.Message}");
                // Limpa o token inválido e força o logout.
                await Logout();
            }
        }

        private void NotifyUserChanged()
        {
            UserChanged?.Invoke(_currentUser);
        }

        // Certifique-se de que ParseClaimsFromJwt lida com tokens vazios ou malformados
        // e retorna uma lista vazia de Claims se não puder parsear.
        //private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        //{
        //    var claims = new List<Claim>();
        //    if (string.IsNullOrEmpty(jwt))
        //    {
        //        return claims;
        //    }

        //    try
        //    {
        //        var payload = jwt.Split('.')[1];
        //        var jsonBytes = ParseBase64WithoutPadding(payload);
        //        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        //        // Adicione seus mapeamentos de claims aqui. Ex:
        //        if (keyValuePairs.TryGetValue("name", out var name)) claims.Add(new Claim(ClaimTypes.Name, name.ToString()));
        //        if (keyValuePairs.TryGetValue("email", out var email)) claims.Add(new Claim(ClaimTypes.Email, email.ToString()));
        //        // Adicione outras claims importantes como roles, etc.
        //        if (keyValuePairs.TryGetValue("role", out var roleObject))
        //        {
        //            if (roleObject is JsonElement roleElement && roleElement.ValueKind == JsonValueKind.Array)
        //            {
        //                foreach (var roleItem in roleElement.EnumerateArray())
        //                {
        //                    claims.Add(new Claim(ClaimTypes.Role, roleItem.ToString()));
        //                }
        //            }
        //            else if (roleObject != null) // Se for uma string única
        //            {
        //                claims.Add(new Claim(ClaimTypes.Role, roleObject.ToString()));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Erro ao parsear JWT: {ex.Message}");
        //        // Retorna claims vazias se houver erro ao parsear
        //        return new List<Claim>();
        //    }
        //    return claims;
        //}

        //private byte[] ParseBase64WithoutPadding(string base64)
        //{
        //    // Sua implementação original aqui
        //    switch (base64.Length % 4)
        //    {
        //        case 2: base64 += "=="; break;
        //        case 3: base64 += "="; break;
        //    }
        //    return Convert.FromBase64String(base64);
        //}
    }
}
