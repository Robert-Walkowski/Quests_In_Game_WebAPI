using Microsoft.AspNetCore.Mvc;
using QuestsInGame_WebAPI_Project.Database;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Input;
using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Exceptions;

namespace QuestsInGame_WebAPI_Project.Services
{
    public class CharactersService : ICharactersService
    {
        #region CharacterCreate
        public async Task<CharacterStatus> CreateCharacterAsync(CreateCharacterInputDto request)
        {
            try
            {
                GuildModel? guild = null;

                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    if (string.IsNullOrEmpty(request.Name))
                        return CharacterStatus.EMPTY_NAME;
                    if (string.IsNullOrEmpty(request.GameClass))
                        return CharacterStatus.EMPTY_GAMECLASS;
                    if (!availableCharacterClasses.Contains(request.GameClass))
                        return CharacterStatus.INCORRECT_GAMECLASS;
                    if (request.Level < 0)
                        return CharacterStatus.LEVEL_BELOW_MINIMUM;
                    if (request.CharacterGuildId is not null &&
                        request.CharacterGuildId.Contains("%2F"))
                    {
                        guild = await session.LoadAsync<GuildModel>(request.CharacterGuildId);

                        if (guild is null)
                            return CharacterStatus.GUILD_NOT_EXISTS;

                        string parsedCharacterGuildId = parsingMethodForGeneratedId(request.CharacterGuildId);
                        request.CharacterGuildId = parsedCharacterGuildId;
                    }

                    CharacterModel character = new CharacterModel
                    {
                        Name = request.Name,
                        GameClass = request.GameClass,
                        Level = request.Level,
                        CharacterGuildId = request.CharacterGuildId
                    };

                    await session.StoreAsync(character);
                    await session.SaveChangesAsync();
                    return CharacterStatus.SUCCESS;
                }
            }
            catch (RavenException)
            {
                return CharacterStatus.RAVEN_ERROR;
            }
        }

        #endregion

        #region CharacterRead

        public async Task<(CharacterModel?, CharacterStatus)> ReadCharacterAsync(string characterId)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedCharacterId = parsingMethodForGeneratedId(characterId);

                    CharacterModel? foundCharacter = await session.LoadAsync<CharacterModel>(parsedCharacterId);

                    if (foundCharacter is null)
                        return (null, CharacterStatus.CHARACTER_NOT_FOUND);

                    return (foundCharacter, CharacterStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (null, CharacterStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region CharacterUpdate

        public async Task<(CharacterModel?, CharacterStatus)> UpdateCharacterAsync(string id, UpdateCharacterInputDto request)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedCharacterId = parsingMethodForGeneratedId(id);

                    CharacterModel? characterToUpdate = await session.LoadAsync<CharacterModel>(parsedCharacterId);

                    if (characterToUpdate is null)
                        return (null, CharacterStatus.CHARACTER_NOT_FOUND);

                    if (request.NewName is not null)
                        characterToUpdate.Name = request.NewName;
                    if (request.NewGameClass is not null && availableCharacterClasses.Contains(request.NewGameClass))
                        characterToUpdate.GameClass = request.NewGameClass;
                    else
                        return (null, CharacterStatus.INCORRECT_GAMECLASS);
                    if (request.NewLevel is not null)
                        characterToUpdate.Level = request.NewLevel;
                    if (request.NewCharacterGuildId is not null && request.NewCharacterGuildId.Contains("%2F"))
                    {
                        string parsedCharacterGuildId = parsingMethodForGeneratedId(request.NewCharacterGuildId);
                        request.NewCharacterGuildId = parsedCharacterGuildId;
                    }

                    await session.SaveChangesAsync();
                    return (characterToUpdate, CharacterStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (null, CharacterStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region CharacterDelete

        public async Task<CharacterStatus> DeleteCharacterAsync(string characterId)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedCharacterId = parsingMethodForGeneratedId(characterId);

                    CharacterModel? character = await session.LoadAsync<CharacterModel>(parsedCharacterId);

                    if (character is null)
                        return CharacterStatus.CHARACTER_NOT_FOUND;

                    session.Delete<CharacterModel>(character);
                    await session.SaveChangesAsync();

                    return CharacterStatus.SUCCESS;
                }
            }
            catch (RavenException)
            {
                return CharacterStatus.RAVEN_ERROR;
            }
        }

        #endregion

        private string parsingMethodForGeneratedId(string originalId)
        {
            string parsedGuildId = originalId.Replace("%2F", "/");
            return parsedGuildId;
        }

        private List<string> availableCharacterClasses = new List<string>() {
            "Mag",
            "Warrior",
            "Archer",
            "Barbarian",
            "Witch"
        };
    }
}
