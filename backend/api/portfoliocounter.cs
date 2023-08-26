
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;



namespace Company.Function;

    public class portfoliocounter
    {
    private readonly ILogger _logger;

    public portfoliocounter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<GetVisitorCounter>();
    }

         [Function("portfoliocounter")]
    public MyOutputType Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
    [CosmosDBInput(databaseName: "AzurePortfolio", collectionName: 
    "Counter", ConnectionStringSetting = "CosmosDbConnectionString", Id = "1",
            PartitionKey = "1")] Counter counter)
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

