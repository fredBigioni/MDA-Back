using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Views;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Numerics;

namespace WebApi.Services
{
    public interface IProductPresentationService
    {
        IEnumerable<Object> GetAllProductPresentations();
        IEnumerable<ProductPresentationGroup> GetAllProductPresentationGroups();
        IEnumerable<Object> GetAllProductPresentationComponents();
        IEnumerable<Object> GetAllProductPresentationComponentsByPharmaceuticalForm(int[] pharmaceuticalFormCodesArray, int[] productCodesArray, int[] productGroupCodesArray);
        ProductPresentationComponent GetProductPresentationComponentByCode(int? code);
        ProductPresentationGroup CreateProductPresentationGroup(ProductPresentationGroupRequest productPresentationGroup);
        ProductPresentation GetProductPresentationByCode(int code);
        int UpdateProductPresentation(int code, ProductPresentationRequest productPresentation, User user);
        IEnumerable<Object> GetProductPresentationGroupDetail(int productPresentationGroupCode);
        ProductPresentationGroup UpdateProductPresentationGroup(int productPresentationGroupCode, ProductPresentationGroupRequest productPresentationGroup);
        Object DeleteProductPresentationGroup(int productPresentationGroupCode);
    }

    public class ProductPresentationService : IProductPresentationService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;
        private readonly ILoggerService _loggerService;

        public ProductPresentationService(
            DataContext context,
            IOptions<AppSettings> appSettings,
            ILoggerService loggerService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _loggerService = loggerService;
        }

        public IEnumerable<Object> GetAllProductPresentations()
        {
            return _context.ProductPresentations
                .Include(productPresentations => productPresentations.Class)
                .Include(productPresentations => productPresentations.BusinessUnit)
                .Include(productPresentations => productPresentations.TherapeuticalClass)
                .Select(pp => new {
                    Code = pp.Code,
                    Description = pp.Description,
                    ClassCode = (int?) pp.Class.Code,
                    ClassDescription = pp.Class.Description,
                    ClassImsCode = pp.Class.Imscode,
                    TherapeuticalClassCode = (int?) pp.TherapeuticalClass.Code,
                    TherapeuticalClassDescription = pp.TherapeuticalClass.Description,
                    BusinessUnitCode = (int?) pp.BusinessUnit.Code,
                    BusinessUnitDescription = pp.BusinessUnit.Description
                    })
                .Distinct()
                .OrderBy(pp => pp.Description)                
                .ToList();
        }

        public IEnumerable<ProductPresentationGroup> GetAllProductPresentationGroups()
        {
            return _context.ProductPresentationGroups
                .OrderBy(p=> p.Description)
                .ToList();
        }               

        public IEnumerable<Object> GetAllProductPresentationComponents()
        {
            return _context.ProductPresentationComponents
                .Select(ppc => new {
                    ProductPresentationGroupCode = ppc.ProductPresentationGroupCode,
                    Code = ppc.Code,
                    Description = ppc.Description,
                    Class = ppc.Class,
                    Laboratory = ppc.Laboratory,
                    TherapeuticalClass = ppc.TherapeuticalClass
                })
                .Distinct()
                .OrderBy(ppc => ppc.Description)
                .ToList();
        }

        public IEnumerable<Object> GetAllProductPresentationComponentsByPharmaceuticalForm(int[] pharmaceuticalFormCodesArray, int[] productCodesArray, int[] productGroupCodesArray)
        {
            var productPresentationsCodesArray = this.GetProductPresentationCodeByProduct(productCodesArray, productGroupCodesArray);

            List<int> productPresentationCodes = _context.ProductPresentationComponents
                .Where(pfc => 
                    productPresentationsCodesArray.Contains((int) pfc.Code) &&
                    pharmaceuticalFormCodesArray.Contains((int) pfc.PharmaceuticalFormCode)
                )
                .Select(ppc => (int) ppc.Code) 
                .Distinct()
                .ToList();   

            List<int> productPresentationGroupCodes = _context.ProductPresentationGroupDetails
                .Where(ppgd => productPresentationCodes.Contains((int) ppgd.ProductPresentationCode))
                .Select(ppgd => (int) ppgd.ProductPresentationGroupCode)  
                .Distinct()            
                .ToList();                             

            return _context.ProductPresentationComponents
                .Where(pfc => 
                    productPresentationCodes.Contains((int) pfc.Code) ||
                    productPresentationGroupCodes.Contains((int) pfc.ProductPresentationGroupCode)
                )
                .Select(ppc => new {
                    ProductPresentationGroupCode = ppc.ProductPresentationGroupCode,
                    Code = ppc.Code,
                    Description = ppc.Description,
                    Class = ppc.Class,
                    Laboratory = ppc.Laboratory,
                    TherapeuticalClass = ppc.TherapeuticalClass
                }) 
                .Distinct()  
                .OrderBy(ppc => ppc.Description)             
                .ToList();                             
        }

        public ProductPresentationComponent GetProductPresentationComponentByCode(int? code)
        {
            return _context.ProductPresentationComponents      
                .SingleOrDefault(pc => pc.Code == code);
        }

        public ProductPresentationGroup CreateProductPresentationGroup(ProductPresentationGroupRequest productPresentationGroup)
        {
            List<ProductPresentationGroupDetail> _productPresentationGroupDetails = new List<ProductPresentationGroupDetail>();

            foreach (int productPresentationCode in productPresentationGroup.ProductPresentationCodes)
            {
                ProductPresentationGroupDetail productPresentationGroupDetail = new ProductPresentationGroupDetail();
                productPresentationGroupDetail.ProductPresentationCode = productPresentationCode;
                _productPresentationGroupDetails.Add(productPresentationGroupDetail);
            }

            ProductPresentationGroup _productPresentationGroup = new ProductPresentationGroup();
            _productPresentationGroup.Description = productPresentationGroup.Description;
            _productPresentationGroup.ExpandGroup = productPresentationGroup.ExpandGroup;
            _productPresentationGroup.ProductPresentationGroupDetails = _productPresentationGroupDetails;

            _context.ProductPresentationGroups.Add(_productPresentationGroup);
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} creó el grupo de presentaciones {_productPresentationGroup.Description}", UserLogged.userLogged.Usuario, _context, null);


            _context.SaveChanges();
            return _productPresentationGroup;
        }      

        public ProductPresentation GetProductPresentationByCode(int code)
        {
            return _context.ProductPresentations     
                .SingleOrDefault(p => p.Code == code);
        }

        public int UpdateProductPresentation(int code, ProductPresentationRequest productPresentation, User user)
        {
            ProductPresentation _productPresentation = this.GetProductPresentationByCode(code);
            _productPresentation.Classcode = productPresentation.ClassCode;
            _productPresentation.TherapeuticalClassCode = productPresentation.TherapeuticalClassCode;
            _productPresentation.BusinessUnitCode = productPresentation.BusinessUnitCode;
            _productPresentation.IsModified = true;
            _context.Entry(_productPresentation).State = EntityState.Modified;
            
            MasterEntityLog _log = new MasterEntityLog();
            _log.Entity = "productPresentation";
            _log.UserId = user.Id;
            _log.LogJson = JsonSerializer.Serialize(productPresentation);

            _context.MasterEntityLogs.Add(_log);

            return _context.SaveChanges();
        }

        public ProductPresentationGroup UpdateProductPresentationGroup(int productPresentationGroupCode, ProductPresentationGroupRequest productPresentationGroup)
        {
            ProductPresentationGroup  _productPresentationGroup = _context.ProductPresentationGroups
                .SingleOrDefault(x => x.Code == productPresentationGroupCode);

            if (_productPresentationGroup == null) return null;

            List<ProductPresentationGroupDetail> pgds = _context.ProductPresentationGroupDetails.Where(d => d.ProductPresentationGroupCode == _productPresentationGroup.Code).ToList();
            if (pgds.Any()) {
                foreach (ProductPresentationGroupDetail pgd in pgds) 
                {
                    _context.ProductPresentationGroupDetails.Remove(pgd);
                }
            }

            List<ProductPresentationGroupDetail> _productPresentationGroupDetails = new List<ProductPresentationGroupDetail>();
            foreach (int productPresentationCode in productPresentationGroup.ProductPresentationCodes)
            {
                ProductPresentationGroupDetail productPresentationGroupDetail = new ProductPresentationGroupDetail();
                productPresentationGroupDetail.ProductPresentationCode = productPresentationCode;
                _productPresentationGroupDetails.Add(productPresentationGroupDetail);
            }

            _productPresentationGroup.Description = productPresentationGroup.Description;
            _productPresentationGroup.ProductPresentationGroupDetails = _productPresentationGroupDetails;

            _context.Entry(_productPresentationGroup).State = EntityState.Modified;
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} modificó el grupo de presentaciones {_productPresentationGroup.Description}", UserLogged.userLogged.Usuario, _context, null);

            _context.SaveChanges();
            return _productPresentationGroup;
        }                

        public IEnumerable<Object> GetProductPresentationGroupDetail(int productPresentationGroupCode)
        {
            var productPresentationCodes =  _context.ProductPresentationGroupDetails
                .Include(pgd => pgd.ProductPresentation)
                .Where(pgd => pgd.ProductPresentationGroupCode == productPresentationGroupCode)
                .Select(pgd => pgd.ProductPresentation.Code)            
                .ToList();

            List<ProductPresentationComponent> pc = new List<ProductPresentationComponent>();
            foreach (var productPresentationCode in productPresentationCodes)
            {                
                pc.Add(this.GetProductPresentationComponentByCode(productPresentationCode));
            }

            return pc;
        } 

        public Object DeleteProductPresentationGroup(int productPresentationGroupCode)
        {
            ProductPresentationGroup  _productPresentationGroup = _context.ProductPresentationGroups
                .SingleOrDefault(x => x.Code == productPresentationGroupCode);

            if (_productPresentationGroup == null) return null;

            List<ProductPresentationGroupDetail> pgds = _context.ProductPresentationGroupDetails.Where(d => d.ProductPresentationGroupCode == _productPresentationGroup.Code).ToList();
            if (pgds.Any()) {
                foreach (ProductPresentationGroupDetail pgd in pgds) 
                {
                    _context.ProductPresentationGroupDetails.Remove(pgd);
                }
            }

            _context.ProductPresentationGroups.Remove(_productPresentationGroup);
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} eliminó el grupo de presentaciones {_productPresentationGroup.Description}", UserLogged.userLogged.Usuario, _context, null);


            return _context.SaveChanges();   
        }

        private IEnumerable<int> GetProductPresentationCodeByProduct(int[] productCodesArray, int[] productGroupCodesArray)
        {
            List<int> productPresentationsCodesArray = new List<int>();

            if (productCodesArray.Any()) {
                productPresentationsCodesArray = _context.ProductPresentations
                    .Where(pp => productCodesArray.Contains((int) pp.ProductCode))
                    .Select(pp => (int) pp.Code)
                    .Distinct()               
                    .ToList();
            }

            if (productGroupCodesArray.Any()) {
                List<int> productGroupDetail = _context.ProductGroupDetails
                    .Where(pgd => productGroupCodesArray.Contains((int) pgd.ProductGroupCode))
                    .Select(pgd => pgd.ProductCode)            
                    .ToList();                

                List<int> arrProductPresentations = _context.ProductPresentations
                    .Where(pp => productGroupDetail.Contains((int) pp.ProductCode))
                    .Select(pp => (int) pp.Code)
                    .Distinct()               
                    .ToList();
    
                foreach (int productPresentationCode in arrProductPresentations)
                {
                    if (!productPresentationsCodesArray.Contains(productPresentationCode)) {
                        productPresentationsCodesArray.Add(productPresentationCode);
                    }
                }
            }

            return productPresentationsCodesArray;   
        }
                         
    }
}