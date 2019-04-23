using System;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication2.Services
{
    public class Login
    {
        public string BuildJWTToken(string username, string password)
        {
            Higher higher = new Higher();
            var user = higher.ReadUser(username);
            if (user == null) return null;
            if (user.password != password) return null;
            var header = new JwtHeader(Settings.GetCredentials());
            var payload = new JwtPayload
            {
                {"UserId",user.id },
                { "exp", ((DateTimeOffset)DateTime.UtcNow).AddMinutes(20).ToUnixTimeMilliseconds() }
            };
            var securityToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(securityToken);
            return tokenString;
        }

        public int? CheckJWTToken(string tokenstring)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenstring);
            if (token.Payload.Exp < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds()) return null;
            if(token.Payload.TryGetValue("UserId", out object value))
            {
                return int.Parse(value.ToString());
            }
            return null;
            //return token.Payload.TryGetValue("UserId", out object value) ? (int?)value : null;
        }
    }
}