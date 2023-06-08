using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Fighter, GetFighterResponseDto>(); 
            CreateMap<AddFighterRequestDto, Fighter>();
        }
    }
}