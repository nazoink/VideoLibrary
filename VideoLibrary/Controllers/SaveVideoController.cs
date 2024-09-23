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
using VideoLibrary.Validators;
using System.Linq;
using VideoLibrary.Repository;
using FluentValidation;

namespace VideoLibrary.Controllers
{
    public class SaveVideoController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IVideoRepo _videoRepo;
        private readonly IValidator<Video> _videoValidator;
        public SaveVideoController(IConfiguration configuration, IVideoRepo videoRepo, IValidator videoValidator)
        {
            config = configuration;
            _videoRepo = videoRepo;
            _videoValidator = (IValidator<Video>)videoValidator;
        }

        //[HttpPost]
        //public async Task<IActionResult> Save([FromHeader] Video videoData)
        //{
        //    //log.LogInformation("C# HTTP trigger function processed a request.");

        //    var str = config.GetSection("ConnectionStrings-VideoLibrary-DB").Value;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(str))
        //        {
        //            conn.Open();
        //            var text = $"INSERT INTO Videos (Title, CreatedBy, CreatedDate) VALUES (@Title, @CreatedBy, @CreatedDate)";

        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                // Execute the command and log the # rows affected.
        //                cmd.CommandText = text;
        //                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = videoData.Title;
        //                cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = videoData.CreatedBy;
        //                cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = videoData.CreatedDate;
        //                var rows = await cmd.ExecuteNonQueryAsync();
        //                //log.LogInformation($"{rows} rows were updated");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //log.LogError("Error", ex);
        //    }
        //    //TODO: send back response
        //    string responseMessage = $"{videoData.Title}. This HTTP triggered function executed successfully.";

        //    return new OkObjectResult(responseMessage);
        //}

        [FunctionName("SaveVideo")]
        public async Task<IActionResult> SaveVideoData(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var videoData = JsonConvert.DeserializeObject<Video>(requestBody);

            var validator = new SaveVideoValidator();
            var validationResult = _videoValidator.Validate(videoData);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => new {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage;
            try
            {
                var video = await _videoRepo.SaveVideoAsync(videoData);
                responseMessage = $"{video.Id}. This HTTP triggered function executed successfully.";
                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                responseMessage = $"{videoData.Id}. This HTTP triggered function failed";
                log.LogError(responseMessage, ex);
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
