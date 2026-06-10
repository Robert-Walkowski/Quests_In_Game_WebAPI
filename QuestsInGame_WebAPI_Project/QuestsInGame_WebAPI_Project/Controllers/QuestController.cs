using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Input;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestDto.Output;
using QuestsInGame_WebAPI_Project.Models;
using System.Reflection.Metadata.Ecma335;

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

        [HttpGet("FTSQuestByTitleOrDescription")]
        public async Task<IActionResult> FTSQuestByTitleOrDescriptionAsync([FromQuery] FTSQuestByTitleAndDescriptionInputDto request)
        {
            (List<QuestModel> resultList, QuestStatus status) = await _questService.FTSQuestByTitleOrDescriptionAsync(request.searchingTerm);

            FTSQuestByTitleAndDescriptionOutputDto response = new FTSQuestByTitleAndDescriptionOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                ResultList = resultList
            };

            if (status == QuestStatus.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("VectorSearchQuests")]
        public async Task<IActionResult> VectorSearchQuestsAsync([FromQuery] string query)
        {
            (List<QuestModel> resultList, QuestStatus status) = await _questService.VectorSearchQuestsAsync(query);

            VectorSearchQuestsOutputDto response = new VectorSearchQuestsOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                ResultList = resultList
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

        [HttpPut("patchByQueryNewLevelQuests")]
        public async Task<IActionResult> PatchByQueryNewLevelQuestsAsync([FromQuery] PatchByQueryNewLevelForQuestsInputDto request)
        {
            QuestStatus status = await _questService.PatchByQueryUpdateMoreQuestsAsync(request.qLevel, request.NewLevel);

            PatchByQueryNewLevelForQuestsOutputDto response = new PatchByQueryNewLevelForQuestsOutputDto
            {
                Status = status,
                Message = status.ToMessage()
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

