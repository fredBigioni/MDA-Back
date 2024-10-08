using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApi.Services;
using WebApi.Models;
using WebApi.Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApi.Helpers;
using System.Linq;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomMarketsController : ControllerBase
    {
        private ICustomMarketService _customMarketService;
        private IUserService _userService;

        public CustomMarketsController(ICustomMarketService customMarketService, IUserService userService)
        {
            _customMarketService = customMarketService;
            _userService = userService;
        }

        [HttpGet("full-tree")]
        public IActionResult GetFullTree()
        {
            var _customMarkets = _customMarketService.GetTree();
            return Ok(_customMarkets);
        }

        [HttpGet("tree")]
        public IActionResult GetTree()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);

            User user = _userService.GetById(userId);

            var _customMarkets = _customMarketService.GetTree(user);
            return Ok(_customMarkets);
        }

        [HttpGet("GetLastAuditory")]
        public async Task<IActionResult> GetLastAuditory()
        {
            try
            {
                var lastAuditory = await _customMarketService.GetLastAuditory();
                if (lastAuditory != null)
                    return Ok(lastAuditory);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("SignMarket")]
        public async Task<IActionResult> SignMarket([FromBody] SignMarketModel request)
        {
            try
            {
                var signed = await _customMarketService.SignMarket(request);

                return Ok(signed);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAllCustomMarkets()
        {

            var _customMarkets = _customMarketService.GetAllCustomMarkets();

            return Ok(_customMarkets);
        }

        [HttpGet("by-line")]
        public IActionResult GetAllCustomMarketByLine([FromQuery] string lineCodes)
        {
            int[] arrlineCodes = new int[0];

            if (lineCodes != null)
            {
                arrlineCodes = Array.ConvertAll(lineCodes.Split(','), int.Parse);
            }

            var _customMarkets = _customMarketService.GetAllCustomMarketsByLine(arrlineCodes);

            return Ok(_customMarkets);
        }

        [HttpGet("{code}")]
        public IActionResult GetByCode(int code)
        {
            var customMarket = _customMarketService.GetByCode(code);
            if (customMarket == null) return NotFound();

            return Ok(customMarket);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CustomMarketRequest customMarket)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);

            User user = _userService.GetById(userId);

            if (!user.IsAdmin) return Forbid();

            var _customMarket = _customMarketService.Create(customMarket);
            return Ok(_customMarket);
        }


        [HttpPut("{code}")]
        public IActionResult Update(int code, [FromBody] CustomMarketRequest customMarket)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
            User user = _userService.GetById(userId);

            if (!user.IsAdmin) return Forbid();

            try
            {
                var _customMarket = _customMarketService.Update(code, customMarket);
                return Ok(_customMarket);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("is_not_valid_add_detail_custom_market"))
                {
                    string[] arrMessage = new string[0];
                    arrMessage = ex.Message.Split(',');
                    return BadRequest(new
                    {
                        message = arrMessage[0],
                        detailCustomMarketCode = int.Parse(arrMessage[1])
                    });
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("clone/{code}")]
        public IActionResult Clone(int code, [FromBody] CustomMarketRequest customMarket)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
            User user = _userService.GetById(userId);

            if (!user.IsAdmin) return Forbid();

            var _customMarket = _customMarketService.Clone(code, customMarket);
            return Ok(_customMarket);
        }

        [HttpPost("SignAllMarket/{lineCode}/{signedUser}")]
        public async Task<IActionResult> SignAllMarket(int lineCode, int signedUser)
        {
            try
            {
                var signed = await _customMarketService.SignAllMarket(lineCode, signedUser);

                return Ok(signed);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("preview/{code}")]
        public IActionResult GetPreviewByCode(int code)
        {
            var customMarket = _customMarketService.GetPreviewByCode(code);
            if (customMarket == null) return NotFound();

            return Ok(customMarket);
        }

        [HttpGet("actualpreview/{code}")]
        public async Task<IActionResult> GetMarketDetail(int code)
        {
            try
            {
                var result = await _customMarketService.GetMarketDetailLastSignByCustomMarketCode(code);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message} - Internal server error");
            }
        }
        

    [HttpGet("historicpreviews/{code}")]
        public ActionResult<IEnumerable<CustomMarketVersionHistoricModel>> GetAllDetails(int code)
        {
            try
            {
                var details = _customMarketService.GetMarketDetailHistoricByCustomMarketCode(code);
                return Ok(details);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"{ex.Message} - Internal server error");
            }
        }

        [HttpGet("historicpreviewstoscreen/{versionCode}")]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetHistoricPreviewToScreen(int versionCode)
        {
            var customMarket = await _customMarketService.GetMarketPreviewHistoricByCustomMarketCode(versionCode);
            if (customMarket == null || !customMarket.Any()) return NotFound();

            return Ok(customMarket);
        }
        [HttpGet("lastsignpreviewstoscreen/{code}")]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetLastSignPreviewToScreen(int code)
        {
            var customMarket = await _customMarketService.GetLastSignedPreviewHistoricByCustomMarketCode(code);
            if (customMarket == null || !customMarket.Any()) return NotFound();

            return Ok(customMarket);
        }


        [HttpDelete("{code}")]
        public IActionResult Delete(int code)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
                User user = _userService.GetById(userId);

                if (!user.IsAdmin) return Forbid();

                var _customMarket = _customMarketService.Delete(code);
                return Ok(_customMarket);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("is_not_valid_delete_custom_market"))
                {
                    string[] arrMessage = new string[0];
                    arrMessage = ex.Message.Split(',');
                    return BadRequest(new
                    {
                        message = arrMessage[0],
                        customMarketDescription = arrMessage[1]
                    });
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
