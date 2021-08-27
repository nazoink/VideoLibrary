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
using System.Data;
using VideoLibrary.Models;
using System.Collections.Generic;

namespace VideoLibrary.Controllers
{
    public class LoadVideoController
    {
        private readonly IConfiguration config;
        public LoadVideoController(IConfiguration configuration)
        {
            config = configuration;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get([FromHeader] Video inputVideo, ILogger log)
        //{
        //    //get the video we have stored in out data.
        //    log.LogInformation("C# HTTP trigger function processed a request.");


        //    var str = config.GetSection("ConnectionStrings-VideoLibrary-DB").Value;
        //    using (SqlConnection conn = new SqlConnection(str))
        //    {
        //        conn.Open();
        //        var text = $"SELECT Title FROM Videos WHERE Title = @Title";

        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            // Execute the command and log the # rows affected.
        //            cmd.CommandText = text;
        //            cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = inputVideo.Title;
        //            var rows = await cmd.ExecuteReaderAsync();
        //            log.LogInformation($"{rows} rows were returned");
        //        }
        //    }
        //    string responseMessage = $"{inputVideo.Title}. This HTTP triggered function executed successfully.";

        //    return new OkObjectResult(responseMessage);
        //}

        //[HttpPost]
        //public async Task<ActionResult<List<Video>>> Search([FromHeader] Video searchQuery, ILogger log)
        //{
        //    //get the video we have stored in out data.
        //    log.LogInformation("C# HTTP trigger function processed a request.");


        //    var str = config.GetSection("ConnectionStrings-VideoLibrary-DB").Value;
        //    using (SqlConnection conn = new SqlConnection(str))
        //    {
        //        conn.Open();
        //        var text = $"SELECT Title FROM Videos WHERE Title = @Title";

        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            // Execute the command and log the # rows affected.
        //            cmd.CommandText = text;
        //            cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = searchQuery.Title;
        //            var rows = await cmd.ExecuteReaderAsync();
        //            log.LogInformation($"{rows} rows were returned");
        //        }
        //    }
        //    string responseMessage = $"{searchQuery.Title}. This HTTP triggered function executed successfully.";

        //    return new OkObjectResult(responseMessage);
        //}

        [FunctionName("LoadVideo")]
        public async Task<IActionResult> LoadVideoData(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var videoData = JsonConvert.DeserializeObject<Video>(requestBody);

            //var validator = new LoadVideoValidator();
            //var validationResult = validator.Validate(videoData);

            //if (!validationResult.IsValid)
            //{
            //    return new BadRequestObjectResult(validationResult.Errors.Select(e => new {
            //        Field = e.PropertyName,
            //        Error = e.ErrorMessage
            //    }));
            //}

            log.LogInformation("C# HTTP trigger function processed a request.");

            var str = config.GetSection("ConnectionStrings-VideoLibrary-DB").Value;
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var text = $"SELECT Title FROM Videos WHERE Title = @Title";

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Execute the command and log the # rows affected.
                    cmd.CommandText = text;
                    cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = videoData.Title;
                    var rows = await cmd.ExecuteReaderAsync();
                    log.LogInformation($"{rows} rows were returned");
                }
            }
            string responseMessage = $"{videoData.Title}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
