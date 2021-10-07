using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Karma.Helpers;
using Karma.Models;
using Karma.Models.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Karma.Services
{
    public interface IUserService
    {
        User Authenticate(UserCredentials request);
        IEnumerable<User> GetAll();
    }
    public class UserService : IUserService
    {
        private List<User> _users = new List<User> {
            new User { Username = "First", Id = 1, FirstName = "First", LastName = "Test"},
            new User { Username = "Second", Id = 2, FirstName = "Second", LastName = "Test"}
        };
        private readonly JwtSettings _jwtSettings;

        public UserService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public User Authenticate(UserCredentials request)
        {
            var user = _users.SingleOrDefault(u => request.Username == u.Username);
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }
    }
}