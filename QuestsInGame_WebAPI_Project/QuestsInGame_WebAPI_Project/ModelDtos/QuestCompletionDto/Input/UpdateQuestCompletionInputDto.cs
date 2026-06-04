namespace QuestsInGame_WebAPI_Project.ModelDtos.QuestCompletionDto.Input
{
    public class UpdateQuestCompletionInputDto
    {
        public TimeSpan? NewCompletionTime { get; set; }
        public double? NewGrade { get; set; }
    }
}
