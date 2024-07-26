using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Models;
using WebApi.Entities;
using System.Security.Claims;
using System;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductPresentationsController : ControllerBase
    {
        private IProductPresentationService _productPresentationService;
        private IUserService _userService;

        public ProductPresentationsController(IProductPresentationService productPresentationService, IUserService userService)
        {
            _productPresentationService = productPresentationService;
            _userService = userService;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var _productPresentations = _productPresentationService.GetAllProductPresentations();
            return Ok(_productPresentations);
        }

        [HttpGet("components")]
        public IActionResult GetAllProductPresentationComponents()
        {
            var _productPresentationComponents = _productPresentationService.GetAllProductPresentationComponents();
            return Ok(_productPresentationComponents);
        }

        [HttpGet("groups")]
        public IActionResult GetAllProductPresentationGroups()
        {
            var _productPresentationGroups = _productPresentationService.GetAllProductPresentationGroups();
            return Ok(_productPresentationGroups);
        }

        [HttpPost("group")]
        public IActionResult CreateProductPresentationGroup([FromBody] ProductPresentationGroupRequest productPresentationGroup)
        {
            var _productPresentationGroup = _productPresentationService.CreateProductPresentationGroup(productPresentationGroup);
            return Ok(_productPresentationGroup);
        }  

        [HttpGet("group/{code}")]
        public IActionResult GetProductPresentationGroupDetail(int code)
        {
            var _productPresentationGroup = _productPresentationService.GetProductPresentationGroupDetail(code);
            return Ok(_productPresentationGroup);
        }    

        [HttpPut("group/{code}")]
        public IActionResult UpdateLaboratoryGroup(int code, [FromBody] ProductPresentationGroupRequest productPresentationGroup)
        {
            try {
                var _productPresentationGroup = _productPresentationService.UpdateProductPresentationGroup(code, productPresentationGroup);
                return Ok(_productPresentationGroup);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }        

        [HttpDelete("group/{code}")]
        public IActionResult Delete(int code)
        {
            try
            {
                var _productPresentationGroup = _productPresentationService.DeleteProductPresentationGroup(code);
                return Ok(_productPresentationGroup);    
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }   

        [HttpGet("by-pharmaceuticalForm")]
        public IActionResult GetAllByProduct([FromQuery]string pharmaceuticalFormCodes, [FromQuery]string productCodes, [FromQuery]string productGroupCodes)
        {
            int[] arrPharmaceuticalFormCodes = new int[0];
            int[] arrProductCodes = new int[0];
            int[] arrProductGroupCodes = new int[0];

            if (pharmaceuticalFormCodes == null) {
                return BadRequest("El parametro pharmaceuticalFormCodes debe estar seteado");
            }

            if (pharmaceuticalFormCodes !=  null) {
                arrPharmaceuticalFormCodes = Array.ConvertAll(pharmaceuticalFormCodes.Split(','), int.Parse);
            }

            if (productCodes !=  null) {
                arrProductCodes = Array.ConvertAll(productCodes.Split(','), int.Parse);
            }

            if (productGroupCodes !=  null) {
                arrProductGroupCodes = Array.ConvertAll(productGroupCodes.Split(','), int.Parse);
            }            

            var _productPresentationComponents = _productPresentationService.GetAllProductPresentationComponentsByPharmaceuticalForm(arrPharmaceuticalFormCodes, arrProductCodes, arrProductGroupCodes);
            return Ok(_productPresentationComponents);
        }

        [HttpPut("{code}")]
        public IActionResult Update(int code, [FromBody] ProductPresentationRequest productPresentation)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
            User user = _userService.GetById(userId); 

            var result = _productPresentationService.UpdateProductPresentation(code, productPresentation, user);
            return Ok();
        }                                   
    }
}
