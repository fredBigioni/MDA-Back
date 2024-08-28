using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Views;
using WebApi.Models;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace WebApi.Services
{
    public interface IDrugService
    {
        IEnumerable<Drug> GetAll();
        DrugComponent GetDrugComponentsByCode(int? drugCode);
        IEnumerable<DrugComponent> GetAllDrugComponents();
        Drug GetByCode(int code);
        int Update(int code, Drug drug);
        IEnumerable<DrugGroup> GetAllDrugGroups();
        DrugGroup CreateDrugGroup(DrugGroupRequest drugGroup);
        DrugGroup UpdateDrugGroup(int drugGroupCode, DrugGroupRequest drugGroup);
        IEnumerable<Object> GetDrugGroupDetail(int drugGroupCode);
        Object DeleteDrugGroup(int drugGroupCode);
    }

    public class DrugService : IDrugService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;
        private readonly ILoggerService _loggerService;

        public DrugService(
            DataContext context,
            IOptions<AppSettings> appSettings,
            ILoggerService loggerService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _loggerService= loggerService;
        }

        public IEnumerable<Drug> GetAll()
        {
            return _context.Drugs
                .OrderBy(d => d.Description)             
                .ToList();
        }

        public IEnumerable<DrugGroup> GetAllDrugGroups()
        {
            return _context.DrugGroups
                .OrderBy(d => d.Description)             
                .ToList();
        }        

        public DrugComponent GetDrugComponentsByCode(int? drugCode)
        {
            return _context.DrugComponents      
                .SingleOrDefault(dc => dc.DrugCode == drugCode);
        }             

        public IEnumerable<DrugComponent> GetAllDrugComponents()
        {
            return _context.DrugComponents
                .OrderBy(d => d.Description)
                .ToList();
        }

        public Drug GetByCode(int code)
        {
            return _context.Drugs.Find(code);
        }

        public int Update(int code, Drug drug)
        {
            _context.Update(drug);
            return _context.SaveChanges();
        }

        public DrugGroup CreateDrugGroup(DrugGroupRequest drugGroup)
        {
            List<DrugGroupDetail> _drugGroupDetails = new List<DrugGroupDetail>();

            foreach (int drugCode in drugGroup.DrugCodes)
            {
                DrugGroupDetail drugGroupDetail = new DrugGroupDetail();
                drugGroupDetail.DrugCode = drugCode;
                _drugGroupDetails.Add(drugGroupDetail);
            }

            DrugGroup _drugGroup = new DrugGroup();
            _drugGroup.Description = drugGroup.Description;
            _drugGroup.DrugGroupDetails = _drugGroupDetails;

            _context.DrugGroups.Add(_drugGroup);
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} creo el grupo de drogas {_drugGroup.Description}", UserLogged.userLogged.Usuario, _context, null);
           
            _context.SaveChanges();
            return _drugGroup;
        }

        public DrugGroup UpdateDrugGroup(int drugGroupCode, DrugGroupRequest drugGroup)
        {
            DrugGroup  _drugGroup = _context.DrugGroups
                .SingleOrDefault(x => x.Code == drugGroupCode);

            if (_drugGroup == null) return null;

            List<DrugGroupDetail> dgds = _context.DrugGroupDetails.Where(d => d.DrugGroupCode == _drugGroup.Code).ToList();
            if (dgds.Any()) {
                foreach (DrugGroupDetail dgd in dgds) 
                {
                    _context.DrugGroupDetails.Remove(dgd);
                }
            }

            List<DrugGroupDetail> _drugGroupDetails = new List<DrugGroupDetail>();
            foreach (int drugCode in drugGroup.DrugCodes)
            {
                DrugGroupDetail drugGroupDetail = new DrugGroupDetail();
                drugGroupDetail.DrugCode = drugCode;
                _drugGroupDetails.Add(drugGroupDetail);
            }

            _drugGroup.Description = drugGroup.Description;
            _drugGroup.DrugGroupDetails = _drugGroupDetails;

            _context.Entry(_drugGroup).State = EntityState.Modified;
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} modificó el grupo de drogas {_drugGroup.Description}", UserLogged.userLogged.Usuario, _context, null);


            _context.SaveChanges();
            return _drugGroup;
        }  

        public IEnumerable<Object> GetDrugGroupDetail(int drugGroupCode)
        {
            var drugCodes =  _context.DrugGroupDetails
                .Include(pgd => pgd.Drug)
                .Where(pgd => pgd.DrugGroupCode == drugGroupCode)
                .Select(pgd => pgd.Drug.Code)            
                .ToList();

            List<DrugComponent> dc = new List<DrugComponent>();
            foreach (var drugCode in drugCodes)
            {                
                dc.Add(this.GetDrugComponentsByCode(drugCode));
            }

            return dc;
        }

        public Object DeleteDrugGroup(int drugGroupCode)
        {
            DrugGroup  _drugGroup = _context.DrugGroups
                .SingleOrDefault(x => x.Code == drugGroupCode);

            if (_drugGroup == null) return null;

            List<DrugGroupDetail> dgds = _context.DrugGroupDetails.Where(d => d.DrugGroupCode == _drugGroup.Code).ToList();
            if (dgds.Any()) {
                foreach (DrugGroupDetail dgd in dgds) 
                {
                    _context.DrugGroupDetails.Remove(dgd);
                }
            }

            _context.DrugGroups.Remove(_drugGroup);
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} eliminó el grupo de drogas {_drugGroup.Description}", UserLogged.userLogged.Usuario, _context, null);

            return _context.SaveChanges();   
        }                             
    }
}