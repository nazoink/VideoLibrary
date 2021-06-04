using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace VideoLibrary.LoadFunctions
{
    public class LoadVideo
    {
        private IConfiguration config;
        public LoadVideo(IConfiguration configuration)
        {
            config = configuration;
        }

        [FunctionName("LoadVideo")]
        public async Task<IActionResult> LoadVideoData(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            var str = config.GetSection("ConnectionStrings-VideoLibrary-DB").Value;
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var text = $"SELECT Title FROM Videos WHERE Title = '{name}'";

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Execute the command and log the # rows affected.
                    cmd.CommandText = text;
                    var rows = await cmd.ExecuteReaderAsync();
                    log.LogInformation($"{rows} rows were returned");
                }
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
