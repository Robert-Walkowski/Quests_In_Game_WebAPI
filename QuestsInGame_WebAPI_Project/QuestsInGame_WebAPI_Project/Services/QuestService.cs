using QuestsInGame_WebAPI_Project.Database;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Input;
using QuestsInGame_WebAPI_Project.Models;
using QuestsInGame_WebAPI_Project.StaticIndexes;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
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
                    if (string.IsNullOrEmpty(request.QuestTitle))
                        return QuestStatus.EMPTY_QUEST_TITLE;
                    if (request.QuestLevel <= 0)
                        return QuestStatus.INCORRECT_QUEST_LEVEL;

                    QuestModel quest = new QuestModel
                    {
                        QuestTitle = request.QuestTitle,
                        QuestDescription = request.QuestDescription,
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
                        questToUpdate.QuestTitle = request.NewQuestTitle;
                    if (request.NewQuestDescription is not null)
                        questToUpdate.QuestDescription = request.NewQuestDescription;
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

        #region FullTextSearchMethods
        
        public async Task<(List<QuestModel>, QuestStatus)> FTSQuestByTitleOrDescriptionAsync(string searchingTerm)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    List<QuestModel> result = await session.Query<Quests_ByNameAndDescription.Result, Quests_ByNameAndDescription>()
                                                           .Where(x => x.QuestData == searchingTerm)
                                                           .OfType<QuestModel>()
                                                           .ToListAsync();

                    if (result.Count == 0)
                        return (new List<QuestModel>(), QuestStatus.QUEST_LIST_IS_EMPTY);

                    return (result, QuestStatus.SUCCESS);
                }
            }
            catch (RavenException)
            {
                return (new List<QuestModel>(), QuestStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region VectorSearchMethods

        public async Task<(List<QuestModel>, QuestStatus)> VectorSearchQuestsAsync(string query)
        {
            try
            {
                using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
                {
                    List<QuestModel> resultList = await session.Query<Quests_VectorSearch.Result, Quests_VectorSearch>()
                                                               .VectorSearch(
                                                               field => field.WithField(x => x.Vector),
                                                               searchTerm => searchTerm.ByText(query), 0.75f, 20)
                                                               .OrderByScore()
                                                               .OfType<QuestModel>()
                                                               .Take(2)
                                                               .ToListAsync();

                    if (resultList.Count == 0)
                        return (new List<QuestModel>(), QuestStatus.QUEST_NOT_FOUND);

                    return (resultList, QuestStatus.SUCCESS);
                }
            }
            catch (RavenException ex)
            {
                Console.WriteLine(ex.Message);
                return (new List<QuestModel>(), QuestStatus.RAVEN_ERROR);
            }
        }

        #endregion

        #region PatchByQueryMethods

        public async Task<QuestStatus> PatchByQueryUpdateMoreQuestsAsync(int qLevel, int NewLevel)
        {
            try
            {
                await DocumentStoreHolder.Store.Operations.SendAsync(new PatchByQueryOperation(new IndexQuery
                {
                    Query = @"from QuestModels as q
                              where q.QuestLevel < $level
                              update
                              {
                                q.QuestLevel = $NewLevel
                              }",

                    QueryParameters = new Parameters
                    {
                        { "level", qLevel },
                        { "NewLevel", NewLevel }
                    }
                }));

                return QuestStatus.SUCCESS;
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
