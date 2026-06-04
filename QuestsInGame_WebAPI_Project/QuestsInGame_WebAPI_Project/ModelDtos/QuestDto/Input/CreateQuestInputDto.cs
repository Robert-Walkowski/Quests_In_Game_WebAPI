using QuestsInGame_WebAPI_Project.Enums;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Input
{
    public class CreateQuestInputDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int QuestLevel { get; set; }
        public QuestReward Reward { get; set; }
        public QuestCompletionStatus Status { get; set; }
    }
}
