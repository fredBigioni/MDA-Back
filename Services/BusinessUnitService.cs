using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface IBusinessUnitService
    {
        BusinessUnit GetByCode(int code);
        IEnumerable<BusinessUnit> GetAll();
        BusinessUnit Create(BusinessUnitRequest businessUnit);
        int Update(int code, BusinessUnitRequest businessUnit);
    }

    public class BusinessUnitService : IBusinessUnitService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public BusinessUnitService(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public BusinessUnit GetByCode(int code)
        {
            return _context.BusinessUnits     
                .SingleOrDefault(p => p.Code == code);            
        }

        public IEnumerable<BusinessUnit> GetAll()
        {
            return _context.BusinessUnits.ToList();
        }    

        public BusinessUnit Create(BusinessUnitRequest businessUnit)
        {
            BusinessUnit _businessUnit = new BusinessUnit();
            _businessUnit.Description = businessUnit.Description;
            
            _context.BusinessUnits.Add(_businessUnit);  
            _context.SaveChanges();

            return _businessUnit;            
        }

        public int Update(int code, BusinessUnitRequest businessUnit)
        {
            BusinessUnit _businessUnit = this.GetByCode(code);
            _businessUnit.Description = businessUnit.Description;
            _context.Entry(_businessUnit).State = EntityState.Modified;

            return _context.SaveChanges();
        }
    }
}