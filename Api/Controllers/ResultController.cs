using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResultController : ControllerBase
    {
        #region DI
        private readonly ILogger<ResultController> _logger;
        private readonly ISurveyService _surveyService;
        #endregion

        public ResultController(ILogger<ResultController> logger, ISurveyService surveyService)
        {
            _logger = logger;
            _surveyService = surveyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurvey(int id) {

            var result = await _surveyService.GetSurveyResult(id);

            if (result != null) {
                return Ok(result);
            }

            return NotFound();
        }
    }
}