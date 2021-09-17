using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VideoLibrary.Models;

namespace VideoLibrary.Validators
{
    public class LoadVideoValidator : AbstractValidator<Video>
    {
        public LoadVideoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
