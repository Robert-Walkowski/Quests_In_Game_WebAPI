using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.StaticIndexes;

namespace QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Output
{
    public class StaticIndexCharacterByExpirienceScoreOutputDto
    {
        public CharacterStatus Status { get; set; }
        public string Message { get; set; }
        public List<Characters_ByExpirienceScore.Result> ResultList { get; set; }
    }
}
