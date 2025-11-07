using FluentValidation;
using VideoLibrary.Models;

namespace VideoLibrary.Validators
{
    /// <summary>
    /// Validator used when saving a Video. Ensures required fields are present.
    /// </summary>
    public class SaveVideoValidator : AbstractValidator<Video>
    {
        public SaveVideoValidator()
        {
            // Title is required when saving a video.
            RuleFor(x => x.Title).NotEmpty();
        }
    }
}
