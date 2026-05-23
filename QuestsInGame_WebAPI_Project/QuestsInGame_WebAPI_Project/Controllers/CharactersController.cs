using Microsoft.AspNetCore.Mvc;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Input;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Output;
using QuestsInGame_WebAPI_Project.Models;
using QuestsInGame_WebAPI_Project.Services;

namespace QuestsInGame_WebAPI_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly CharactersService _charactersService;

        public CharactersController(CharactersService charactersService)
        {
            this._charactersService = charactersService;
        }

        //TODO: Make Whole Logic for this controller

        [HttpPost("createCharacter")]
        public async Task<IActionResult> CreateCharacterAsync([FromBody] CreateCharacterInputDto request)
        {
            CharacterStatus status = await _charactersService.CreateCharacterAsync(request);

            CreateCharacterOutputDto response = new CreateCharacterOutputDto()
            {
                Status = status,
                Message = status.ToMessage()
            };

            if (status == CharacterStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("readCharacter/{id}")]
        public async Task<IActionResult> ReadCharacterAsync(string id)
        {
            (CharacterModel? character, CharacterStatus status) = await _charactersService.ReadCharacterAsync(id);

            ReadCharacterOutputDto response = new ReadCharacterOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                Character = character
            };

            if (status == CharacterStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("deleteCharacter/{id}")]
        public async Task<IActionResult> DeleteCharacterAsync(string id)
        {
            CharacterStatus status = await _charactersService.DeleteCharacterAsync(id);

            DeleteCharacterOutputDto response = new DeleteCharacterOutputDto
            {
                Status = status,
                Message = status.ToMessage()
            };

            if (status == CharacterStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
