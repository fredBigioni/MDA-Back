using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Models;
using System;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        private IUserService _userService;

        public ProductsController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var _products = _productService.GetAllProducts();
            return Ok(_products);
        } 

        [HttpGet("components")]
        public IActionResult GetAllProductComponents()
        {
            var _productComponents = _productService.GetAllProductComponents();
            return Ok(_productComponents);
        }

        [HttpGet("groups")]
        public IActionResult GetAllProductGroups()
        {
            var _productGroups = _productService.GetAllProductGroups();
            return Ok(_productGroups);
        }

        [HttpPost("group")]
        public IActionResult CreateProductGroup([FromBody] ProductGroupRequest productGroup)
        {
            var _productGroup = _productService.CreateProductGroup(productGroup);
            return Ok(_productGroup);
        }   

        [HttpGet("group/{code}")]
        public IActionResult GetProductGroupDetail(int code)
        {
            var _productGroup = _productService.GetProductGroupDetail(code);
            return Ok(_productGroup);
        }    

        [HttpPut("group/{code}")]
        public IActionResult UpdateProductGroup(int code, [FromBody] ProductGroupRequest productGroup)
        {
            try {
                var _productGroup = _productService.UpdateProductGroup(code, productGroup);
                return Ok(_productGroup);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }    

        [HttpDelete("group/{code}")]
        public IActionResult Delete(int code)
        {
            try
            {
                var _productGroup = _productService.DeleteProductGroup(code);
                return Ok(_productGroup);    
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }                 

        [HttpGet("by-laboratory")]
        public IActionResult GetAllProductByLaboratory([FromQuery]string laboratoryCodes, [FromQuery]string laboratoryGroupCodes)
        {
            int[] arrLaboratoryCodes = new int[0];
            int[] arrLaboratoryGroupCodes  = new int[0];

            if (laboratoryCodes == null && laboratoryGroupCodes == null) {
                return BadRequest("Al menos uno de los parametros laboratoryCodes,laboratoryGroupCodes deben estar seteados");
            }
            
            if (laboratoryCodes !=  null) {
                arrLaboratoryCodes = Array.ConvertAll(laboratoryCodes.Split(','), int.Parse);
            }

            if (laboratoryGroupCodes !=  null) {
                arrLaboratoryGroupCodes = Array.ConvertAll(laboratoryGroupCodes.Split(','), int.Parse);
            }

            var _productComponents = _productService.GetAllProductByLaboratory(arrLaboratoryCodes, arrLaboratoryGroupCodes);
            return Ok(_productComponents);
        }

        [HttpGet("by-drug")]
        public IActionResult GetAllProductByDrug([FromQuery]string drugCodes, [FromQuery]string drugGroupCodes)
        {
            int[] arrDrugCodes = new int[0];
            int[] arrDrugGroupCodes  = new int[0];

            if (drugCodes == null && drugGroupCodes == null) {
                return BadRequest("Al menos uno de los parametros drugCodes,drugGroupCodes deben estar seteados");
            }
            
            if (drugCodes !=  null) {
                arrDrugCodes = Array.ConvertAll(drugCodes.Split(','), int.Parse);
            }

            if (drugGroupCodes !=  null) {
                arrDrugGroupCodes = Array.ConvertAll(drugGroupCodes.Split(','), int.Parse);
            }

            var _productComponents = _productService.GetAllProductByDrug(arrDrugCodes, arrDrugGroupCodes);
            return Ok(_productComponents);
        }

        [HttpGet("by-therapeuticalClass")]
        public IActionResult GetAllProductByTherapeuticalClass([FromQuery]string therapeuticalClassCodes)
        {
            int[] arrTherapeuticalClassCodes = new int[0];

            if (therapeuticalClassCodes == null) {
                return BadRequest("El parametros therapeuticalClassCodes debe estar seteado");
            } else {
                arrTherapeuticalClassCodes = Array.ConvertAll(therapeuticalClassCodes.Split(','), int.Parse);
    
                var _productComponents = _productService.GetAllProductByTherapeuticalClass(arrTherapeuticalClassCodes);
                return Ok(_productComponents);
            }
        }

        [HttpPut("{code}")]
        public IActionResult Update(int code, [FromBody] ProductRequest product)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
            User user = _userService.GetById(userId); 

            var result = _productService.UpdateProduct(code, product, user);
            return Ok();
        }                                                   
    }
}
