using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface ITherapeuticalClassService
    {
        IEnumerable<TherapeuticalClass> GetAll();

        IEnumerable<Object> GetAllTherapeuticalClassesComponent();
    }

    public class TherapeuticalClassService : ITherapeuticalClassService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public TherapeuticalClassService(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<TherapeuticalClass> GetAll()
        {
            return _context.TherapeuticalClasses
                .OrderBy(tc=> tc.Imscode)
                .ToList();
        }

        public IEnumerable<Object> GetAllTherapeuticalClassesComponent()
        {
            return _context.TherapeuticalClasses
                .Select(tc => new {
                    //Class = tc.Class.Description,
                    Code = tc.Code,
                    Imscode = tc.Imscode,
                    Description = tc.Description,
                })
                .OrderBy(tc=> tc.Imscode)
                .ToList();
        }        
    }
}