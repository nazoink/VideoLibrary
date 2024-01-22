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
using VideoLibrary.Validators;
using System.Linq;
using VideoLibrary.Repository;

namespace VideoLibrary.Controllers
{
    public class LoadVideoController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IVideoRepo _videoRepo;
        public LoadVideoController(IConfiguration configuration, IVideoRepo videoRepo)
        {
            config = configuration;
            _videoRepo = videoRepo;
        }

        [FunctionName("LoadVideo")]
        public async Task<IActionResult> LoadVideoData(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var videoData = JsonConvert.DeserializeObject<Video>(requestBody);

            var validator = new LoadVideoValidator();
            var validationResult = validator.Validate(videoData);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage;
            try
            {
                var video = await _videoRepo.LoadVideoAsync(videoData.Id.Value);
                responseMessage = $"{video.Id}. This HTTP triggered function executed successfully.";
                return new OkObjectResult(video);
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
