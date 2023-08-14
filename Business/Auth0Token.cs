using Auth0.AuthenticationApi.Models;
using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Microsoft.Extensions.Configuration;
using DataAccess.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Business
{
    public class Auth0Token
    {
        private readonly IConfiguration _configuration;
        private readonly AuthenticationApiClient _auth0Client;
        private readonly IHttpContextAccessor _contextAccessor;
       public Auth0Token(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _auth0Client = new AuthenticationApiClient(new Uri(_configuration["Auth0:Domain"]));
            _contextAccessor = httpContextAccessor;
        }

        public async Task<string> GetAccessToken(string email, string password)
        {
            var tokenRequest = new ResourceOwnerTokenRequest
            {
                ClientId = _configuration["Auth0:ClientId"],
                ClientSecret = _configuration["Auth0:ClientSecret"],
                Audience = _configuration["Auth0:Audience"],
                Scope = "openid profile", // Adjust the required scope based on your needs
              
                Username = email,
                Password = password
            };

            var tokenResponse = await _auth0Client.GetTokenAsync(tokenRequest);
           
            // Access token and other data obtained from the response
            var accessToken = tokenResponse.AccessToken;
            if(accessToken==null)
            {
                throw new Exception("Token couldn't provided.");
            }
            return accessToken;
        }
        public async Task<string> GetUsernameFromToken()
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not set.");
            }

            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
            var token = authorizationHeader["Bearer ".Length..].Trim();
            var user = await _auth0Client.GetUserInfoAsync(token.ToString());

            if (user != null)
            {
                return user.NickName;
            }

            return null;
        }
        public Task<bool> IsAdminAsync()
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not set.");
            }
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
            var token = authorizationHeader["Bearer ".Length..].Trim();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var permissionsClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "permissions");
            var permissions = permissionsClaim?.Value?.Split(',');

            return Task.FromResult(permissions != null && permissions.Length > 0);
        }
    }

}

