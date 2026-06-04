namespace QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Input
{
    public class UpdateGuildInputDto
    {
        public string? NewGuildName { get; set; }
        public string? NewDescription { get; set; }
        public List<string>? NewMembersId { get; set; }
    }
}
