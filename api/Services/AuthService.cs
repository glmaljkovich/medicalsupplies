using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ArqNetCore.DTOs.Auth;
using ArqNetCore.Exceptions.Auth;

namespace ArqNetCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<UserService> _logger;

        private readonly AppSettings _appSettings;

        public AuthService(
            ILogger<UserService> logger,
            IOptions<AppSettings> appSettings
        )
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public void Verify(AuthVerifyDTO authTokenDTO){
            if (authTokenDTO.ValueRaw == null)
            {
                throw new ArgumentNullException("password");
            } 
            if (string.IsNullOrWhiteSpace(authTokenDTO.ValueRaw))
            {
                 throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            } 
            if (authTokenDTO.ValueHash.Length != 64) 
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            } 
            if (authTokenDTO.ValueSalt.Length != 128) 
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            } 
            
            using (var hmac = new HMACSHA512(authTokenDTO.ValueSalt))
            {
                byte[] chunk = Encoding.UTF8.GetBytes(authTokenDTO.ValueRaw);
                var computedHash = hmac.ComputeHash(chunk);
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != authTokenDTO.ValueHash[i]) {
                        throw new AuthVerifyFailException();
                    };
                }
            }
        }

        public AuthTokenResultDTO AuthToken(AuthTokenDTO authTokenDTO)
        {   
            _logger.LogInformation("AuthToken generating token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(_appSettings.Secret);
            IDictionary<string, string> SubjectRaw = authTokenDTO.SubjectRaw;
            var claims = new List<Claim>();
            foreach(var item in SubjectRaw)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }

            SecurityKey key = new SymmetricSecurityKey(secret);
            string algorithm = SecurityAlgorithms.HmacSha256Signature;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(key, algorithm),
                Claims = authTokenDTO.Claims
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return new AuthTokenResultDTO{
                Token = tokenString
            };
        }
        public AuthHashResultDTO Hash(string valueRaw)
        {
            if (valueRaw == null)
            {
                throw new ArgumentNullException("password");
            } 
            if (string.IsNullOrWhiteSpace(valueRaw))
            {
                 throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            } 
            byte[] valueHash = null;
            byte[] valueSalt = null;
            using (var hmac = new HMACSHA512())
            {
                valueSalt = hmac.Key;
                byte[] encoded = Encoding.UTF8.GetBytes(valueRaw);
                valueHash = hmac.ComputeHash(encoded);
            }
            return new AuthHashResultDTO
            {
                ValueHash = valueHash,
                ValueSalt = valueSalt
            };
        }

    }
}