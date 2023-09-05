using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System.Reflection;

namespace ImplementNlog
{
    public static class Function1
    {
        private static readonly NLog.ILogger _logger;

        static Function1()
        {
            // Load NLog configuration
            string baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(baseDirectory);
            _logger = LogManager.LoadConfiguration("E:\\ImplementNlog\\ImplementNlog\\nlog.config").GetCurrentClassLogger();
        }
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            Microsoft.Extensions.Logging.ILogger log)
        {
            //NLog.ILogger _nlog = LogManager.GetCurrentClassLogger();
            //log.LogInformation("C# HTTP trigger function processed a request.");
            _logger.Info("hello sir ");
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
