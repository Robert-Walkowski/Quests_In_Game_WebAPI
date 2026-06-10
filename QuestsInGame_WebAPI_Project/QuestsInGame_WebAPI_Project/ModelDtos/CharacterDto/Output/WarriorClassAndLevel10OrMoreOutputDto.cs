using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Output
{
    public class WarriorClassAndLevel10OrMoreOutputDto
    {
        public CharacterStatus Status { get; set; }
        public string Message { get; set; }
        public List<CharacterModel> ResultList { get; set; }
    }
}
