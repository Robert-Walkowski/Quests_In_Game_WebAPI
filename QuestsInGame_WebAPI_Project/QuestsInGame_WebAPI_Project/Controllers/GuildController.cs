using Microsoft.AspNetCore.Mvc;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Input;
using QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Output;
using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuildController : ControllerBase
    {
        private IGuildsService _guildsService;

        public GuildController(IGuildsService guildsService)
        {
            this._guildsService = guildsService;
        }

        [HttpPost("guildCreate")]
        public async Task<IActionResult> CreateGuildAsync(CreateGuildInputDto request)
        {
            GuildStatus status = await _guildsService.CreateGuildAsync(request);

            CreateGuildOutputDto response = new CreateGuildOutputDto
            {
                Status = status,
                Message = status.ToMessage()
            };

            if (status == GuildStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("readGuild/{id}")]
        public async Task<IActionResult> ReadGuildInformationAsync(string id)
        {
            (GuildModel? guild, GuildStatus status) = await _guildsService.ReadGuildAsync(id);

            ReadGuildOutputDto response = new ReadGuildOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                ReadGuild = guild
            };

            if (status == GuildStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("updateGuild/{id}")]
        public async Task<IActionResult> UpdateGuildInformationAsyn(string id, UpdateGuildInputDto request)
        {
            (GuildModel? guild, GuildStatus status) = await _guildsService.UpdateGuildDataAsync(id, request);

            UpdateGuildOutputDto response = new UpdateGuildOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                ReadGuild = guild
            };

            if (status == GuildStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("deleteGuild/{id}")]
        public async Task<IActionResult> DeleteGuildAsync(string id)
        {
            GuildStatus status = await _guildsService.DeleteGuildAsync(id);

            DeleteGuildOutputDto response = new DeleteGuildOutputDto
            {
                Status = status,
                Message = status.ToMessage()
            };

            if (status == GuildStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
