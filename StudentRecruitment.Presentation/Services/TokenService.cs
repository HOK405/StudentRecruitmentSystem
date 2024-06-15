using Microsoft.JSInterop;
using StudentRecruitment.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace StudentRecruitment.Presentation.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private string _cachedToken;
        private readonly HttpClient _http;

        public TokenService(IJSRuntime jsRuntime, HttpClient http)
        {
            _jsRuntime = jsRuntime;
            _http = http;
        }

        public async Task InitializeAsync()
        {
            _cachedToken = await GetTokenFromLocalStorageAsync();
        }

        private async Task<string> GetTokenFromLocalStorageAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
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

            _cachedToken = await GetTokenFromLocalStorageAsync();
            return _cachedToken;
        }

        public async Task ClearTokenAsync()
        {
            _cachedToken = null;
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }

        public async Task LikeStudentAsync(LikeStudentDto likeStudentDto)
        {
            var response = await _http.PostAsJsonAsync("api/employer/like", likeStudentDto);
            response.EnsureSuccessStatusCode();
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

        public async Task<int> GetUserIdAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return 0;
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "nameid");
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}