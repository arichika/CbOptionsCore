
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using CbOptionsCore;
using Samples.SampleBizLogic;
using Microsoft.Extensions.Logging;

namespace Samples.SampleApp
{
    public static class SampleFunction2WithOptions
    {
        [FunctionName("SampleFunction2WithOptions")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req,
            ILogger logger,
            [CbOptions(sectionKey: "SampleBizLogic:MyBizOptions", reloadOnChange: true)]MyBizOptions options
            )
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");
            logger.LogInformation($"options={options}");

            var (result, methodName) = new SomeFunction(options).SomeMethod();

            logger.LogInformation($"result={result}, methodName={methodName}");

            return options != null
                ? (ActionResult)new OkObjectResult($"result={result}, methodName={methodName}")
                : new BadRequestObjectResult("Please check settings and enviroments.");
        }
    }
}
