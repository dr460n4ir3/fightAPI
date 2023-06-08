using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI.Services.FighterService
{
    public interface IFighterService
    {
        Task<ServiceResponse<List<GetFighterResponseDto>>> GetAllFighters();
        Task<ServiceResponse<GetFighterResponseDto>> GetFighterById(int id);
        Task<ServiceResponse<List<GetFighterResponseDto>>> CreateFighter(AddFighterRequestDto newFighter);
        Task<ServiceResponse<GetFighterResponseDto>> UpdateFighter(UpdateFighterRequestDto updatedFighter);
    }
}