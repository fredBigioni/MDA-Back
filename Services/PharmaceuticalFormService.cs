using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Views;
using WebApi.Helpers;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface IPharmaceuticalFormService
    {
        IEnumerable<PharmaceuticalForm> GetAll();
        IEnumerable<Object> GetAllByProduct(int[] productCodesArray, int[] productGroupCodesArray);
        IEnumerable<Object> GetAllByProductPresentation(int[] productPresentationCodesArray, int[] productPresentationGroupCodesArray);
    }

    public class PharmaceuticalFormService : IPharmaceuticalFormService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public PharmaceuticalFormService(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<PharmaceuticalForm> GetAll()
        {
            return _context.PharmaceuticalForms
                .OrderBy(pf => pf.Imscode)
                .ToList();
        }

        public IEnumerable<Object> GetAllByProduct(int[] productCodesArray, int[] productGroupCodesArray)
        {
            return _context.PharmaceuticalFormComponents
                .Where(pfc => 
                    productCodesArray.Contains((int) pfc.ProductCode) || productGroupCodesArray.Contains((int) pfc.ProductGroupCode)
                )
                .Select(pfc => new {
                    Code = pfc.Code,
                    Description = pfc.Description,
                    Imscode = pfc.Imscode
                })                   
                .Distinct()      
                .OrderBy(pf => pf.Imscode)          
                .ToList();

        }

        public IEnumerable<Object> GetAllByProductPresentation(int[] productPresentationCodesArray, int[] productPresentationGroupCodesArray)
        {
            return _context.PharmaceuticalFormComponents
                .Where(pfc => 
                    productPresentationCodesArray.Contains((int) pfc.ProductPresentationCode) || productPresentationGroupCodesArray.Contains((int) pfc.ProductPresentationGroupCode)
                )
                .Select(pfc => new {
                    Code = pfc.Code,
                    Description = pfc.Description,
                    Imscode = pfc.Imscode
                })                
                .Distinct()    
                .OrderBy(pf => pf.Imscode)                    
                .ToList();
        }                
    }
}