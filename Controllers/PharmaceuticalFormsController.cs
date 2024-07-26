using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Models;
using WebApi.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PharmaceuticalFormsController : ControllerBase
    {
        private IPharmaceuticalFormService _pharmaceuticalFormService;

        public PharmaceuticalFormsController(IPharmaceuticalFormService pharmaceuticalFormService)
        {
            _pharmaceuticalFormService = pharmaceuticalFormService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _pharmaceuticalForms = _pharmaceuticalFormService.GetAll();
            return Ok(_pharmaceuticalForms);
        }

        [HttpGet("by-product")]
        public IActionResult GetAllByProduct([FromQuery]string productCodes, [FromQuery]string productGroupCodes)
        {
            int[] arrProductCodes = new int[0];
            int[] arrProductGroupCodes  = new int[0];

            if (productCodes == null && productGroupCodes == null) {
                return BadRequest("Al menos uno de los parametros productCodes,productGroupCodes deben estar seteados");
            }
            
            if (productCodes !=  null) {
                arrProductCodes = Array.ConvertAll(productCodes.Split(','), int.Parse);
            }

            if (productGroupCodes !=  null) {
                arrProductGroupCodes = Array.ConvertAll(productGroupCodes.Split(','), int.Parse);
            }

            var _pharmaceuticalForms = _pharmaceuticalFormService.GetAllByProduct(arrProductCodes, arrProductGroupCodes);
            return Ok(_pharmaceuticalForms);
        }

        [HttpGet("by-productPresentation")]
        public IActionResult GetAllByProductPresentation([FromQuery]string productPresentationCodes, [FromQuery]string productPresentationGroupCodes)
        {
            int[] arrProductPresentationCodes = new int[0];
            int[] arrProductPresentationGroupCodes  = new int[0];

            if (productPresentationCodes == null && productPresentationGroupCodes == null) {
                return BadRequest("Al menos uno de los parametros productPresentationCodes,productPresentationGroupCodes deben estar seteados");
            }
            
            if (productPresentationCodes !=  null) {
                arrProductPresentationCodes = Array.ConvertAll(productPresentationCodes.Split(','), int.Parse);
            }

            if (productPresentationGroupCodes !=  null) {
                arrProductPresentationGroupCodes = Array.ConvertAll(productPresentationGroupCodes.Split(','), int.Parse);
            }

            var _pharmaceuticalForms = _pharmaceuticalFormService.GetAllByProductPresentation(arrProductPresentationCodes, arrProductPresentationGroupCodes);
            return Ok(_pharmaceuticalForms);
        }                   
    }
}
