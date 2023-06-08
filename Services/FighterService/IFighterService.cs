using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI.Services.FighterService
{
    public interface IFighterService
    {
        List<Fighter> GetAllFighters();
        Fighter GetSingleFighter(int id);
        List<Fighter> CreateFighter(Fighter newFighter);
    }
}