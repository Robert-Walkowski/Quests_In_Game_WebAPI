using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Input;
using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.Interfaces
{
    public interface IGuildsService
    {
        Task<GuildStatus> CreateGuildAsync(CreateGuildInputDto request);
        Task<GuildStatus> DeleteGuildAsync(string guildId);
        Task<(GuildModel?, GuildStatus)> ReadGuildAsync(string guildId);
        Task<(GuildModel?, GuildStatus)> UpdateGuildDataAsync(string guildId, UpdateGuildInputDto request);
    }
}