using QuestsInGame_WebAPI_Project.Database;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Input;
using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Exceptions;

namespace QuestsInGame_WebAPI_Project.Services
{
    public class QuestService : IQuestService
    {
        #region CreateQuest

        public async Task<QuestStatus> CreateQuestAsync(CreateQuestInputDto request)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    if (string.IsNullOrEmpty(request.Title))
                        return QuestStatus.EMPTY_QUEST_TITLE;
                    if (request.QuestLevel <= 0)
                        return QuestStatus.INCORRECT_QUEST_LEVEL;

                    QuestModel quest = new QuestModel
                    {
                        Title = request.Title,
                        Description = request.Description,
                        QuestLevel = request.QuestLevel,
                        Reward = request.Reward,
                        Status = QuestCompletionStatus.AVAILABLE
                    };

                    await session.StoreAsync(quest);
                    await session.SaveChangesAsync();
                    return QuestStatus.SUCCESS;
                }
            }
            catch (RavenException)
            {
                return QuestStatus.RAVEN_ERROR;
            }
        }

        #endregion

        #region ReadQuest

        public async Task<(QuestModel?, QuestStatus)> ReadQuestInformationAsync(string questId)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedQuestId = parsingMethodForGeneratedId(questId);

                    QuestModel? foundQuest = await session.LoadAsync<QuestModel>(parsedQuestId);

                    if (foundQuest is null)
                        return (null, QuestStatus.QUEST_NOT_FOUND);

                    return (foundQuest, QuestStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (null, QuestStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region UpdateQuest

        public async Task<(QuestModel?, QuestStatus)> UpdateQuestInformationAsync(string questId, UpdateQuestInputDto request)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedQuestId = parsingMethodForGeneratedId(questId);

                    QuestModel? questToUpdate = await session.LoadAsync<QuestModel>(parsedQuestId);

                    if (questToUpdate is null)
                        return (null, QuestStatus.QUEST_NOT_FOUND);

                    if (request.NewQuestTitle is not null)
                        questToUpdate.Title = request.NewQuestTitle;
                    if (request.NewDescription is not null)
                        questToUpdate.Description = request.NewDescription;
                    if (request.NewQuestLevel > 0)
                        questToUpdate.QuestLevel = request.NewQuestLevel;
                    if (request.NewQuestReward is not null)
                        questToUpdate.Reward = request.NewQuestReward;
                    if (request.NewStatus != QuestCompletionStatus.AVAILABLE)
                        questToUpdate.Status = request.NewStatus;

                    await session.SaveChangesAsync();
                    return (questToUpdate, QuestStatus.SUCCESS);

                }
            }
            catch (RavenException)
            {
                return (null, QuestStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region DeleteQuest

        public async Task<QuestStatus> DeleteQuestAsync(string questId)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    string parsedQuestId = parsingMethodForGeneratedId(questId);

                    QuestModel? questToDelete = await session.LoadAsync<QuestModel>(parsedQuestId);

                    if (questToDelete is null)
                        return QuestStatus.QUEST_NOT_FOUND;

                    session.Delete<QuestModel>(questToDelete);
                    await session.SaveChangesAsync();
                    return QuestStatus.SUCCESS;
                }
            }
            catch (RavenException)
            {
                return QuestStatus.RAVEN_ERROR;
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
