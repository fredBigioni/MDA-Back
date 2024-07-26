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
    public class BusinessUnitsController : ControllerBase
    {
        private IBusinessUnitService _businessUnitService;

        public BusinessUnitsController(IBusinessUnitService businessUnitService)
        {
            _businessUnitService = businessUnitService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _businessUnits = _businessUnitService.GetAll();
            return Ok(_businessUnits);
        }     

        [HttpGet("{code}")]
        public IActionResult GetByCode(int code)
        {
            var _businessUnit = _businessUnitService.GetByCode(code);
            if (_businessUnit == null) return NotFound();

            return Ok(_businessUnit);
        }

        [HttpPost]
        public IActionResult Create([FromBody] BusinessUnitRequest businessUnit)
        {
            var _businessUnit = _businessUnitService.Create(businessUnit);
            return Ok(_businessUnit);
        }   

        [HttpPut("{code}")]
        public IActionResult Update(int code, [FromBody] BusinessUnitRequest businessUnit)
        {
            try {
                var result = _businessUnitService.Update(code, businessUnit);
                return Ok();
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

    }
}
