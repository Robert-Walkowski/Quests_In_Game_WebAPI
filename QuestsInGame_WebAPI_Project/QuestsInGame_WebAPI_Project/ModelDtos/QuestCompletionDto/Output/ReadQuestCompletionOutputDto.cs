using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestCompletionDto.Output
{
    public class ReadQuestCompletionOutputDto : CreateQuestCompletionOutputDto
    {
        public QuestCompletionModel? QuestCompletion { get; set; }
    }
}
