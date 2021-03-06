using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Skill;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;

namespace dotnet_rpg
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Mapping Charater to GetCharacterDto
            CreateMap<Character,GetCharacterDto>();

            //Mapping AddCharacterDto to Character
            CreateMap<AddCharacterDto,Character>();

            CreateMap<Weapon,GetWeaponDto>();

            CreateMap<Skill, GetSkillDto>();
        }
    }
}