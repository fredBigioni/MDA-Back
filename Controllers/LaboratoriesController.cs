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
    public class LaboratoriesController : ControllerBase
    {
        private ILaboratoryService _laboratoryService;

        public LaboratoriesController(ILaboratoryService laboratoryService)
        {
            _laboratoryService = laboratoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _laboratories = _laboratoryService.GetAll();
            return Ok(_laboratories);
        }

        [HttpGet("components")]
        public IActionResult GetAllLaboratoryComponents()
        {
            var _laboratories = _laboratoryService.GetAllLaboratoryComponents();
            return Ok(_laboratories);
        }

        [HttpGet("groups")]
        public IActionResult GetAllLaboratoryGroup()
        {
            var _laboratories = _laboratoryService.GetAllLaboratoryGroup();
            return Ok(_laboratories);
        }              

        [HttpPost("group")]
        public IActionResult CreateLaboratoryGroup([FromBody] LaboratoryGroupRequest laboratoryGroup)
        {
            var _laboratoryGroup = _laboratoryService.CreateLaboratoryGroup(laboratoryGroup);
            return Ok(_laboratoryGroup);
        } 

        [HttpGet("group/{code}")]
        public IActionResult GetLaboratoryGroupDetail(int code)
        {
            var _laboratoryGroup = _laboratoryService.GetLaboratoryGroupDetail(code);
            return Ok(_laboratoryGroup);
        }    

        [HttpPut("group/{code}")]
        public IActionResult UpdateLaboratoryGroup(int code, [FromBody] LaboratoryGroupRequest laboratoryGroup)
        {
            try {
                var _laboratoryGroup = _laboratoryService.UpdateLaboratoryGroup(code, laboratoryGroup);
                return Ok(_laboratoryGroup);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }   

        [HttpDelete("group/{code}")]
        public IActionResult Delete(int code)
        {
            try
            {
                var _laboratoryGroup = _laboratoryService.DeleteLaboratoryGroup(code);
                return Ok(_laboratoryGroup);    
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }                                      
    }
}
