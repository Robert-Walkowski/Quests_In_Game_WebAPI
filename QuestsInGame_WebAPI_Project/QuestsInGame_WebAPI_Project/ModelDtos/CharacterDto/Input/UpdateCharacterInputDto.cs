namespace QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Input
{
    public class UpdateCharacterInputDto
    {
        public string? NewName { get; set; }
        public string? NewGameClass { get; set; }
        public int NewLevel { get; set; }
        public string? NewCharacterGuildId { get; set; }
    }
}
