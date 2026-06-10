using Microsoft.AspNetCore.Identity.Data;
using QuestsInGame_WebAPI_Project.Database;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.GuildDto.Input;
using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Exceptions;

namespace QuestsInGame_WebAPI_Project.Services
{
    public class GuildsService : IGuildsService
    {
        #region GuildCreate

        public async Task<GuildStatus> CreateGuildAsync(CreateGuildInputDto request)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    if (request.GuildName is null)
                        return GuildStatus.EMPTY_GUILD_NAME;

                    GuildModel guild = new GuildModel
                    {
                        GuildName = request.GuildName,
                        GuildDescription = request.GuildDescription,
                        MembersId = request.MembersId
                    };

                    await session.StoreAsync(guild);

                    if (request.MembersId is not null)
                    {
                        foreach (string memberId in request.MembersId)
                        {
                            CharacterModel character = await session.LoadAsync<CharacterModel>(memberId);
                            character.CharacterGuildId = guild.Id;
                        }
                    }

                    await session.SaveChangesAsync();
                    return GuildStatus.SUCCESS;
                }
            }
            catch (RavenException)
            {
                return GuildStatus.RAVEN_ERROR;
            }
        }

        #endregion

        #region GuildRead

        public async Task<(GuildModel?, GuildStatus)> ReadGuildAsync(string guildId)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedGuildId = parsingMethodForGeneratedId(guildId);

                    GuildModel? foundGuild = await session.LoadAsync<GuildModel>(parsedGuildId);

                    if (foundGuild is null)
                        return (null, GuildStatus.GUILD_NOT_FOUND);

                    return (foundGuild, GuildStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (null, GuildStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region GuildUpdate

        public async Task<(GuildModel?, GuildStatus)> UpdateGuildDataAsync(string guildId, UpdateGuildInputDto request)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedStringId = parsingMethodForGeneratedId(guildId);

                    GuildModel? guildToUpdate = await session.LoadAsync<GuildModel>(parsedStringId);

                    if (guildToUpdate is null)
                        return (null, GuildStatus.GUILD_NOT_FOUND);
                    if (request.NewGuildName is not null)
                        guildToUpdate.GuildName = request.NewGuildName;
                    if (request.NewGuildDescription is not null)
                        guildToUpdate.GuildDescription = request.NewGuildDescription;
                    if (guildToUpdate.MembersId is null && request.NewMembersId is not null)
                    {
                        guildToUpdate.MembersId = new List<string>();

                        foreach (string memberId in request.NewMembersId)
                        {
                            CharacterModel character = await session.LoadAsync<CharacterModel>(memberId);
                            character.CharacterGuildId = guildToUpdate.Id;
                            guildToUpdate.MembersId.Add(character.Id);
                        }
                    }
                    else if (guildToUpdate.MembersId is not null && request.NewMembersId is not null)
                    {
                        foreach (string memberId in request.NewMembersId)
                        {
                            CharacterModel character = await session.LoadAsync<CharacterModel>(memberId);
                            character.CharacterGuildId = guildToUpdate.Id;
                            guildToUpdate.MembersId.Add(memberId);
                        }
                    }

                    await session.SaveChangesAsync();
                    return (guildToUpdate, GuildStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (null, GuildStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region GuildDelete

        public async Task<GuildStatus> DeleteGuildAsync(string guildId)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedGuildId = parsingMethodForGeneratedId(guildId);

                    GuildModel? guildToDelete = await session.LoadAsync<GuildModel>(parsedGuildId);

                    if (guildToDelete is null)
                        return GuildStatus.GUILD_NOT_FOUND;

                    if (guildToDelete.MembersId is not null)
                    {
                        foreach (string memberId in guildToDelete.MembersId)
                        {
                            CharacterModel character = await session.LoadAsync<CharacterModel>(memberId);
                            character.CharacterGuildId = null;
                        }
                    }

                    session.Delete<GuildModel>(guildToDelete);
                    await session.SaveChangesAsync();
                    return GuildStatus.SUCCESS;
                }
            }
            catch (RavenException)
            {
                return GuildStatus.RAVEN_ERROR;
            }
        }

        #endregion

        private string parsingMethodForGeneratedId(string originalId)
        {
            string parsedGuildId = originalId.Replace("%2F", "/");
            return parsedGuildId;
        }
    }
}
