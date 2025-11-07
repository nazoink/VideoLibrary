using System.Threading.Tasks;
using Xunit;
using VideoLibrary.Validators;
using VideoLibrary.Models;

namespace VideoLibrary.Tests
{
 public class SaveVideoValidatorTests
 {
 private readonly SaveVideoValidator _validator = new SaveVideoValidator();

 [Fact]
 public async Task Validate_Fails_When_Title_Is_Null()
 {
 var video = new Video { Title = null };
 var result = await _validator.ValidateAsync(video);
 Assert.False(result.IsValid);
 Assert.Contains(result.Errors, e => e.PropertyName == "Title");
 }

 [Fact]
 public async Task Validate_Fails_When_Title_Is_Empty()
 {
 var video = new Video { Title = string.Empty };
 var result = await _validator.ValidateAsync(video);
 Assert.False(result.IsValid);
 Assert.Contains(result.Errors, e => e.PropertyName == "Title");
 }

 [Fact]
 public async Task Validate_Succeeds_When_Title_Is_Present()
 {
 var video = new Video { Title = "Some Title" };
 var result = await _validator.ValidateAsync(video);
 Assert.True(result.IsValid);
 }
 }
}
