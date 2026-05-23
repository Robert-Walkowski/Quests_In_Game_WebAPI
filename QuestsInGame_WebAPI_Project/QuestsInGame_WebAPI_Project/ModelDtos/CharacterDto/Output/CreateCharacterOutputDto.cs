using QuestsInGame_WebAPI_Project.Enums;

namespace QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Output
{
    public class CreateCharacterOutputDto
    {
        public CharacterStatus Status { get; set; }
        public string Message { get; set; }
    }
}
