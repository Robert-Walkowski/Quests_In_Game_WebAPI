using QuestsInGame_WebAPI_Project.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuestsInGame_WebAPI_Project.ModelDtos.CharacterDto.Input
{
    public class CreateCharacterInputDto
    {
        public string? Name { get; set; }
        public string? GameClass { get; set; }
        public int Level { get; set; }
        public string? CharacterGuildId { get; set; }
    }
}
