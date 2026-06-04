namespace QuestsInGame_WebAPI_Project.Models
{
    public class QuestCompletionModel
    {
        public string Id { get; set; }
        public string CharacterId { get; set; }
        public string QuestId { get; set; }
        public TimeSpan? CompletionTime { get; set; }
        public double? Grade { get; set; }
    }
}
