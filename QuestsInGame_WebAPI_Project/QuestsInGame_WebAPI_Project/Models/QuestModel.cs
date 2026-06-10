using QuestsInGame_WebAPI_Project.Enums;

namespace QuestsInGame_WebAPI_Project.Models
{
    public class QuestModel
    {
        public string Id { get; set; }
        public string QuestTitle { get; set; }
        public string? QuestDescription { get; set; }
        public int QuestLevel { get; set; }
        public QuestReward? Reward { get; set; }
        public QuestCompletionStatus Status { get; set; } = QuestCompletionStatus.AVAILABLE;
    }
}
