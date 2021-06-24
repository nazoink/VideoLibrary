using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VideoLibrary.Models;

namespace VideoLibrary.Validators
{
    public class SaveVideoValidator : AbstractValidator<Video>
    {
        public SaveVideoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
        }
    }
}
