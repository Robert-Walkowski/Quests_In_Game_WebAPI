namespace QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Input
{
    public class UpdateGuildInputDto
    {
        public string? NewGuildName { get; set; }
        public string? NewGuildDescription { get; set; }
        public List<string>? NewMembersId { get; set; }
    }
}
