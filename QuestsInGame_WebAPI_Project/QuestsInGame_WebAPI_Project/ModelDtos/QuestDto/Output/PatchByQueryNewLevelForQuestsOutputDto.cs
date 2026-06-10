using QuestsInGame_WebAPI_Project.Enums;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Output
{
    public class PatchByQueryNewLevelForQuestsOutputDto
    {
        public QuestStatus Status { get; set; }
        public string Message { get; set; }
    }
}
