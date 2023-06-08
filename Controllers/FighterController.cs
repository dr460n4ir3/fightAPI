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
        public ActionResult<List<Fighter>> Get() 
        {
            return Ok(_fighterService.GetAllFighters());
        }

        [HttpGet("{id}")]
        public ActionResult<Fighter> GetSingle(int id)
        {
            var fighter = _fighterService.GetSingleFighter(id);
            if (fighter == null)
            {
                return NotFound();
            }
            return Ok(fighter);
        }

        [HttpPost]
        public ActionResult<List<Fighter>> Create(Fighter newFighter)
        {
            return Ok(_fighterService.CreateFighter(newFighter));
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