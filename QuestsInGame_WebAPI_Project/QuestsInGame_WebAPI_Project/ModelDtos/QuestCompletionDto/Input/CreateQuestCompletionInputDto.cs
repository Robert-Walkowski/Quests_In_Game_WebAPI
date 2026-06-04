namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestCompletionDto.Input
{
    public class CreateQuestCompletionInputDto
    {
        public string CharacterId { get; set; }
        public string QuestId { get; set; }
        public TimeSpan? CompletionTime { get; set; }
        public double? Grade { get; set; }
    }
}
