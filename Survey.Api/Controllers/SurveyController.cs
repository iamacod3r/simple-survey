using Microsoft.AspNetCore.Mvc;
using Survey.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Survey.Api.Controllers
{
    [Route("api/survey")]
    [ApiController]
    public class SurveyController : Controller
    {
        private readonly ISurveyService SurveyService;
        public SurveyController(ISurveyService surveyService)
        {
            SurveyService = surveyService;
        }

        // GET api/survey/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurvey(int id)
        {

            var survey = await SurveyService.GetSurvey(id);
            if (survey != null)
            {
                return Ok(survey);
            }

            return NotFound();
        }

        // POST api/survey
        [HttpPost]
        public async Task<IActionResult> CreateSurvey([FromBody] Model.Input.Survey survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await SurveyService.CreateSurvey(survey);
            return Ok(result);
        }

        // POST api/survey/question
        [HttpPost("question")]
        public async Task<IActionResult> CreateSurveyQuestion([FromBody] Model.Input.Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await SurveyService.CreateSurveyQuestion(question);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        // POST api/survey/questions
        [HttpPost("questions")]
        public async Task<IActionResult> CreateSurveyQuestions([FromBody] List<Model.Input.Question> questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await SurveyService.CreateSurveyQuestions(questions);
            return Ok(result);
        }

        // POST api/survey/questionupdate
        [HttpPost("questionupdate")]
        public async Task<IActionResult> UpdateSurveyQuestion([FromBody] Model.Input.Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await SurveyService.UpdateQuestion(question);
            return Ok(result);
        }

        [HttpPost("saveanswer")]
        public async Task<IActionResult> SaveAnswer([FromBody] Model.Input.Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToString());
            }

            var result = await SurveyService.SaveAnswer(answer);
            if (result)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}