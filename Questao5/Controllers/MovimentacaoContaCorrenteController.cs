using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;

namespace Questao5.Controllers
{
    /// <summary>
    /// API de Movimentações da Conta Corrente
    /// </summary>
    [ApiController]
    [Route("api/movimentacaocontacorrente")]
    public class MovimentacaoContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentacaoContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza movimentações da conta corrente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("movimentacao")]
        public async Task<IActionResult> MovimentarContaCorrente([FromBody] MovimentacaoContaCorrenteRequest request)
        {
            try
            {
                var resultado = await _mediator.Send(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Tipo = "ERROR", Mensagem = ex.Message });
            }
        }
    }

}
