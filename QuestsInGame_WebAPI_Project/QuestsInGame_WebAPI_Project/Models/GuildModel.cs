namespace QuestsInGame_WebAPI_Project.Models
{
    public class GuildModel
    {
        public string Id { get; set; }
        public string GuildName { get; set; }
        public string? GuildDescription { get; set; }
        public List<string>? MembersId { get; set; }
    }
}
