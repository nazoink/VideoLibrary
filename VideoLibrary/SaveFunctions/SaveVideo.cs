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

namespace VideoLibrary.SaveFunctions
{
    public class SaveVideo
    {
        private IConfiguration config;
        public SaveVideo(IConfiguration configuration)
        {
            config = configuration;
        }

        [FunctionName("SaveVideo")]
        public async Task<IActionResult> Run(
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

            //TODO: save to the DB
            var str = config.GetSection("ConnectionStrings-VideoLibrary-DB").Value;
            //var str = "Server=tcp:videolibrary.database.windows.net,1433;Initial Catalog=VideoLibrary;Persist Security Info=False;User ID=vipman;Password=v1p!m@n21;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var text = $"INSERT INTO Videos (Title, CreatedBy, CreatedDate) VALUES ('{name}', 'Tim', '{DateTime.Now}')";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were updated");
                }
            }

            //TODO: send back response

            return new OkObjectResult(responseMessage);
        }
    }
}
