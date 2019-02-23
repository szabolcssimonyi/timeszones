using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TimeZones.Extensibility;
using TimeZones.Extensibility.Dto;
using TimeZones.Extensibility.Interfaces;

namespace TimeZones.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOptions<Configuration> options;
        private readonly ILogger<AuthenticationService> logger;
        private readonly UserManager<IdentityUser> userManager;

        public AuthenticationService(
            IOptions<Configuration> options,
            ILogger<AuthenticationService> logger,
            UserManager<IdentityUser> userManager)
        {
            this.options = options;
            this.logger = logger;
            this.userManager = userManager;
        }

        public string CreateToken(IdentityUser user)
        {
            var secret = options.Value.SecretKey;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credits = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: CreateClaimList(user),
                expires: DateTime.Now.AddSeconds(3600).ToUniversalTime(),
                signingCredentials: credits);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityUser> Login(LoginDto request)
        {
            var user = string.IsNullOrEmpty(request?.UserName) ? null : await userManager.FindByNameAsync(request.UserName);
            if (user == null || !await userManager.CheckPasswordAsync(user, request?.Password))
            {
                logger.LogInformation($@"Credential not found during login: {request?.UserName}/{request?.Password}");
                return null;
            }

            return user;
        }

        private IEnumerable<Claim> CreateClaimList(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim("Email", user.Email),
            };
            return claims;
        }
    }
}
