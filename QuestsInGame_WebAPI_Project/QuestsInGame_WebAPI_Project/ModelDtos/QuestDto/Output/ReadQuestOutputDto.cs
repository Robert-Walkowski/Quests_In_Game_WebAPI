using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Output
{
    public class ReadQuestOutputDto : CreateQuestOutputDto
    {
        public QuestModel? QuestModel { get; set; }
    }
}
