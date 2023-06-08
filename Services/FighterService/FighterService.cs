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
        
        public  async Task<ServiceResponse<List<Fighter>>> CreateFighter(Fighter newFighter)
        {
            var serviceResponse = new ServiceResponse<List<Fighter>>();
            fighters.Add(newFighter);
            serviceResponse.Data = fighters;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Fighter>>> GetAllFighters()
        {
            var serviceResponse = new ServiceResponse<List<Fighter>>();
            serviceResponse.Data = fighters;
            return serviceResponse;
        }

        public async Task<ServiceResponse<Fighter>> GetFighterById(int id)
        {
            var serviceResponse = new ServiceResponse<Fighter>();
            var fighter = fighters.FirstOrDefault(f => f.Id == id);
            serviceResponse.Data = fighter;
            return serviceResponse;
        }
    }
}