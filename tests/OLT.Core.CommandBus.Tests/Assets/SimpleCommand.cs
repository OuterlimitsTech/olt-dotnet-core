﻿using FluentValidation.Results;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets
{
    public record SimpleCommand() : OltCommand
    {
        public override Task<ValidationResult> ValidateAsync()
        {
            return Task.FromResult(new ValidationResult());
        }
    }
}
