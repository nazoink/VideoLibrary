using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using VideoLibrary.Models;
using VideoLibrary.Repository;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace VideoLibrary.Controllers
{
    public class SaveVideoController
    {
        private readonly IConfiguration config;
        private readonly IVideoRepo _videoRepo;
        private readonly IValidator<Video> _videoValidator;
        public SaveVideoController(IConfiguration configuration, IVideoRepo videoRepo, IValidator<Video> videoValidator)
        {
            config = configuration;
            _videoRepo = videoRepo;
            _videoValidator = videoValidator;
        }

        [Function("SaveVideo")]
        public async Task<HttpResponseData> SaveVideoData([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
         FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("SaveVideo");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var videoData = JsonConvert.DeserializeObject<Video>(requestBody);

            var result = await HandleSaveVideoAsync(videoData);

            if (!result.Success)
            {
                var response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await response.WriteStringAsync(result.ErrorMessage ?? "");
                return response;
            }

            var ok = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await ok.WriteStringAsync($"{result.Video.Id}. This HTTP triggered function executed successfully.");
            return ok;
        }

        // Testable core logic
        public async Task<(bool Success, Video Video, string ErrorMessage)> HandleSaveVideoAsync(Video videoData)
        {
            if (videoData == null)
                return (false, null, "Invalid payload");

            var validationResult = _videoValidator.Validate(videoData);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(';', validationResult.Errors);
                return (false, null, errors);
            }

            try
            {
                var video = await _videoRepo.SaveVideoAsync(videoData);
                return (true, video, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}
