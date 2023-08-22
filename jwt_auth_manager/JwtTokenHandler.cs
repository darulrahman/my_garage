using jwt_auth_manager.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace jwt_auth_manager
{
    public class JwtTokenHandler
    {
        public const string securityKey = "Ini merupakan security Keys ya gaes";
        private const int tokenValidity = 5;
        private readonly List<UserAccount> validUser;

        public JwtTokenHandler()
        {
            validUser = new List<UserAccount>
            {
                    new UserAccount { Username = "admin", DisplayName = "Admin Suradmin", Password = "admin", Role = "Administrator"},
                    new UserAccount { Username = "user", DisplayName = "User Sureser", Password = "user", Role = "User"}
            };
        }

        public AuthRes? GenerateToken(AuthReq req)
        {
            if (string.IsNullOrWhiteSpace(req.Username)
                || string.IsNullOrWhiteSpace(req.Password))
                return null;

            UserAccount user = validUser.Where(x => x.Username == req.Username && x.Password == req.Password).FirstOrDefault();

            if (user == null) return null;

            var tokenExpireyTime = DateTime.Now.AddMinutes(tokenValidity);
            var tokenKey = Encoding.ASCII.GetBytes(securityKey);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name, req.Username),
                    new Claim("Role", user.Role)
                });

            var signingCredential = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature
                );

            var secTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpireyTime,
                SigningCredentials = signingCredential
            };

            var jwtSecTokenHandler = new JwtSecurityTokenHandler();
            var secToken = jwtSecTokenHandler.CreateToken(secTokenDescriptor);

            var token = jwtSecTokenHandler.WriteToken(secToken);

            return new AuthRes
            {
                Username = user.Username,
                Role = user.Role,
                Expires = (int)tokenExpireyTime.Subtract(DateTime.Now).TotalSeconds,
                AccessToken = token
            };

        }
    }
}
