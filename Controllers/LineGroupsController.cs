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
    public class LineGroupsController : ControllerBase
    {
        private ILineGroupService _lineGroupService;

        public LineGroupsController(ILineGroupService lineGroupService)
        {
            _lineGroupService = lineGroupService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
/*             var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value; */

            var _lineGroups = _lineGroupService.GetAll();
            return Ok(_lineGroups);
        }

        [HttpGet("{code}")]
        public IActionResult GetByCode(int code)
        {
            var _lineGroup = _lineGroupService.GetByCode(code);
            if (_lineGroup == null) return NotFound();

            return Ok(_lineGroup);
        }

        [HttpPut("{code}")]
        public IActionResult Update(int code, [FromBody] LineGroupRequest lineGroup)
        {
            try {
                var _lineGroup = _lineGroupService.Update(code, lineGroup);
                return Ok(lineGroup);
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] LineGroupRequest lineGroup)
        {
            var _lineGroup = _lineGroupService.Create(lineGroup);
            return Ok(_lineGroup);
        }                         
    }
}
