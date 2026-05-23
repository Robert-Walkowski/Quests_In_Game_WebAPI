using QuestsInGame_WebAPI_Project.Enums;

namespace QuestsInGame_WebAPI_Project.Models
{
    public class QuestModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Difficulty { get; set; }
        public QuestReward Reward { get; set; }
        public QuestStatus Status { get; set; }
    }
}
