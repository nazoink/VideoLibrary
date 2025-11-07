using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using VideoLibrary.Models;
using VideoLibrary.Repository;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text.Json;
using System.Linq;

namespace VideoLibrary.Controllers
{
    public class LoadVideoController
    {
        private readonly IConfiguration config;
        private readonly IVideoRepo _videoRepo;
        private readonly IValidator<Video> _validator;
        public LoadVideoController(IConfiguration configuration, IVideoRepo videoRepo, IValidator<Video> validator)
        {
            config = configuration;
            _videoRepo = videoRepo;
            _validator = validator;
        }

        [Function("LoadVideo")]
        public async Task<HttpResponseData> LoadVideoData([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("LoadVideo");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var videoData = JsonSerializer.Deserialize<Video>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var result = await HandleLoadVideoAsync(videoData);

            var response = req.CreateResponse(result.Success ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.BadRequest);
            await response.WriteStringAsync(result.Success ? JsonSerializer.Serialize(result.Video) : result.ErrorMessage ?? "");
            return response;
        }

        // Testable core logic
        public async Task<(bool Success, Video Video, string ErrorMessage)> HandleLoadVideoAsync(Video videoData)
        {
            if (videoData == null || videoData.Id == null)
                return (false, null, "Invalid payload");

            var validationResult = await _validator.ValidateAsync(videoData);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(';', validationResult.Errors.Select(e => e.ErrorMessage));
                return (false, null, errors);
            }

            try
            {
                var video = await _videoRepo.LoadVideoAsync(videoData.Id.Value);
                return (true, video, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}
