using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class GetPortfolioCounter
{
    private readonly ILogger _logger;

    public GetPortfolioCounter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<GetPortfolioCounter>();
    }

    [Function("GetPortfolioCounter")]
    public MyOutputType Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
    [CosmosDBInput(databaseName: "AzurePorfolioCounter", collectionName: 
    "Counter", ConnectionStringSetting = "CosmosDbConnectionString", Id = "index",
            PartitionKey = "index")] Counter counter)
    {

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        string jsonString = JsonSerializer.Serialize(counter);
        response.WriteString(jsonString);
        counter.Count =+ counter.Count+1;
        return new MyOutputType()
        {
            UpdatedCounter = counter,
            HttpResponse = response
        };
    }
}
