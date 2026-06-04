namespace QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Input
{
    public class CreateGuildInputDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? MembersId { get; set; }
    }
}
