using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.StaticIndexes;

namespace QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Output
{
    public class StaticIndexCharacterWithGuildNameOutputDto
    {
        public CharacterStatus Status { get; set; }
        public string Message { get; set; }
        public List<Characters_WithGuildName.Result> ResultList { get; set; }
    }
}
