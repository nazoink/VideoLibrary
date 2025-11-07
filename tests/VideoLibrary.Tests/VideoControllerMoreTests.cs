using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using VideoLibrary.Controllers;
using VideoLibrary.Models;
using VideoLibrary.Repository;
using Microsoft.Extensions.Configuration;

namespace VideoLibrary.Tests
{
 public class VideoControllerMoreTests
 {
 private readonly Mock<IVideoRepo> _repoMock = new Mock<IVideoRepo>();
 private readonly Mock<IValidator<Video>> _validatorMock = new Mock<IValidator<Video>>();
 private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();

 [Fact]
 public async Task HandleSaveVideoAsync_Returns_Bad_When_Validator_Fails()
 {
 var video = new Video { Title = "" };
 var failures = new[] { new ValidationFailure("Title", "Title is required") };
 _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Video>(), default))
 .ReturnsAsync(new ValidationResult(failures));

 var controller = new SaveVideoController(_configMock.Object, _repoMock.Object, _validatorMock.Object);
 var result = await controller.HandleSaveVideoAsync(video);

 Assert.False(result.Success);
 Assert.Contains("Title is required", result.ErrorMessage);
 }

 [Fact]
 public async Task HandleSaveVideoAsync_Returns_Bad_When_Repo_Throws()
 {
 var video = new Video { Title = "Valid" };
 _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Video>(), default)).ReturnsAsync(new ValidationResult());
 _repoMock.Setup(r => r.SaveVideoAsync(It.IsAny<Video>())).ThrowsAsync(new Exception("db down"));

 var controller = new SaveVideoController(_configMock.Object, _repoMock.Object, _validatorMock.Object);
 var result = await controller.HandleSaveVideoAsync(video);

 Assert.False(result.Success);
 Assert.Contains("db down", result.ErrorMessage);
 }

 [Fact]
 public async Task HandleLoadVideoAsync_Returns_Success_When_Found()
 {
 var video = new Video { Id =5, Title = "Found" };
 _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Video>(), default)).ReturnsAsync(new ValidationResult());
 _repoMock.Setup(r => r.LoadVideoAsync(It.IsAny<int>())).ReturnsAsync(video);

 var controller = new LoadVideoController(_configMock.Object, _repoMock.Object, _validatorMock.Object);
 var input = new Video { Id =5 };
 var result = await controller.HandleLoadVideoAsync(input);

 Assert.True(result.Success);
 Assert.NotNull(result.Video);
 Assert.Equal(5, result.Video.Id);
 }

 [Fact]
 public async Task HandleLoadVideoAsync_Returns_Bad_When_Repo_Throws()
 {
 _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Video>(), default)).ReturnsAsync(new ValidationResult());
 _repoMock.Setup(r => r.LoadVideoAsync(It.IsAny<int>())).ThrowsAsync(new InvalidOperationException("timeout"));

 var controller = new LoadVideoController(_configMock.Object, _repoMock.Object, _validatorMock.Object);
 var input = new Video { Id =1 };
 var result = await controller.HandleLoadVideoAsync(input);

 Assert.False(result.Success);
 Assert.Contains("timeout", result.ErrorMessage);
 }
 }
}
