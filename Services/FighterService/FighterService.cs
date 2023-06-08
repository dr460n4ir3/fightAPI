using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI.Services.FighterService
{
    public class FighterService : IFighterService
    {
        private static List<Fighter> fighters = new List<Fighter> {
            new Fighter(),
            new Fighter { Id = 1, Name = "Luffy" }
        };
        
        public List<Fighter> CreateFighter(Fighter newFighter)
        {
            fighters.Add(newFighter);
            return fighters;
        }

        public List<Fighter> GetAllFighters()
        {
            return fighters;
        }

        public Fighter GetSingleFighter(int id)
        {
            return fighters.FirstOrDefault(f => f.Id == id);
        }
    }
}