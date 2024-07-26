using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Helpers;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface ICustomMarketGroupService
    {
        IEnumerable<CustomMarketGroup> GetAll();
        IEnumerable<CustomMarketGroup> GetAllByCustomMarket(int customMarketCode);
        CustomMarketGroup GetByCode(int Code);
        int Update(int code, CustomMarketGroupRequest customMarketGroup);
        CustomMarketGroup Create(CustomMarketGroupRequest customMarketGroup);
        Object Delete(int Code);
    }

    public class CustomMarketGroupService : ICustomMarketGroupService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public CustomMarketGroupService(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<CustomMarketGroup> GetAll()
        {
            return _context.CustomMarketGroups
                .Include(customMarketGroups => customMarketGroups.CustomMarket)
                .ToList();
        }

        public IEnumerable<CustomMarketGroup> GetAllByCustomMarket(int customMarketCode)
        {
            return _context.CustomMarketGroups
                .OrderBy(cmg => cmg.Description)
                .Where(cmg => cmg.CustomMarketCode == customMarketCode)
                .OrderBy(cm => cm.Description)
                .ToList();
        }

        public CustomMarketGroup GetByCode(int Code)
        {
            return _context.CustomMarketGroups
                .SingleOrDefault(x => x.Code == Code);
        }

        public int Update(int code, CustomMarketGroupRequest customMarketGroup)
        {
            var _customMarketGroup = this.GetByCode(code);
            _customMarketGroup.Description = customMarketGroup.Description;
            _customMarketGroup.GroupCondition = customMarketGroup.GroupCondition;

            _context.Entry(_customMarketGroup).State = EntityState.Modified;
            return _context.SaveChanges();
        }

        public CustomMarketGroup Create(CustomMarketGroupRequest customMarketGroup)
        {
            CustomMarketGroup _customMarketGroup = new CustomMarketGroup();

            _customMarketGroup.CustomMarketCode = customMarketGroup.CustomMarketCode;
            _customMarketGroup.Description = customMarketGroup.Description;
            _customMarketGroup.GroupCondition = customMarketGroup.GroupCondition;

            _context.CustomMarketGroups.Add(_customMarketGroup);  
            _context.SaveChanges();

            return _customMarketGroup;
        }

        public Object Delete(int code)
        {
            var _customMarketGroup = this.GetByCode(code);

            if (_customMarketGroup == null) return null;

            List<CustomMarketDetail> cmds = _context.CustomMarketDetails.Where(d => d.CustomMarketGroupCode == _customMarketGroup.Code).ToList();
            if (cmds.Any()) {
                throw new Exception("has_custom_market_detail");                                
            }

            _context.CustomMarketGroups.Remove(_customMarketGroup);
            return _context.SaveChanges();   
        }
       
    }
}