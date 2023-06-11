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
        private readonly DataContext _context;

        public FighterService(IMapper mapper, DataContext context) // the DataContext is injected to connect to the database
        {
            _context = context;
            _mapper = mapper;
        }

        public  async Task<ServiceResponse<List<GetFighterResponseDto>>> CreateFighter(AddFighterRequestDto newFighter)
        {
            var serviceResponse = new ServiceResponse<List<GetFighterResponseDto>>();
            var fighter = _mapper.Map<Fighter>(newFighter);
            _context.Fighters.Add(fighter);
            await _context.SaveChangesAsync(); // this saves the changes to the database
            serviceResponse.Data = await _context.Fighters.Select(f => _mapper.Map<GetFighterResponseDto>(f)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetFighterResponseDto>>> GetAllFighters(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetFighterResponseDto>>();
            var dbFighters = await _context.Fighters.Where(c => c.User!.Id ==userId).ToListAsync();
            serviceResponse.Data = dbFighters.Select(f => _mapper.Map<GetFighterResponseDto>(f)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetFighterResponseDto>> GetFighterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetFighterResponseDto>();
            var dbFighter = await _context.Fighters.FirstOrDefaultAsync(f => f.Id == id);
            serviceResponse.Data = _mapper.Map<GetFighterResponseDto>(dbFighter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetFighterResponseDto>> UpdateFighter(UpdateFighterRequestDto updatedFighter)
        {
            var serviceResponse = new ServiceResponse<GetFighterResponseDto>();

            try
            {
                var fighter = await _context.Fighters.FirstOrDefaultAsync(f => f.Id == updatedFighter.Id);
                if(fighter == null)
                {
                    throw new Exception($"There is no Fighter with the Id of '{updatedFighter.Id}' please check the Id and try again.");
                }

                
                _mapper.Map(updatedFighter, fighter); // this is the same as the code below (commented out code)
                await _context.SaveChangesAsync(); // this saves the changes to the database
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
                var fighter = await _context.Fighters.FirstOrDefaultAsync(f => f.Id == id);
                if(fighter == null)
                {
                    throw new Exception($"There is no Fighter with the Id of '{id}' please check the Id and try again.");
                }
                _context.Fighters.Remove(fighter);
                await _context.SaveChangesAsync(); // this saves the changes to the database
                serviceResponse.Data = await _context.Fighters.Select(f => _mapper.Map<GetFighterResponseDto>(f)).ToListAsync();
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