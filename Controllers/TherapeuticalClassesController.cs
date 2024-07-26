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
    public class TherapeuticalClassesController : ControllerBase
    {
        private ITherapeuticalClassService _therapeuticalClassService;

        public TherapeuticalClassesController(ITherapeuticalClassService therapeuticalClassService)
        {
            _therapeuticalClassService = therapeuticalClassService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _therapeuticalClass = _therapeuticalClassService.GetAll();
            return Ok(_therapeuticalClass);
        }

        [HttpGet("component")]
        public IActionResult GetAllTherapeuticalClassesComponent()
        {
            var _therapeuticalClassesComponent = _therapeuticalClassService.GetAllTherapeuticalClassesComponent();
            return Ok(_therapeuticalClassesComponent);
        }           
    }
}
