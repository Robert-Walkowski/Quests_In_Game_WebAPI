namespace QuestsInGame_WebAPI_Project.Models
{
    public class GuildModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? MembersId { get; set; }
    }
}
