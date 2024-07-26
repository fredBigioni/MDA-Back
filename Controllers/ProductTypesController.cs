using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;


namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductTypesController : ControllerBase
    {
        private IProductTypeService _productTypeService;

        public ProductTypesController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _productTypes = _productTypeService.GetAll();
            return Ok(_productTypes);
        }                     
    }
}
