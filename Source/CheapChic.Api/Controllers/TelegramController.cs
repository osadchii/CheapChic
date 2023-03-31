using CheapChic.Infrastructure.Constants;
using CheapChic.Infrastructure.Handlers.Telegram.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace CheapChic.Api.Controllers;

[Route($"{ControllerName.Telegram}/{{token}}")]
public class TelegramController : ApiController
{
    public TelegramController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromRoute] string token, [FromBody] Update update)
    {
        var command = new HandleUpdate.Command(token, update);
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok("Ok");
    }
}