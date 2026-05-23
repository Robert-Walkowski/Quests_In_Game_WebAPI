using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Output
{
    public class ReadCharacterOutputDto : CreateCharacterOutputDto
    {
        public CharacterModel? Character { get; set; }
    }
}
