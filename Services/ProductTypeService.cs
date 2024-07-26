using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;


namespace WebApi.Services
{
    public interface IProductTypeService
    {
        IEnumerable<ProductType> GetAll();
    }

    public class ProductTypeService : IProductTypeService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public ProductTypeService(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<ProductType> GetAll()
        {
            return _context.ProductTypes.ToList();
        }      
    }
}