using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fightAPI.Controllers
{
    [Authorize] // Authorize requires a token to access any of the methods
    [ApiController]
    [Route("api/[controller]")]
    public class FighterController : ControllerBase
    {
        private readonly IFighterService _fighterService;

        public FighterController(IFighterService fighterService)
        {
            _fighterService = fighterService;
        }

        /* **** Note: Need to fix User Fighter relationships 
        currently you must add relationships manually to DB 
        otherwise when use the get All fighters route it
        will only return empty results! **** */

        //[AllowAnonymous] // AllowAnonymous allows us to access this method without a token
        [HttpGet("All")]
        public async Task<ActionResult<ServiceResponse<List<GetFighterResponseDto>>>> Get() // must add async <task> and await to return
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _fighterService.GetAllFighters(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetFighterResponseDto>>> GetSingle(int id) // must add async <task> and await to return
        {
            var fighter = _fighterService.GetFighterById(id);
            if (fighter == null)
            {
                return NotFound();
            }
            return Ok(await fighter);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetFighterResponseDto>>>> CreateFighter(AddFighterRequestDto newFighter) // must add async <task> and await to return
        {
            return Ok(await _fighterService.CreateFighter(newFighter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetFighterResponseDto>>> UpdateFighter(UpdateFighterRequestDto updatedFighter) // must add async <task> and await to return
        {
            var response = await _fighterService.UpdateFighter(updatedFighter);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetFighterResponseDto>>>> DeleteFighter(int id) // must add async <task> and await to return
        {
            var response = await _fighterService.DeleteFighter(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}