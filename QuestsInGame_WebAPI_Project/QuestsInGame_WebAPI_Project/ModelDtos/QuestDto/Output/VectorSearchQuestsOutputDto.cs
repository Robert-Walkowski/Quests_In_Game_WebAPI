using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Output
{
    public class VectorSearchQuestsOutputDto
    {
        public QuestStatus Status { get; set; }
        public string Message { get; set; }
        public List<QuestModel> ResultList { get; set; }
    }
}
