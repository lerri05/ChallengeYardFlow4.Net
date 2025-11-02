using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeYardFlow.Data;
using ChallengeYardFlow.Modelo;
using ChallengeYardFlow.Services;
using Microsoft.AspNetCore.Authorization;

namespace ChallengeYardFlow.Controllers
{
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class LocacoesController : ControllerBase
    {
        private readonly LocadoraContext _ctx;
        private readonly PricingService _pricingService;
        public LocacoesController(LocadoraContext ctx, PricingService pricingService)
        {
            _ctx = ctx;
            _pricingService = pricingService;
        }

        [HttpPost("calcular")]
        public async Task<IActionResult> Calcular([FromBody] Locacao req)
        {
            var Moto = await _ctx.Motos.FindAsync(req.MotoId);
            if (Moto == null) return NotFound($"Moto {req.MotoId} não encontrado.");

            var subtotal = _pricingService.CalculateTotal(req.DataInicial, req.DataFinal, Moto.ValorDiaria);

            var resp = new
            {
                Moto = Moto.Modelo,
                DataInicial = req.DataInicial,
                DataFinal = req.DataFinal,
                ValorDiaria = Moto.ValorDiaria,
                ValorFinal = subtotal
            };

            return Ok(resp);
        }
    }
}