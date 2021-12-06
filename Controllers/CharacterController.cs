using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [Authorize]//Enables Authorizaion to Controller
    [ApiController]//Enables Api Features
    [Route("[controller]")]//We Can Access the Controller with its Name 
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        //Injecting the Service (Dependecny Injection)
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        //Returning all the Characters List
        //[AllowAnonymous] // Allow to the all user
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(await _characterService.getAllCharacters(id));            
        }

        //Returning the SIngle Character with given Parameter
        [HttpGet("{id}")]   
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(await _characterService.getCharaterById(id));            
        }       
        
        //Adding Character to the List
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter (AddCharacterDto newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        //Updating Character of the list
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            if(response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]   
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
        {
            var response = await _characterService.DeleteChaacter(id);
            if(response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);            
        }   
    }
}