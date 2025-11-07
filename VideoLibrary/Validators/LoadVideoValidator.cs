using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VideoLibrary.Models;

namespace VideoLibrary.Validators
{
    /// <summary>
    /// Validator used when loading a Video to ensure the payload contains a valid Id.
    /// </summary>
    public class LoadVideoValidator : AbstractValidator<Video>
    {
        public LoadVideoValidator()
        {
            // Id must be provided when loading a video.
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
