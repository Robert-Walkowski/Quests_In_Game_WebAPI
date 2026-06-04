using QuestsInGame_WebAPI_Project.Enums;

namespace QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Output
{
    public class CreateGuildOutputDto
    {
        public GuildStatus Status { get; set; }
        public string Message { get; set; }
    }
}
