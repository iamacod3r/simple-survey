using Microsoft.AspNetCore.Mvc;
using Survey.Domain.Interfaces;
using System.Threading.Tasks;

namespace Survey.Api.Controllers
{
    [Route("api/result")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly ISurveyService SurveyService;

        public ResultController(ISurveyService surveyService)
        {
            SurveyService = surveyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurvey(int id) {

            var result = await SurveyService.GetSurveyResult(id);

            if (result != null) {
                return Ok(result);
            }

            return NotFound();
        }
    }
}