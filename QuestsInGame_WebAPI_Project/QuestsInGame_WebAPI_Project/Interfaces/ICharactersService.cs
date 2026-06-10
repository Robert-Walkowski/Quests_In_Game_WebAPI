using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Input;
using QuestsInGame_WebAPI_Project.Models;
using QuestsInGame_WebAPI_Project.StaticIndexes;

namespace QuestsInGame_WebAPI_Project.Interfaces
{
    public interface ICharactersService
    {
        Task<CharacterStatus> CreateCharacterAsync(CreateCharacterInputDto request);
        Task<CharacterStatus> DeleteCharacterAsync(string characterId);
        Task<(CharacterModel?, CharacterStatus)> ReadCharacterAsync(string characterId);
        Task<(CharacterModel?, CharacterStatus)> UpdateCharacterAsync(string id, UpdateCharacterInputDto request);
        Task<(List<CharacterModel>, CharacterStatus)> AutoIndexCharactersWithLevel10OrMoreAndWarriorClassAsync();
        Task<(List<CharacterModel>, CharacterStatus)> AutoIndexCharactersWithGuildOnlyAsync();
        Task<(List<Characters_WithGuildName.Result>, CharacterStatus)> StaticIndexCharactersWithGuildNameWithGuildAsync();
        Task<(List<Characters_ByExpirienceScore.Result>, CharacterStatus)> StaticIndexCharacterByExpirienceScoreWithGoldTierAndClassWarriorAsync();
        Task<(List<CharacterModel>, CharacterStatus)> SkipFourFirstCharactersAndTakeTwoAsync();
    }
}