using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Views;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface ILaboratoryService
    {
        IEnumerable<Laboratory> GetAll();
        IEnumerable<LaboratoryComponent> GetAllLaboratoryComponents();
        LaboratoryGroup CreateLaboratoryGroup(LaboratoryGroupRequest laboratoryGroup);
        IEnumerable<LaboratoryGroup> GetAllLaboratoryGroup();
        LaboratoryComponent GetLaboratoryComponentByCode(int? code);
        IEnumerable<Object> GetLaboratoryGroupDetail(int laboratoryGroupCode);
        LaboratoryGroup UpdateLaboratoryGroup(int laboratoryGroupCode, LaboratoryGroupRequest laboratoryGroup);
        Object DeleteLaboratoryGroup(int laboratoryGroupCode);
    }

    public class LaboratoryService : ILaboratoryService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;
        private readonly ILoggerService _loggerService;

        public LaboratoryService(
            DataContext context,
            IOptions<AppSettings> appSettings,
            ILoggerService loggerService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _loggerService = loggerService;
        }

        public IEnumerable<Laboratory> GetAll()
        {
            return _context.Laboratories
                .OrderBy(l=> l.Description)
                .ToList();
        }

        public IEnumerable<LaboratoryGroup> GetAllLaboratoryGroup()
        {
            return _context.LaboratoryGroups
                .OrderBy(l=> l.Description)
                .ToList();
        }

        public IEnumerable<LaboratoryComponent> GetAllLaboratoryComponents()
        {
            return _context.LaboratoryComponents
                .OrderBy(l=> l.Description)
                .ToList();
        }

        public LaboratoryGroup CreateLaboratoryGroup(LaboratoryGroupRequest laboratoryGroup)
        {
            List<LaboratoryGroupDetail> _laboratoryGroupDetails = new List<LaboratoryGroupDetail>();

            foreach (int laboratoryCode in laboratoryGroup.LaboratoryCodes)
            {
                LaboratoryGroupDetail laboratoryGroupDetail = new LaboratoryGroupDetail();
                laboratoryGroupDetail.LaboratoryCode = laboratoryCode;
                _laboratoryGroupDetails.Add(laboratoryGroupDetail);
            }

            LaboratoryGroup _laboratoryGroup = new LaboratoryGroup();
            _laboratoryGroup.Description = laboratoryGroup.Description;
            _laboratoryGroup.Class = laboratoryGroup.Class;
            _laboratoryGroup.LaboratoryGroupDetails = _laboratoryGroupDetails;

            _context.LaboratoryGroups.Add(_laboratoryGroup);
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} creó el grupo de laboratorios {_laboratoryGroup.Description}", UserLogged.userLogged.Usuario, _context, null);

            _context.SaveChanges();
            return _laboratoryGroup;
        }

        public LaboratoryComponent GetLaboratoryComponentByCode(int? code)
        {
            return _context.LaboratoryComponents      
                .SingleOrDefault(lc => lc.LaboratoryCode == code);
        }

        public IEnumerable<Object> GetLaboratoryGroupDetail(int laboratoryGroupCode)
        {
            var laboratoryCodes =  _context.LaboratoryGroupDetails
                .Include(lgd => lgd.Laboratory)
                .Where(lgd => lgd.LaboratoryGroupCode == laboratoryGroupCode)
                .Select(lgd => lgd.Laboratory.Code)            
                .ToList();

            List<LaboratoryComponent> lc = new List<LaboratoryComponent>();
            foreach (var laboratoryCode in laboratoryCodes)
            {                
                lc.Add(this.GetLaboratoryComponentByCode(laboratoryCode));
            }

            return lc;
        }

        public LaboratoryGroup UpdateLaboratoryGroup(int laboratoryGroupCode, LaboratoryGroupRequest laboratoryGroup)
        {
            LaboratoryGroup  _laboratoryGroup = _context.LaboratoryGroups
                .SingleOrDefault(x => x.Code == laboratoryGroupCode);

            if (_laboratoryGroup == null) return null;

            List<LaboratoryGroupDetail> lgds = _context.LaboratoryGroupDetails.Where(d => d.LaboratoryGroupCode == _laboratoryGroup.Code).ToList();
            if (lgds.Any()) {
                foreach (LaboratoryGroupDetail lgd in lgds) 
                {
                    _context.LaboratoryGroupDetails.Remove(lgd);
                }
            }

            List<LaboratoryGroupDetail> _laboratoryGroupDetails = new List<LaboratoryGroupDetail>();
            foreach (int LaboratoryCode in laboratoryGroup.LaboratoryCodes)
            {
                LaboratoryGroupDetail laboratoryGroupDetail = new LaboratoryGroupDetail();
                laboratoryGroupDetail.LaboratoryCode = LaboratoryCode;
                _laboratoryGroupDetails.Add(laboratoryGroupDetail);
            }

            _laboratoryGroup.Description = laboratoryGroup.Description;
            _laboratoryGroup.Class = laboratoryGroup.Class;
            _laboratoryGroup.LaboratoryGroupDetails = _laboratoryGroupDetails;

            _context.Entry(_laboratoryGroup).State = EntityState.Modified;
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} modificó el grupo de laboratorios {_laboratoryGroup.Description}", UserLogged.userLogged.Usuario, _context, null);

            _context.SaveChanges();
            return _laboratoryGroup;
        }

        public Object DeleteLaboratoryGroup(int laboratoryGroupCode)
        {
            LaboratoryGroup  _laboratoryGroup = _context.LaboratoryGroups
                .SingleOrDefault(x => x.Code == laboratoryGroupCode);

            if (_laboratoryGroup == null) return null;

            List<LaboratoryGroupDetail> lgds = _context.LaboratoryGroupDetails.Where(d => d.LaboratoryGroupCode == _laboratoryGroup.Code).ToList();
            if (lgds.Any()) {
                foreach (LaboratoryGroupDetail lgd in lgds) 
                {
                    _context.LaboratoryGroupDetails.Remove(lgd);
                }
            }

            _context.LaboratoryGroups.Remove(_laboratoryGroup);
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} eliminó el grupo de laboratorios {_laboratoryGroup.Description}", UserLogged.userLogged.Usuario, _context, null);

            return _context.SaveChanges();                           
        }                  
    }
}