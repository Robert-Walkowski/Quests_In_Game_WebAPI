using QuestsInGame_WebAPI_Project.Enums;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Input
{
    public class CreateQuestInputDto
    {
        public string QuestTitle { get; set; }
        public string? QuestDescription { get; set; }
        public int QuestLevel { get; set; }
        public QuestReward Reward { get; set; }
        public QuestCompletionStatus Status { get; set; }
    }
}
