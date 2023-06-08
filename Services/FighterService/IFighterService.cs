using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI.Services.FighterService
{
    public interface IFighterService
    {
        Task<ServiceResponse<List<Fighter>>> GetAllFighters();
        Task<ServiceResponse<Fighter>> GetFighterById(int id);
        Task<ServiceResponse<List<Fighter>>> CreateFighter(Fighter newFighter);
    }
}