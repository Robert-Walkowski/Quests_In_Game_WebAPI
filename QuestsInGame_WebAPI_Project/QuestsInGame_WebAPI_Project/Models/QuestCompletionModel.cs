namespace QuestsInGame_WebAPI_Project.Models
{
    public class QuestCompletionModel
    {
        public string Id { get; set; }
        public string CharacterId { get; set; }
        public TimeSpan DurationTime { get; set; }
        public double Grade { get; set; }
    }
}
