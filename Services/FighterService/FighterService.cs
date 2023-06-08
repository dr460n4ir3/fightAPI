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
        private readonly IMapper _mapper;

        public FighterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public  async Task<ServiceResponse<List<GetFighterResponseDto>>> CreateFighter(AddFighterRequestDto newFighter)
        {
            var serviceResponse = new ServiceResponse<List<GetFighterResponseDto>>();
            var fighter = _mapper.Map<Fighter>(newFighter);
            fighter.Id = fighters.Max(f => f.Id) + 1; // this is a hack to get the next id
            fighters.Add(fighter);
            serviceResponse.Data = fighters.Select(f => _mapper.Map<GetFighterResponseDto>(f)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetFighterResponseDto>>> GetAllFighters()
        {
            var serviceResponse = new ServiceResponse<List<GetFighterResponseDto>>();
            serviceResponse.Data = fighters.Select(f => _mapper.Map<GetFighterResponseDto>(f)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetFighterResponseDto>> GetFighterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetFighterResponseDto>();
            var fighter = fighters.FirstOrDefault(f => f.Id == id);
            serviceResponse.Data = _mapper.Map<GetFighterResponseDto>(fighter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetFighterResponseDto>> UpdateFighter(UpdateFighterRequestDto updatedFighter)
        {
            var serviceResponse = new ServiceResponse<GetFighterResponseDto>();

            try
            {
                var fighter = fighters.FirstOrDefault(f => f.Id == updatedFighter.Id);
                if(fighter == null)
                {
                    throw new Exception($"There is no Fighter with the Id of '{updatedFighter.Id}' please check the Id and try again.");
                }

                _mapper.Map(updatedFighter, fighter); // this is the same as the code below (commented out code)

                /*fighter.Name = updatedFighter.Name;
                fighter.Health = updatedFighter.Health;
                fighter.Strength = updatedFighter.Strength;
                fighter.Defense = updatedFighter.Defense;
                fighter.Intelligence = updatedFighter.Intelligence;
                fighter.Class = updatedFighter.Class;

                serviceResponse.Data = _mapper.Map<GetFighterResponseDto>(fighter);
                return serviceResponse;*/
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetFighterResponseDto>>> DeleteFighter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetFighterResponseDto>>();
            try
            {
                var fighter = fighters.FirstOrDefault(f => f.Id == id);
                if(fighter == null)
                {
                    throw new Exception($"There is no Fighter with the Id of '{id}' please check the Id and try again.");
                }
                fighters.Remove(fighter);
                serviceResponse.Data = fighters.Select(f => _mapper.Map<GetFighterResponseDto>(f)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                
            }
            return serviceResponse;
        }
    }
}