using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudentRecruitment.Presentation.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private string _cachedToken;

        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetTokenAsync(string token)
        {
            _cachedToken = token;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }

        public async Task<string> GetTokenAsync()
        {
            if (_cachedToken != null)
            {
                return _cachedToken;
            }

            _cachedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            return _cachedToken;
        }

        public async Task ClearTokenAsync()
        {
            _cachedToken = null;
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }

        public async Task<string> GetUserRoleAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var roleClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "role");
            return roleClaim?.Value;
        }

        public async Task InitializeAsync()
        {
            _cachedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}