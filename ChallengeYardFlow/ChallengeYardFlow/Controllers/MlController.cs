using ChallengeYardFlow.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ChallengeYardFlow.Controllers
{
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class MlController : ControllerBase
    {
        private readonly SentimentService _sentimentService;

        public MlController(SentimentService sentimentService)
        {
            _sentimentService = sentimentService;
        }

        public sealed class SentimentRequest
        {
            public string Text { get; set; } = string.Empty;
        }

        [HttpPost("sentiment")]
        public IActionResult Analyze([FromBody] SentimentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text)) return BadRequest("Texto obrigatório");
            var result = _sentimentService.Predict(request.Text);
            return Ok(new { request.Text, result.Prediction, result.Probability, result.Score });
        }
    }
}


