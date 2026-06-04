using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Input;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Output;
using QuestsInGame_WebAPI_Project.Models;

namespace QuestsInGame_WebAPI_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly IQuestService _questService;

        public QuestController(IQuestService questService)
        {
            this._questService = questService;
        }

        [HttpPost("createQuest")]
        public async Task<IActionResult> CreateQuestAsync(CreateQuestInputDto request)
        {
            QuestStatus status = await _questService.CreateQuestAsync(request);

            CreateQuestOutputDto response = new CreateQuestOutputDto
            {
                Status = status,
                Message = status.ToMessage()
            };

            if (status == QuestStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("readQuest/{id}")]
        public async Task<IActionResult> ReadQuestAsync(string id)
        {
            (QuestModel? readQuest, QuestStatus status) = await _questService.ReadQuestInformationAsync(id);

            ReadQuestOutputDto response = new ReadQuestOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                QuestModel = readQuest
            };

            if (status == QuestStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("updateQuest/{id}")]
        public async Task<IActionResult> UpdateQuestInformationAsync(string id, UpdateQuestInputDto request)
        {
            (QuestModel? updatedQuest, QuestStatus status) = await _questService.UpdateQuestInformationAsync(id, request);

            UpdateQuestOutputDto response = new UpdateQuestOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                QuestModel = updatedQuest
            };

            if (status == QuestStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("deleteQuest/{id}")]
        public async Task<IActionResult> DeleteQuestAsync(string id)
        {
            QuestStatus status = await _questService.DeleteQuestAsync(id);

            DeleteQuestOutputDto response = new DeleteQuestOutputDto
            {
                Status = status,
                Message = status.ToMessage()
            };

            if (status == QuestStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }
    }
}

