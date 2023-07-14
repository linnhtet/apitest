using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using WebApi.Services;
using System.Linq;
using WebApi.Models.Users;
using WebApi.Models.Token;
using WebApi.Entities;

namespace WebApi.Helpers
{
    public static class TokenHelper
    {
        public static ClaimsPrincipal GetClaims(string token, byte[] key)
        {
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            return handler.ValidateToken(token, validations, out var tokenSecure);
        }
        public static TokenInfo ReteiveTokenInfo(string token, string appSecret)
        {
            TokenInfo tokenInfo = null;
            if (string.IsNullOrEmpty(token))
            {
                return tokenInfo;
            }
            var key = Encoding.ASCII.GetBytes(appSecret);
            var claimsPricipal = GetClaims(token, key);

            if (claimsPricipal == null)
            {
                return tokenInfo;
            }

            tokenInfo = new TokenInfo
            {
                AMSSessionID = claimsPricipal.FindFirstValue(CustomClaimTypes.AMSSessionID),
                UserID = Int32.Parse(claimsPricipal.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value),
                LoginName = claimsPricipal.FindFirstValue(CustomClaimTypes.LoginName),
                StaffName = claimsPricipal.FindFirstValue(CustomClaimTypes.StaffName),
                StaffEmail = claimsPricipal.FindFirstValue(CustomClaimTypes.StaffEmail),
                CompanyID = claimsPricipal.FindFirstValue(CustomClaimTypes.CompanyID),
                CompanyCode = claimsPricipal.FindFirstValue(CustomClaimTypes.CompanyCode),
                UserRoles = claimsPricipal.FindFirstValue(CustomClaimTypes.UserRoles),
                UserRights = claimsPricipal.FindFirstValue(CustomClaimTypes.UserRights),
            };
            return tokenInfo;

        }

        public static string GenerateToken(string session,UserModel user,string appSecret,int tokenExpiration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(CustomClaimTypes.AMSSessionID, session.ToString()),
                    new Claim(ClaimTypes.Name, user.UserID.ToString()),
                    new Claim(CustomClaimTypes.LoginName, user.LoginName),
                    new Claim(CustomClaimTypes.StaffName, user.StaffName),
                    new Claim(CustomClaimTypes.StaffEmail, user.StaffEmail),
                    new Claim(CustomClaimTypes.CompanyID, user.CompanyID.ToString()),
                    new Claim(CustomClaimTypes.CompanyCode, user.CompanyCode),
                    new Claim(CustomClaimTypes.UserRoles, user.UserRolesID),
                    new Claim(CustomClaimTypes.UserRights, user.UserRightsID),
                }),
                Expires = DateTime.UtcNow.AddDays(tokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

    }

}
