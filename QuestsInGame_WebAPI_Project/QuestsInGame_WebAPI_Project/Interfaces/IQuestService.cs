using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Input;
using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.Interfaces
{
    public interface IQuestService
    {
        Task<QuestStatus> CreateQuestAsync(CreateQuestInputDto request);
        Task<QuestStatus> DeleteQuestAsync(string questId);
        Task<(QuestModel?, QuestStatus)> ReadQuestInformationAsync(string questId);
        Task<(QuestModel?, QuestStatus)> UpdateQuestInformationAsync(string questId, UpdateQuestInputDto request);
        Task<(List<QuestModel>, QuestStatus)> FTSQuestByTitleOrDescriptionAsync(string searchingTerm);
        Task<(List<QuestModel>, QuestStatus)> VectorSearchQuestsAsync(string query);
        Task<QuestStatus> PatchByQueryUpdateMoreQuestsAsync(int qLevel, int NewLevel);
    }
}