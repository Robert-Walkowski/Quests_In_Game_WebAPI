using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Output;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Output
{
    public class CreateQuestOutputDto
    {
        public QuestStatus Status { get; set; }
        public string Message { get; set; }
    }
}
