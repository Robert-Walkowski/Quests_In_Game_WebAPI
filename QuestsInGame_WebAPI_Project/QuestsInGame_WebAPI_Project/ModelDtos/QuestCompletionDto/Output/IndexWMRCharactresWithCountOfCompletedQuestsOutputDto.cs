using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.StaticIndexes;
using System.IO.Pipes;

namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestCompletionDto.Output
{
    public class IndexWMRCharactresWithCountOfCompletedQuestsOutputDto
    {
        public QuestCompletionStatusEnum Status { get; set; }
        public string Message { get; set; }
        public List<QuestCompletions_ByCharacter.Result> ResultList { get; set; }
    }
}
