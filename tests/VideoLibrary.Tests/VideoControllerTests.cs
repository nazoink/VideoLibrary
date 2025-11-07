using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentValidation;
using VideoLibrary.Controllers;
using VideoLibrary.Models;
using VideoLibrary.Repository;
using Microsoft.Extensions.Configuration;
using FluentValidation.Results;

namespace VideoLibrary.Tests
{
 public class VideoControllerTests
 {
 private readonly Mock<IVideoRepo> _repoMock = new Mock<IVideoRepo>();
 private readonly Mock<IValidator<Video>> _validatorMock = new Mock<IValidator<Video>>();
 private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();

 [Fact]
 public async Task HandleSaveVideoAsync_Returns_Success_When_Valid()
 {
 var video = new Video { Title = "Test" };
 _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Video>(), default)).ReturnsAsync(new ValidationResult());
 _repoMock.Setup(r => r.SaveVideoAsync(It.IsAny<Video>())).ReturnsAsync(new Video { Id =1, Title = "Test" });

 var controller = new SaveVideoController(_configMock.Object, _repoMock.Object, _validatorMock.Object);
 var result = await controller.HandleSaveVideoAsync(video);

 Assert.True(result.Success);
 Assert.NotNull(result.Video);
 Assert.Equal(1, result.Video.Id);
 }

 [Fact]
 public async Task HandleLoadVideoAsync_Returns_Bad_When_Invalid()
 {
 var video = new Video { Id = null };
 _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Video>(), default)).ReturnsAsync(new ValidationResult());

 var controller = new LoadVideoController(_configMock.Object, _repoMock.Object, _validatorMock.Object);
 var result = await controller.HandleLoadVideoAsync(video);

 Assert.False(result.Success);
 }
 }
}
