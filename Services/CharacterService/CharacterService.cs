using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor ;
        
        public CharacterService(IMapper mapper , DataContext context , IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId()=> int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newcharacter)
        {
            var serviceResponse =  new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newcharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters
            .Where(c => c.User.Id == GetUserId())
            .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return  serviceResponse ;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteChaacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync( c => c.Id == id  && c.User.Id == GetUserId());
                if(character != null)
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = _context.Characters
                    .Where(c => c.User.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character Not Found !";
                }
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> getAllCharacters()
        {
            var serviceResponse =  new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters
            .Include(c=> c.weapon)
            .Include(c=> c.Skills)
            .Where(c=> c.User.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;    
        }

        public async Task<ServiceResponse<GetCharacterDto>> getCharaterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters
            .Include(c=> c.weapon)
            .Include(c=> c.Skills)
            .FirstOrDefaultAsync(c =>c.Id == id && c.User.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;            
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters
                .Include(c=> c.User)
                .FirstOrDefaultAsync( c => c.Id == updatedCharacter.Id);
                if(character.User.Id == GetUserId())
                {
                    character.Name = updatedCharacter.Name;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Strenght = updatedCharacter.Strenght;
                    character.Defence = updatedCharacter.Defence;
                    character.Intelligence = updatedCharacter.Intelligence;
                    character.Class = updatedCharacter.Class;

                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character Not Found !"; 
                }
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkills(AddCharacterSkillDto newCharacterSkil)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                .Include(c=> c.weapon)      //Including Addition of Data
                .Include(c=> c.Skills)      //Including Addition of Data
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkil.CharacterId && c.User.Id == GetUserId());

                if(character == null)
                {
                    response.Success = false;
                    response.Message = "Character not Found !";
                    return response;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s=> s.Id == newCharacterSkil.SkillId);
                if(skill == null)
                {
                    response.Success = false;
                    response.Message = "Skill Not Found";
                    return response;
                }

                character.Skills.Add(skill);
                await _context.SaveChangesAsync();
                
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}