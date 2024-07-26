using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Models;
using System;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomMarketGroupsController : ControllerBase
    {
        private ICustomMarketGroupService _customMarketGroupService;

        public CustomMarketGroupsController(ICustomMarketGroupService customMarketGroupService)
        {
            _customMarketGroupService = customMarketGroupService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _customMarketGroups = _customMarketGroupService.GetAll();
            return Ok(_customMarketGroups);
        }

        [HttpGet("by-customMarket")]
        public IActionResult GetAllByCustomMarketCode([FromQuery]int customMarketCode)
        {
            var _customMarketGroups = _customMarketGroupService.GetAllByCustomMarket(customMarketCode);
            return Ok(_customMarketGroups);
        }

        [HttpPut("{code}")]
        public IActionResult Update(int code, [FromBody] CustomMarketGroupRequest customMarketGroup)
        {
            try {
                var _customMarketGroup = _customMarketGroupService.Update(code, customMarketGroup);
                return Ok(_customMarketGroup);
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }        

        [HttpPost]
        public IActionResult Create([FromBody] CustomMarketGroupRequest customMarketGroup)
        {
            var _customMarketGroup = _customMarketGroupService.Create(customMarketGroup);
            return Ok(_customMarketGroup);
        }

        [HttpDelete("{code}")]
        public IActionResult Delete(int code)
        {
            try {
                var _customMarketGroup = _customMarketGroupService.Delete(code);
                return Ok(_customMarketGroup);
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }          
        }               
    }
}
