﻿using FluentValidation.Results;
using System;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract record OltCommand() : IOltCommand
    {    
        public abstract Task<ValidationResult> ValidateAsync();
        public virtual string ActionName => GetType().FullName;
    }

    public abstract record OltCommand<TResult>() : OltCommand, IOltCommand<TResult> where TResult : notnull
    {
    }
}