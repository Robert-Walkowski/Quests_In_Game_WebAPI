using QuestsInGame_WebAPI_Project.Database;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestCompletionDto.Input;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Input;
using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Documents;
using Raven.Client.Exceptions;

namespace QuestsInGame_WebAPI_Project.Services
{
    public class QuestCompletionService : IQuestCompletionService
    {
        #region CreateQuestCompletion

        public async Task<QuestCompletionStatusEnum> CreateQuestCompletionAsync(CreateQuestCompletionInputDto request)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    List<CharacterModel> charactersModel = await session.Query<CharacterModel>().ToListAsync();
                    List<QuestModel> questsModel = await session.Query<QuestModel>().ToListAsync();

                    string parsedCharacterId = parsingMethodForGeneratedId(request.CharacterId);
                    string parsedQuestId = parsingMethodForGeneratedId(request.QuestId);

                    CharacterModel? character = await session.LoadAsync<CharacterModel>(parsedCharacterId);
                    QuestModel? quest = await session.LoadAsync<QuestModel>(parsedQuestId);

                    if (string.IsNullOrEmpty(request.CharacterId) && !charactersModel.Contains(character))
                        return QuestCompletionStatusEnum.CHARACTER_NOT_FOUND;
                    if (string.IsNullOrEmpty(request.QuestId) && !questsModel.Contains(quest))
                        return QuestCompletionStatusEnum.QUEST_NOT_FOUND;
                    if (request.Grade < 1 || request.Grade > 10)
                        return QuestCompletionStatusEnum.QUEST_GRADE_TOO_HIGH;

                    QuestCompletionModel questCompletion = new QuestCompletionModel
                    {
                        CharacterId = request.CharacterId,
                        QuestId = request.QuestId,
                        Grade = request.Grade,
                        CompletionTime = request.CompletionTime
                    };

                    quest.Status = QuestCompletionStatus.COMPLETED;

                    await session.StoreAsync(questCompletion);
                    await session.SaveChangesAsync();
                    return QuestCompletionStatusEnum.SUCCESS;
                }
            }
            catch (RavenException)
            {
                return QuestCompletionStatusEnum.RAVEN_ERROR;
            }
        }

        #endregion

        #region ReadQuestCompletion

        public async Task<(QuestCompletionModel?, QuestCompletionStatusEnum)> ReadQuestInformationAsync(string questCompletionId)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedQuestCompletionId = parsingMethodForGeneratedId(questCompletionId);

                    QuestCompletionModel? foundQuest = await session.LoadAsync<QuestCompletionModel>(parsedQuestCompletionId);

                    if (foundQuest is null)
                        return (null, QuestCompletionStatusEnum.QUEST_NOT_FOUND);

                    return (foundQuest, QuestCompletionStatusEnum.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (null, QuestCompletionStatusEnum.RAVEN_ERROR);
            }
        }

        #endregion

        #region UpdateQuestCompletion

        public async Task<(QuestCompletionModel?, QuestCompletionStatusEnum)> UpdateQuestInformationAsync(string questCompletionId, UpdateQuestCompletionInputDto request)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedQuestCompletionId = parsingMethodForGeneratedId(questCompletionId);

                    QuestCompletionModel? questCompletionToUpdate = await session.LoadAsync<QuestCompletionModel>(parsedQuestCompletionId);

                    if (questCompletionToUpdate is null)
                        return (null, QuestCompletionStatusEnum.QUEST_NOT_FOUND);

                    if (request.NewGrade is not null && !(request.NewGrade < 1 || request.NewGrade > 10))
                        questCompletionToUpdate.Grade = request.NewGrade;
                    if (request.NewCompletionTime is not null)
                        questCompletionToUpdate.CompletionTime = request.NewCompletionTime;

                    await session.SaveChangesAsync();
                    return (questCompletionToUpdate, QuestCompletionStatusEnum.SUCCESS);

                }
            }
            catch (RavenException)
            {
                return (null, QuestCompletionStatusEnum.RAVEN_ERROR);
            }
        }

        #endregion

        #region DeleteQuestCompletion

        public async Task<QuestCompletionStatusEnum> DeleteQuestCompletionAsync(string questCompletionId)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedQuestCompletionId = parsingMethodForGeneratedId(questCompletionId);

                    QuestCompletionModel? questCompletionToDelete = await session.LoadAsync<QuestCompletionModel>(parsedQuestCompletionId);

                    string parsedQuestId = parsingMethodForGeneratedId(questCompletionToDelete.QuestId);

                    QuestModel? questToDelete = await session.LoadAsync<QuestModel>(parsedQuestId);

                    if (questCompletionToDelete is null || questToDelete is null)
                        return QuestCompletionStatusEnum.QUEST_NOT_FOUND;

                    session.Delete<QuestCompletionModel>(questCompletionToDelete);
                    session.Delete<QuestModel>(questToDelete);
                    await session.SaveChangesAsync();
                    return QuestCompletionStatusEnum.SUCCESS;
                }
            }
            catch (RavenException)
            {
                return QuestCompletionStatusEnum.RAVEN_ERROR;
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
