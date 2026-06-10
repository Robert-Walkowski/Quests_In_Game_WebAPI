using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestsInGame_WebAPI_Project.Enums;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestCompletionDto.Input;
using QuestsInGame_WebAPI_Project.ModelDtos.QuestCompletionDto.Output;
using QuestsInGame_WebAPI_Project.Models;
using QuestsInGame_WebAPI_Project.Services;
using QuestsInGame_WebAPI_Project.StaticIndexes;

namespace QuestsInGame_WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestCompletionController : ControllerBase
    {
        private readonly IQuestCompletionService _questCompletionService;

        public QuestCompletionController(IQuestCompletionService questCompletionService)
        {
            this._questCompletionService = questCompletionService;
        }

        [HttpPost("createQuestCompletion")]
        public async Task<IActionResult> CreateQuestCompletionAsync(CreateQuestCompletionInputDto request)
        {
            QuestCompletionStatusEnum status = await _questCompletionService.CreateQuestCompletionAsync(request);

            CreateQuestCompletionOutputDto response = new CreateQuestCompletionOutputDto
            {
                Status = status,
                Message = status.ToMessage()
            };

            if (status == QuestCompletionStatusEnum.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("readQuestCompletion/{id}")]
        public async Task<IActionResult> ReadQuestCompletionAsync(string id)
        {
            (QuestCompletionModel? questCompletion, QuestCompletionStatusEnum status) = await _questCompletionService.ReadQuestInformationAsync(id);

            ReadQuestCompletionOutputDto response = new ReadQuestCompletionOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                QuestCompletion = questCompletion
            };

            if (status == QuestCompletionStatusEnum.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("IWMRCharactersWithCountOfCompletedQuests")]
        public async Task<IActionResult> IWMRCharactersWithCountOfCompletedQuestsAsync()
        {
            (List<QuestCompletions_ByCharacter.Result> resultList, QuestCompletionStatusEnum status) = await _questCompletionService.IWMRCharactersWithCountOfCompletedQuestsAsync();

            IndexWMRCharactresWithCountOfCompletedQuestsOutputDto response = new IndexWMRCharactresWithCountOfCompletedQuestsOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                ResultList = resultList
            };

            if (status == QuestCompletionStatusEnum.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("updateQuestCompletion/{id}")]
        public async Task<IActionResult> UpdateQuestCompletionAsync(string id, UpdateQuestCompletionInputDto request)
        {
            (QuestCompletionModel? questCompletion, QuestCompletionStatusEnum status) = await _questCompletionService.UpdateQuestInformationAsync(id, request);

            UpdateQuestCompletionOutputDto response = new UpdateQuestCompletionOutputDto
            {
                Status = status,
                Message = status.ToMessage(),
                QuestCompletion = questCompletion
            };

            if (status == QuestCompletionStatusEnum.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("deleteQuestCompletion/{id}")]
        public async Task<IActionResult> DeleteQuestCompletionAsync(string id)
        {
            QuestCompletionStatusEnum status = await _questCompletionService.DeleteQuestCompletionAsync(id);

            DeleteQuestCompletionOutputDto response = new DeleteQuestCompletionOutputDto
            {
                Status = status,
                Message = status.ToMessage()
            };

            if (status == QuestCompletionStatusEnum.SUCCESS)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
