using Microsoft.AspNetCore.Mvc;
using QuestsInGame_WebAPI_Project.Controllers;
using QuestsInGame_WebAPI_Project.Database;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Input;
using QuestsInGame_WebAPI_Project.Models;
using QuestsInGame_WebAPI_Project.StaticIndexes;
using Raven.Client.Documents;
using Raven.Client.Documents.Changes;
using Raven.Client.Exceptions;
using System.Collections;

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
                    if (request.CharacterGuildId is not null)
                    {
                        string parsedCharacterGuildId = parsingMethodForGeneratedId(request.CharacterGuildId);
                        request.CharacterGuildId = parsedCharacterGuildId;

                        guild = await session.LoadAsync<GuildModel>(request.CharacterGuildId);

                        if (guild is null)
                            return CharacterStatus.GUILD_NOT_EXISTS;
                    }

                    CharacterModel character = new CharacterModel
                    {
                        Name = request.Name,
                        GameClass = request.GameClass,
                        Level = request.Level,
                        CharacterGuildId = request.CharacterGuildId
                    };

                    await session.StoreAsync(character);

                    if (guild is not null)
                    {
                        if (guild.MembersId is null)
                        {
                            guild.MembersId = new List<string>();
                            guild.MembersId.Add(character.Id);
                        }
                        else
                        {
                            guild.MembersId.Add(character.Id);
                        }
                    }
                    
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
                    if (request.NewGameClass is not null && !availableCharacterClasses.Contains(request.NewGameClass))
                        return (null, CharacterStatus.INCORRECT_GAMECLASS);
                    if (request.NewGameClass is not null && availableCharacterClasses.Contains(request.NewGameClass))
                        characterToUpdate.GameClass = request.NewGameClass;
                    if (request.NewLevel > 0)
                        characterToUpdate.Level = request.NewLevel;
                    if (request.NewCharacterGuildId is not null && request.NewCharacterGuildId.Contains("%2F"))
                    {
                        string parsedCharacterGuildId = parsingMethodForGeneratedId(request.NewCharacterGuildId);
                        request.NewCharacterGuildId = parsedCharacterGuildId;
                        characterToUpdate.CharacterGuildId = request.NewCharacterGuildId;

                        GuildModel? guild = await session.LoadAsync<GuildModel>(request.NewCharacterGuildId);

                        if (guild.MembersId is not null)
                            guild.MembersId.Add(characterToUpdate.Id);
                        else
                        {
                            guild.MembersId = new List<string>();
                            guild.MembersId.Add(characterToUpdate.Id);
                        }
                    }
                    else if (request.NewCharacterGuildId is not null && !request.NewCharacterGuildId.Contains("%2F"))
                    {
                        characterToUpdate.CharacterGuildId = request.NewCharacterGuildId;

                        GuildModel? guild = await session.LoadAsync<GuildModel>(request.NewCharacterGuildId);

                        if (guild.MembersId is not null)
                            guild.MembersId.Add(characterToUpdate.Id);
                        else
                        {
                            guild.MembersId = new List<string>();
                            guild.MembersId.Add(characterToUpdate.Id);
                        }
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

        #region AutoIndexesMethods

        public async Task<(List<CharacterModel>, CharacterStatus)> AutoIndexCharactersWithLevel10OrMoreAndWarriorClassAsync()
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    List<CharacterModel> result = await session.Query<CharacterModel>()
                                                                .Where(c => c.GameClass == "Warrior" && c.Level >= 10)
                                                                .ToListAsync();
                    if (result.Count == 0)
                        return (new List<CharacterModel>(), CharacterStatus.AUTO_INDEX_FAIL);

                    return (result, CharacterStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (new List<CharacterModel>(), CharacterStatus.RAVEN_ERROR);
            }
        }

        public async Task<(List<CharacterModel>, CharacterStatus)> AutoIndexCharactersWithGuildOnlyAsync()
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    List<CharacterModel> result = await session.Query<CharacterModel>()
                                                               .Where(c => c.CharacterGuildId != null)
                                                               .ToListAsync();
                    
                    if (result.Count == 0)
                        return (new List<CharacterModel>(), CharacterStatus.AUTO_INDEX_FAIL);

                    return (result, CharacterStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (new List<CharacterModel>(), CharacterStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region StaticIndexesMethods

        public async Task<(List<Characters_WithGuildName.Result>, CharacterStatus)> StaticIndexCharactersWithGuildNameWithGuildAsync()
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    List<Characters_WithGuildName.Result> result = await session
                                                                        .Query<Characters_WithGuildName.Result, Characters_WithGuildName>()
                                                                        .Where(r => r.GuildName != "Brak gildii")
                                                                        .ProjectInto<Characters_WithGuildName.Result>()
                                                                        .ToListAsync();

                    if (result.Count == 0)
                        return (new List<Characters_WithGuildName.Result>(), CharacterStatus.CHARACTER_NOT_FOUND);

                    return (result, CharacterStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (new List<Characters_WithGuildName.Result>(), CharacterStatus.RAVEN_ERROR);
            }
        }

        public async Task<(List<Characters_ByExpirienceScore.Result>, CharacterStatus)> StaticIndexCharacterByExpirienceScoreWithGoldTierAndClassWarriorAsync()
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    List<Characters_ByExpirienceScore.Result> result = await session.Query<Characters_ByExpirienceScore.Result, Characters_ByExpirienceScore>()
                                                                                    .Where(r => r.Tier == "Gold" && r.GameClass == "Warrior")
                                                                                    .ProjectInto<Characters_ByExpirienceScore.Result>()
                                                                                    .ToListAsync();

                    if (result.Count == 0)
                        return (new List<Characters_ByExpirienceScore.Result>(), CharacterStatus.CHARACTER_NOT_FOUND);

                    return (result, CharacterStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (new List<Characters_ByExpirienceScore.Result>(), CharacterStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region PagingMethods

        public async Task<(List<CharacterModel>, CharacterStatus)> SkipFourFirstCharactersAndTakeTwoAsync()
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    List<CharacterModel> result = await session.Query<CharacterModel>()
                                                               .Skip(4)
                                                               .Take(2)
                                                               .ToListAsync();

                    if (result.Count == 0)
                        return (new List<CharacterModel>(), CharacterStatus.CHARACTER_NOT_FOUND);

                    return (result, CharacterStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (new List<CharacterModel>(), CharacterStatus.RAVEN_ERROR);
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
