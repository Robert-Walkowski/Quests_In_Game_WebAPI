using Microsoft.AspNetCore.Mvc;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Input;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Output;
using QuestsInGame_WebAPI_Project.Models;
using QuestsInGame_WebAPI_Project.Services;

namespace QuestsInGame_WebAPI_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharactersService _charactersService;

        public CharacterController(ICharactersService charactersService)
        {
            this._charactersService = charactersService;
        }

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
            (CharacterModel? loadedCharacter, CharacterStatus status) = await _charactersService.ReadCharacterAsync(id);

            ReadCharacterOutputDto response = new ReadCharacterOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                Character = loadedCharacter
            };

            if (status == CharacterStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("updateCharacter/{id}")]
        public async Task<IActionResult> UpdateCharacterAsync(string id, [FromBody] UpdateCharacterInputDto request)
        {
            (CharacterModel? updatedCharacter, CharacterStatus status) = await _charactersService.UpdateCharacterAsync(id, request);

            UpdateCharacterOutputDto response = new UpdateCharacterOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                Character = updatedCharacter
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
