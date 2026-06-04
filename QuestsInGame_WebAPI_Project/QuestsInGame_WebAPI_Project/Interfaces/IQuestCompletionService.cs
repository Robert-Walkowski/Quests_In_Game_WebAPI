using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestCompletionDto.Input;
using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.Interfaces
{
    public interface IQuestCompletionService
    {
        Task<QuestCompletionStatusEnum> CreateQuestCompletionAsync(CreateQuestCompletionInputDto request);
        Task<QuestCompletionStatusEnum> DeleteQuestCompletionAsync(string questCompletionId);
        Task<(QuestCompletionModel?, QuestCompletionStatusEnum)> ReadQuestInformationAsync(string questCompletionId);
        Task<(QuestCompletionModel?, QuestCompletionStatusEnum)> UpdateQuestInformationAsync(string questCompletionId, UpdateQuestCompletionInputDto request);
    }
}