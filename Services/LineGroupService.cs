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
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface ILineGroupService
    {
        IEnumerable<LineGroup> GetAll();
        LineGroup GetByCode(int Code);
        int Update(int code, LineGroupRequest lineGroup);
        LineGroup Create(LineGroupRequest lineGroup);
    }

    public class LineGroupService : ILineGroupService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public LineGroupService(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<LineGroup> GetAll()
        {
            return _context.LineGroups
                .OrderBy(lg => lg.Description)
                .ToList();
        }      

        public LineGroup GetByCode(int Code)
        {
            return _context.LineGroups
                .SingleOrDefault(x => x.Code == Code);
        }

        public int Update(int code, LineGroupRequest lineGroup)
        {
            var _lineGroup = this.GetByCode(code);
            _lineGroup.Description = lineGroup.Description;

            _context.Entry(_lineGroup).State = EntityState.Modified;
            return _context.SaveChanges();
        }

        public LineGroup Create(LineGroupRequest lineGroup)
        {
            LineGroup _lineGroup = new LineGroup();

            _lineGroup.Description = lineGroup.Description;

            _context.LineGroups.Add(_lineGroup);  
            _context.SaveChanges();

            return _lineGroup;            
        }
    }
}