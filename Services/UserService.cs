using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Entities;
using WebApi.Helpers;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(UserRequest user);
        int Update(int id, UserRequest user);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public UserService(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            using(PrincipalContext pc = new PrincipalContext(ContextType.Domain, _appSettings.Domain))
            {
                // validate the credentials
                bool isValid = pc.ValidateCredentials(model.Username, model.Password);

                if (isValid == false) return null;
                
                var user =  _context.Users.SingleOrDefault(x => x.Usuario == model.Username && x.Dominio == _appSettings.Domain);
                
                // return null if user not found
                if (user == null) return null;

                // authentication successful so generate jwt and refresh tokens
                var jwtToken = generateJwtToken(user);
                var refreshToken = generateRefreshToken(ipAddress);

                // save refresh token
                user.RefreshTokens.Add(refreshToken);
                _context.Update(user);
                _context.SaveChanges();

                return new AuthenticateResponse(user, jwtToken, refreshToken.Token);                    
            }
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new
            TransactionOptions { IsolationLevel= IsolationLevel.ReadUncommitted }))
            {
                var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
                
                // return null if no user found with token
                if (user == null) {
                    return null;
                }

                var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

                // return null if token is no longer active
                if (!refreshToken.IsActive) { 
                    return null;
                }

                // replace old refresh token with a new one and save
                var newRefreshToken = generateRefreshToken(ipAddress);
                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.RevokedByIp = ipAddress;
                refreshToken.ReplacedByToken = newRefreshToken.Token;
                user.RefreshTokens.Add(newRefreshToken);
                _context.Update(user);
                _context.SaveChanges();
                
                scope.Complete();

                // generate new jwt
                var jwtToken = generateJwtToken(user);

                return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
            }         
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            
            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .OrderBy(u => u.Usuario)
                .ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User Create(UserRequest user)
        {
            User _user = new User();
            _user.Usuario = user.Usuario;
            _user.Dominio = user.Dominio;
            _user.IsAdmin = user.IsAdmin;
            
            _context.Users.Add(_user);  
            _context.SaveChanges();

            return _user;            
        }   

        public int Update(int id, UserRequest user)
        {
            User _user = this.GetById(id);
            _user.IsAdmin = user.IsAdmin;
            _context.Entry(_user).State = EntityState.Modified;

            return _context.SaveChanges();
        }     

        // helper methods

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.JwtTokenExpiresInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using(var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(_appSettings.RefreshTokenExpiresInDays),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}