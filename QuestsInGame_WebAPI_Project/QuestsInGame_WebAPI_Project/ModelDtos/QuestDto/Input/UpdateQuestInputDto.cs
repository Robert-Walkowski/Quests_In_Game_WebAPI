using Microsoft.AspNetCore.Localization;
using QuestsInGame_WebAPI_Project.Enums;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Input
{
    public class UpdateQuestInputDto
    {
        public string? NewQuestTitle { get; set; }
        public string? NewDescription { get; set; }
        public int NewQuestLevel { get; set; } = 0;
        public QuestReward? NewQuestReward { get; set; }
        public QuestCompletionStatus NewStatus { get; set; } = QuestCompletionStatus.AVAILABLE;
    }
}
