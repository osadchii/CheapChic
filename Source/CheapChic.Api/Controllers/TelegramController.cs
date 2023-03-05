using Microsoft.AspNetCore.Mvc;

namespace CheapChic.Api.Controllers;

[Route("telegram")]
public class TelegramController : ApiController
{
    // TODO: Remove it
    [HttpGet]
    public async Task<IActionResult> FirstAction()
    {
        await Task.Delay(1);
        return Ok("First action");
    }
}