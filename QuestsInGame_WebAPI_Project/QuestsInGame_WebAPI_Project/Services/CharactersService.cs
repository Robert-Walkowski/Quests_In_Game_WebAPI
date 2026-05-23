using Microsoft.AspNetCore.Mvc;
using QuestsInGame_WebAPI_Project.Database;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Input;
using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Exceptions;

namespace QuestsInGame_WebAPI_Project.Services
{
    public class CharactersService
    {
        #region Character Create
        public async Task<CharacterStatus> CreateCharacterAsync(CreateCharacterInputDto request)
        {
			try
			{
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
				{
                    if (string.IsNullOrEmpty(request.Name))
                        return CharacterStatus.EMPTY_NAME;
                    if (string.IsNullOrEmpty(request.GameClass))
                        return CharacterStatus.EMPTY_GAMECLASS;
                    if (request.Level < 0)
                        return CharacterStatus.LEVEL_BELOW_MINIMUM;

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

        #region Character Read

		public async Task<(CharacterModel?, CharacterStatus)> ReadCharacterAsync(string characterId)
		{
			try
			{
				using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
				{
					string parsedCharacterId = characterId.Replace("%2F", "/");

					CharacterModel? foundCharacter = await session.LoadAsync<CharacterModel>(parsedCharacterId);

					if (foundCharacter is null)
						return (null, CharacterStatus.SUCCESS);

					return (foundCharacter, CharacterStatus.SUCCESS);
				}
			}
			catch (RavenException)
			{
				return (null, CharacterStatus.RAVEN_ERROR);
			}
		}

        #endregion

        #region Character Update

		/*public async Task<(CharacterModel, CharacterStatus)> UpdateCharacterAsync()
		{
			
		}*/

        #endregion

        #region Character Delete

        public async Task<CharacterStatus> DeleteCharacterAsync(string characterId)
        {
			try
			{
				using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
				{
					string parsedCharacterId = characterId.Replace("%2F", "/");

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
    }
}
