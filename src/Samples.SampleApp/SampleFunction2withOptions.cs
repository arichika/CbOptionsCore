
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using CbOptionsCore;
using Samples.SampleBizLogic;

namespace Samples.SampleApp
{
    public static class SampleFunction2WithOptions
    {
        [FunctionName("SampleFunction2WithOptions")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            TraceWriter log,
            [CbOptions(sectionKey: "SampleBizLogic:MyBizOptions", reloadOnChange: true)]MyBizOptions options
            )
        {
            log.Info("C# HTTP trigger function processed a request.");
            log.Info($"options={options}");

            var (result, methodName) = new SomeFunction(options).SomeMethod();

            log.Info($"result={result}, methodName={methodName}");

            return options != null
                ? (ActionResult)new OkObjectResult($"result={result}, methodName={methodName}")
                : new BadRequestObjectResult("Please check settings and enviroments.");
        }
    }
}
