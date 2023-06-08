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
        public async Task<ActionResult<ServiceResponse<List<Fighter>>>> Get() // must add async <task> and await to return
        {
            return Ok(await _fighterService.GetAllFighters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Fighter>>> GetSingle(int id) // must add async <task> and await to return
        {
            var fighter = _fighterService.GetFighterById(id);
            if (fighter == null)
            {
                return NotFound();
            }
            return Ok(await fighter);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Fighter>>>> CreateFighter(Fighter newFighter) // must add async <task> and await to return
        {
            return Ok(await _fighterService.CreateFighter(newFighter));
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