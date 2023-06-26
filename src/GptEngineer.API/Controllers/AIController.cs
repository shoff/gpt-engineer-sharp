namespace GptEngineer.API.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/v1/ai")]
public class AIController : ControllerBase
{
    [HttpGet]
    public async IAsyncEnumerable<int> Get()
    {
        for (var i = 0; i < 100; i++)
        {
            await Task.Delay(1000);
            yield return i;
        }
    }
}