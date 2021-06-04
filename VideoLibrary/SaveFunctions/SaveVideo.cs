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
using VideoLibrary.Models;
using System.Data;

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
        public async Task<IActionResult> SaveVideoData(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Video>(requestBody);
            //name = name ?? data.Title;

            var str = config.GetSection("ConnectionStrings-VideoLibrary-DB").Value;
            try
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    var text = $"INSERT INTO Videos (Title, CreatedBy, CreatedDate) VALUES (@Title, @CreatedBy, @CreatedDate)";

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        // Execute the command and log the # rows affected.
                        cmd.CommandText = text;
                        cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = data.Title;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = data.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = data.CreatedDate;
                        var rows = await cmd.ExecuteNonQueryAsync();
                        log.LogInformation($"{rows} rows were updated");
                    }
                }

            } catch(Exception ex)
            {
                log.LogError("Error",ex);
            }
            //TODO: send back response
            string responseMessage = $"{data.Title}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
