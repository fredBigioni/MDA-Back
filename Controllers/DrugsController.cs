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
    public class DrugsController : ControllerBase
    {
        private IDrugService _drugService;

        public DrugsController(IDrugService drugService)
        {
            _drugService = drugService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var drugs = _drugService.GetAll();
            return Ok(drugs);
        }

        [HttpGet("components")]
        public IActionResult GetAllDrugComponents()
        {
            var drugs = _drugService.GetAllDrugComponents();
            return Ok(drugs);
        }        

        [HttpGet("{code}")]
        public IActionResult GetById(int code)
        {
            var drug = _drugService.GetByCode(code);
            if (drug == null) return NotFound();

            return Ok(drug);
        }

        [HttpPut("{code}")]
        public IActionResult Update(int code, Drug drug)
        {
            try {
                _drugService.Update(code, drug);
                return Ok(drug);
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("groups")]
        public IActionResult GetAllDrugGroups()
        {
            var _drugGroup = _drugService.GetAllDrugGroups();
            return Ok(_drugGroup);
        } 

        [HttpPost("group")]
        public IActionResult CreateDrugGroup([FromBody] DrugGroupRequest drugGroup)
        {
            var _drugGroup = _drugService.CreateDrugGroup(drugGroup);
            return Ok(_drugGroup);
        } 

        [HttpGet("group/{code}")]
        public IActionResult GetDrugGroupDetail(int code)
        {
            var _drugGroup = _drugService.GetDrugGroupDetail(code);
            return Ok(_drugGroup);
        }    

        [HttpPut("group/{code}")]
        public IActionResult UpdateDrugGroup(int code, [FromBody] DrugGroupRequest drugGroup)
        {
            try {
                var _drugGroup = _drugService.UpdateDrugGroup(code, drugGroup);
                return Ok(_drugGroup);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("group/{code}")]
        public IActionResult Delete(int code)
        {
            try
            {
                var _drugGroup = _drugService.DeleteDrugGroup(code);
                return Ok(_drugGroup);    
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }                             
    }
}
