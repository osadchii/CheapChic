using CheapChic.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace CheapChic.Api.Controllers;

[Route($"{ControllerName.Telegram}/{{token}}")]
public class TelegramController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> Update([FromRoute] string token, [FromBody] Update update)
    {
        await Task.Delay(1);
        return Ok();
    }
}