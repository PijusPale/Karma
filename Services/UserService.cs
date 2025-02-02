using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Karma.Helpers;
using Karma.Models;
using Karma.Models.Authentication;
using Karma.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Karma.Services
{
    public interface IUserService
    {
        User Authenticate(UserCredentials request);
        IEnumerable<User> GetAll();
        User GetUserById(int id);
        IEnumerable<Listing> GetAllUserListingsByUserId(int userId);
        IEnumerable<Listing> GetAllRequestedListingsByUserId(int userId);
        void Update(User user);
        void Add(User user);
        void Delete(User user);
    }
    public class UserService : IUserService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;

        public UserService(IOptions<JwtSettings> jwtSettings, IUserRepository userRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
        }

        public User Authenticate(UserCredentials request)
        {
            var user = _userRepository.GetAll().SingleOrDefault(u => request.Password == u.Password && (request.Username == u.Username || request.Username == u.Email));
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
            return _userRepository.GetAll();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public IEnumerable<Listing> GetAllUserListingsByUserId(int userId)
        {
            return _userRepository.GetAllUserListingsByUserId(userId);
        }

        public IEnumerable<Listing> GetAllRequestedListingsByUserId(int userId)
        {
            return _userRepository.GetAllRequestedListingsByUserId(userId);
        }

        public void Update(User user)
        {
            _userRepository.UpdateAsync(user);
        }

        public void Add(User user)
        {
            _userRepository.Add(user);
        }
        public void Delete(User user)
        {
            _userRepository.DeleteById(user.Id);
        }

    }
}