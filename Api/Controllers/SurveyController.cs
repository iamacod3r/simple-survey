using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain.Interfaces;

namespace Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SurveyController : ControllerBase
    {
        #region DI
        private readonly ILogger<SurveyController> _logger;
        private readonly ISurveyService _surveyService;
        #endregion
        public SurveyController(ILogger<SurveyController> logger, ISurveyService surveyService)
        {
            _logger = logger;
            _surveyService = surveyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var survey = await _surveyService.GetSurvey(id);
            if (survey != null)
            {
                return Ok(survey);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurvey([FromBody] Model.Input.Survey survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await _surveyService.CreateSurvey(survey);
            return Ok(result);
        }

        [HttpPost("question")]
        public async Task<IActionResult> CreateSurveyQuestion([FromBody] Model.Input.Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await _surveyService.CreateSurveyQuestion(question);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost("questions")]
        public async Task<IActionResult> CreateSurveyQuestions([FromBody] List<Model.Input.Question> questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await _surveyService.CreateSurveyQuestions(questions);
            return Ok(result);
        }

        [HttpPost("questionupdate")]
        public async Task<IActionResult> UpdateSurveyQuestion([FromBody] Model.Input.Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await _surveyService.UpdateQuestion(question);
            return Ok(result);
        }

        [HttpPost("saveanswer")]
        public async Task<IActionResult> SaveAnswer([FromBody] Model.Input.Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await _surveyService.SaveAnswer(answer);
            if (result)
            {
                return Ok(result);
            }
            return NotFound();
        }

    }
}