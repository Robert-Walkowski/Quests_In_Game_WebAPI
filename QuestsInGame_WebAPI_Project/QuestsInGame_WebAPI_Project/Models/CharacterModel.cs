namespace QuestsInGame_WebAPI_Project.Models
{
    public class CharacterModel
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? GameClass { get; set; }
        public int? Level { get; set; }
        public string? CharacterGuildId { get; set; }
    }
}
