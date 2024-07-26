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
    public class LinesController : ControllerBase
    {
        private ILineService _lineService;

        public LinesController(ILineService lineService)
        {
            _lineService = lineService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _lines = _lineService.GetAll();
            return Ok(_lines);
        }

        [HttpGet("by-lineGroup")]
        public IActionResult GetAllLineByLineGroup([FromQuery]string lineGroupCodes)
        {
            int[] arrlineGroupCodes  = new int[0];

            if (lineGroupCodes !=  null) {
                arrlineGroupCodes = Array.ConvertAll(lineGroupCodes.Split(','), int.Parse);
            }

            var _lines = _lineService.GetAllLinesByLineGroup(arrlineGroupCodes);

            return Ok(_lines);
        }        

        [HttpGet("{code}")]
        public IActionResult GetByCode(int code)
        {
            var _line = _lineService.GetByCode(code);
            if (_line == null) return NotFound();

            return Ok(_line);
        }

        [HttpPut("{code}")]
        public IActionResult Update(int code, [FromBody] LineRequest line)
        {
            try {
                var _line = _lineService.Update(code, line);
                return Ok(_line);
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] LineRequest line)
        {
            var _line = _lineService.Create(line);
            return Ok(_line);
        }                        
    }
}
