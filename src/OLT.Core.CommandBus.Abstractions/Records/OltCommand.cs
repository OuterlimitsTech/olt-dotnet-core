using FluentValidation.Results;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract record OltCommand() : IOltCommand
    {
        public abstract Task<ValidationResult> ValidateAsync();
        public virtual string ActionName => GetType().FullName ?? "UnknownOltCommand";
    }

    public abstract record OltCommand<TResult>() : OltCommand, IOltCommand<TResult> where TResult : notnull
    {
    }

}