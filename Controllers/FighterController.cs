using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace fightAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FighterController : ControllerBase
    {
        private readonly IFighterService _fighterService;

        public FighterController(IFighterService fighterService)
        {
            _fighterService = fighterService;
        }

        [HttpGet("All")]
        public async Task<ActionResult<ServiceResponse<List<GetFighterResponseDto>>>> Get() // must add async <task> and await to return
        {
            return Ok(await _fighterService.GetAllFighters());
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

        /*[HttpPut("{id}")]
        public ActionResult<Fighter> Update(int id, Fighter fighter)
        {
            var fighterToUpdate = fighters.FirstOrDefault(f => f.Id == id);
            if (fighterToUpdate == null)
            {
                return NotFound();
            }
            fighterToUpdate.Name = fighter.Name;
            fighterToUpdate.Health = fighter.Health;
            fighterToUpdate.Strength = fighter.Strength;
            fighterToUpdate.Defense = fighter.Defense;
            fighterToUpdate.Intelligence = fighter.Intelligence;
            fighterToUpdate.Class = fighter.Class;
            return Ok(fighterToUpdate);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var fighterToDelete = fighters.FirstOrDefault(f => f.Id == id);
            if (fighterToDelete == null)
            {
                return NotFound();
            }
            fighters.Remove(fighterToDelete);
            return Ok();
        }*/
    }
}