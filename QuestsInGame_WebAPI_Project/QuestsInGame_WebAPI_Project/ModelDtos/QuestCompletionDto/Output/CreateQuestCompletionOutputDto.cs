using QuestsInGame_WebAPI_Project.Enums;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestCompletionDto.Output
{
    public class CreateQuestCompletionOutputDto
    {
        public QuestCompletionStatusEnum Status { get; set; }
        public string Message { get; set; }
    }
}
