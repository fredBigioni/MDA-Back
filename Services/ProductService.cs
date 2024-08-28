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
    public interface IProductService
    {
        IEnumerable<Object> GetAllProducts();
        IEnumerable<Object> GetAllProductComponents();
        IEnumerable<ProductGroup> GetAllProductGroups();
        IEnumerable<Object> GetAllProductByLaboratory(int[] laboratoryCodesArray, int[] laboratoryGroupCodesArray);
        IEnumerable<Object> GetAllProductByDrug(int[] drugCodesArray, int[] drugGroupCodesArray);
        IEnumerable<Object> GetAllProductByTherapeuticalClass(int[] therapeuticalClassCodesArray);
        ProductComponent GetProductComponentsByCode(int? productCode);
        Product GetProductByCode(int productCode);
        ProductGroup CreateProductGroup(ProductGroupRequest productGroup);
        int UpdateProduct(int code, ProductRequest product, User user);
        IEnumerable<Object> GetProductGroupDetail(int productGroupCode);
        ProductGroup UpdateProductGroup(int productGroupCode, ProductGroupRequest productGroup);
        Object DeleteProductGroup(int productGroupCode);
    }

    public class ProductService : IProductService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;
        private readonly ILoggerService _loggerService;

        public ProductService(
            DataContext context,
            IOptions<AppSettings> appSettings,
            ILoggerService loggerService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _loggerService = loggerService;
        }

        public IEnumerable<Object> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.Laboratory)
                .Select(p => new {
                    Code = p.Code,
                    Description = p.Description,
                    LaboratoryCode = p.Laboratory.Code,
                    LaboratoryDescription = p.Laboratory.Description,
                    LaboratoryImsCode = p.Laboratory.Imscode
                    })
                .Distinct()
                .OrderBy(p => p.Description)
                .ToList();
        }

        public IEnumerable<Object> GetAllProductComponents()
        {
            return _context.ProductComponents
                .Select(pc => new {
                    ProductCode = pc.ProductCode,
                    ProductGroupCode = pc.ProductGroupCode,
                    Description = pc.Description,
                    Class = pc.Class,
                    Laboratory = pc.Laboratory
                    })            
                .Distinct()
                .OrderBy(p => p.Description)
                .ToList();
        }

        public IEnumerable<ProductGroup> GetAllProductGroups()
        {
            return _context.ProductGroups
                .OrderBy(p=> p.Description)
                .ToList();
        }

        public IEnumerable<Object> GetAllProductByLaboratory(int[] laboratoryCodesArray, int[] laboratoryGroupCodesArray)
        {
            return _context.ProductComponentByLaboratories
                .Where(pc => 
                    laboratoryCodesArray.Contains((int) pc.LaboratoryCode) || laboratoryGroupCodesArray.Contains((int) pc.LaboratoryGroupCode)
                )
                .Select(pc => new {
                    ProductCode = pc.ProductCode,
                    ProductGroupCode = pc.ProductGroupCode,
                    Description = pc.Description,
                    Class = pc.Class,
                    Laboratory = pc.Laboratory
                })                
                .Distinct()       
                .OrderBy(p => p.Description)     
                .ToList();
        }

        public IEnumerable<Object> GetAllProductByDrug(int[] drugCodesArray, int[] drugGroupCodesArray)
        {
            return _context.ProductComponentByDrugs
                .Where(pc => 
                    drugCodesArray.Contains((int) pc.DrugCode) || drugGroupCodesArray.Contains((int) pc.DrugGroupCode)
                )
                .Select(pc => new {
                    ProductCode = pc.ProductCode,
                    ProductGroupCode = pc.ProductGroupCode,
                    Description = pc.Description,
                    Class = pc.Class,
                    Laboratory = pc.Laboratory
                })                
                .Distinct() 
                .OrderBy(p => p.Description)           
                .ToList();
        }

        public IEnumerable<Object> GetAllProductByTherapeuticalClass(int[] therapeuticalClassCodesArray)
        {
            var tcIMSCodes = _context.TherapeuticalClasses
                .Where(tc => therapeuticalClassCodesArray.Contains((int) tc.Code))
                .Select(tc => tc.Imscode)                 
                .ToList();

            var tcCodes = _context.TherapeuticalClasses
                .AsEnumerable()
                .Where(tc => tcIMSCodes.Any(i => tc.Imscode.StartsWith(i)))
                .Select(tc => tc.Code)    
                .ToList();          


            return _context.ProductComponentByTherapeuticalClasses
                .Where(pc => tcCodes.Contains((int) pc.TherapeuticalClassCode))
                .Select(pc => new {
                    ProductCode = pc.ProductCode,
                    ProductGroupCode = pc.ProductGroupCode,
                    Description = pc.Description,
                    Class = pc.Class,
                    Laboratory = pc.Laboratory
                })                
                .Distinct()  
                .OrderBy(p => p.Description)          
                .ToList();
        }

        public Product GetProductByCode(int productCode)
        {
            return _context.Products      
                .SingleOrDefault(p => p.Code == productCode);
        }          

        public ProductComponent GetProductComponentsByCode(int? productCode)
        {
            return _context.ProductComponents      
                .SingleOrDefault(pc => pc.ProductCode == productCode);
        }             

        public ProductGroup CreateProductGroup(ProductGroupRequest productGroup)
        {
            List<ProductGroupDetail> _productGroupDetails = new List<ProductGroupDetail>();

            foreach (int productCode in productGroup.ProductCodes)
            {
                ProductGroupDetail productGroupDetail = new ProductGroupDetail();
                productGroupDetail.ProductCode = productCode;
                _productGroupDetails.Add(productGroupDetail);
            }

            ProductGroup _productGroup = new ProductGroup();
            _productGroup.Description = productGroup.Description;
            _productGroup.ProductGroupDetails = _productGroupDetails;

            _context.ProductGroups.Add(_productGroup);
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} creó el grupo de productos {_productGroup.Description}", UserLogged.userLogged.Usuario, _context, null);


            _context.SaveChanges();
            return _productGroup;
        }

        public int UpdateProduct(int code, ProductRequest product, User user)
        {
            Product _product = this.GetProductByCode(code);
            _product.LaboratoryCode = product.LaboratoryCode;
            _product.IsModified = true;
            _context.Entry(_product).State = EntityState.Modified;
            
            MasterEntityLog _log = new MasterEntityLog();
            _log.Entity = "product";
            _log.UserId = user.Id;
            _log.LogJson = JsonSerializer.Serialize(product);

            _context.MasterEntityLogs.Add(_log);

            return _context.SaveChanges();
        }     

        public ProductGroup UpdateProductGroup(int productGroupCode, ProductGroupRequest productGroup)
        {
            ProductGroup  _productGroup = _context.ProductGroups
                .SingleOrDefault(x => x.Code == productGroupCode);

            if (_productGroup == null) return null;

            List<ProductGroupDetail> pgds = _context.ProductGroupDetails.Where(d => d.ProductGroupCode == _productGroup.Code).ToList();
            if (pgds.Any()) {
                foreach (ProductGroupDetail pgd in pgds) 
                {
                    _context.ProductGroupDetails.Remove(pgd);
                }
            }

            List<ProductGroupDetail> _productGroupDetails = new List<ProductGroupDetail>();
            foreach (int productCode in productGroup.ProductCodes)
            {
                ProductGroupDetail productGroupDetail = new ProductGroupDetail();
                productGroupDetail.ProductCode = productCode;
                _productGroupDetails.Add(productGroupDetail);
            }

            _productGroup.Description = productGroup.Description;
            _productGroup.ProductGroupDetails = _productGroupDetails;

            _context.Entry(_productGroup).State = EntityState.Modified;
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} modificó el grupo de productos {_productGroup.Description}", UserLogged.userLogged.Usuario, _context, null);


            _context.SaveChanges();
            return _productGroup;
        }        

        public IEnumerable<Object> GetProductGroupDetail(int productGroupCode)
        {
            var productCodes =  _context.ProductGroupDetails
                .Include(pgd => pgd.Product)
                .Where(pgd => pgd.ProductGroupCode == productGroupCode)
                .Select(pgd => pgd.Product.Code)            
                .ToList();

            List<ProductComponent> pc = new List<ProductComponent>();
            foreach (var productCode in productCodes)
            {                
                pc.Add(this.GetProductComponentsByCode(productCode));
            }

            return pc;
        }

        public Object DeleteProductGroup(int productGroupCode)
        {
            ProductGroup  _productGroup = _context.ProductGroups
                .SingleOrDefault(x => x.Code == productGroupCode);

            if (_productGroup == null) return null;

            List<ProductGroupDetail> pgds = _context.ProductGroupDetails.Where(d => d.ProductGroupCode == _productGroup.Code).ToList();
            if (pgds.Any()) {
                foreach (ProductGroupDetail pgd in pgds) 
                {
                    _context.ProductGroupDetails.Remove(pgd);
                }
            }                
            _context.ProductGroups.Remove(_productGroup);
            _loggerService.LogMessage($"El usuario {UserLogged.userLogged.Usuario} eliminó el grupo de productos {_productGroup.Description}", UserLogged.userLogged.Usuario, _context, null);


            return _context.SaveChanges();   
        }
    }
}