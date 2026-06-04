using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Input;
using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.Interfaces
{
    public interface ICharactersService
    {
        Task<CharacterStatus> CreateCharacterAsync(CreateCharacterInputDto request);
        Task<CharacterStatus> DeleteCharacterAsync(string characterId);
        Task<(CharacterModel?, CharacterStatus)> ReadCharacterAsync(string characterId);
        Task<(CharacterModel?, CharacterStatus)> UpdateCharacterAsync(string id, UpdateCharacterInputDto request);
    }
}