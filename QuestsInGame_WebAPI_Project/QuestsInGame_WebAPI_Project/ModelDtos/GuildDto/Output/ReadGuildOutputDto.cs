using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Output
{
    public class ReadGuildOutputDto : CreateGuildOutputDto
    {
        public GuildModel? ReadGuild { get; set; }
    }
}
