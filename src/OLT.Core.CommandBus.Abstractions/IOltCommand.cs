using FluentValidation.Results;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltCommand
    {
        string ActionName { get; }
        Task<ValidationResult> ValidateAsync();
    }

    public interface IOltCommand<TResult> : IOltCommand where TResult : notnull
    {

    }
}