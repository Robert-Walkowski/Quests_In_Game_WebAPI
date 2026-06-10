namespace QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Input
{
    public class CreateGuildInputDto
    {
        public string GuildName { get; set; }
        public string? GuildDescription { get; set; }
        public List<string>? MembersId { get; set; }
    }
}
