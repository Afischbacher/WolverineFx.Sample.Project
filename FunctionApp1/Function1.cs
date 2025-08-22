using ClassLibrary1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace FunctionApp1;

public class Function1(ILogger<Function1> logger, IMessageBus messageBus)
{
    private readonly ILogger<Function1> _logger = logger;

    [Function(nameof(Function1))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var result = await messageBus.InvokeAsync<string>(new Command { Message = "Hello from Function1!" });
        return new OkObjectResult(result);
    }
}