using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IUserPermissionService
    {
        UserPermissionResponse GetAllByUser(User user);
        Object Update(User user, UserPermissionResponse permission);
    }

    public class UserPermissionService : IUserPermissionService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public UserPermissionService(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
   
        public UserPermissionResponse GetAllByUser(User user)
        {
            List<int?> lineGroupCodes = new List<int?>();
            List<int?> lineCodes = new List<int?>();
            List<int?> customMarketCodes = new List<int?>();
            bool fullAccess = false;
      
            var userPermissions =  _context.UserPermissions
                .Where(up => up.UserId == user.Id)
                .ToList();

            if (userPermissions.Any()) 
            {
                foreach (UserPermission userPermission in userPermissions) 
                {
                    if (userPermission.LineGroupCode != null) {
                        lineGroupCodes.Add(userPermission.LineGroupCode);
                    }
                    
                    if (userPermission.LineCode != null) {
                        lineCodes.Add(userPermission.LineCode);
                    }

                    if (userPermission.CustomMarketCode != null) {
                        customMarketCodes.Add(userPermission.CustomMarketCode);
                    }

                    if (userPermission.FullAccess == true) {
                        fullAccess = true;
                    }
                }
            }

            UserPermissionResponse _userPermission =  new UserPermissionResponse {
                LineGroupCodes = lineGroupCodes,
                LineCodes = lineCodes,
                CustomMarketCodes = customMarketCodes,
                FullAccess = fullAccess
            };

            return _userPermission;

        }

        public Object Update(User user, UserPermissionResponse permission)
        {
            var  _userPermissions = _context.UserPermissions
                .Where(up => up.UserId == user.Id);
            
            foreach (UserPermission up in _userPermissions) {
                _context.UserPermissions.Remove(up);
            }            

            if (permission.FullAccess == true) {
                UserPermission up = new UserPermission {
                    UserId = user.Id,
                    FullAccess = true
                };
                _context.UserPermissions.Add(up);
            } else {
                if (permission.CustomMarketCodes.Any()) {
                    foreach (int customMarketCode in permission.CustomMarketCodes) {
                        UserPermission up = new UserPermission {
                            UserId = user.Id,
                            CustomMarketCode = customMarketCode
                        };
                        _context.UserPermissions.Add(up);
                    } 
                }

                if (permission.LineCodes.Any()) {
                    foreach (int lineCode in permission.LineCodes) {
                        UserPermission up = new UserPermission {
                            UserId = user.Id,
                            LineCode = lineCode
                        };
                        _context.UserPermissions.Add(up);
                    } 
                }

                if (permission.LineGroupCodes.Any()) {
                    foreach (int lineGroupCode in permission.LineGroupCodes) {
                        UserPermission up = new UserPermission {
                            UserId = user.Id,
                            LineGroupCode = lineGroupCode
                        };
                        _context.UserPermissions.Add(up);
                    } 
                }                                
            }

           return  _context.SaveChanges();
        }

    }
}