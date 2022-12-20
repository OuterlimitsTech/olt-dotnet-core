using FluentValidation.Results;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltCommand
    {
        string CorrelationId { get; }
        string ActionName { get; }
        Task<ValidationResult> ValidateAsync();
    }
}