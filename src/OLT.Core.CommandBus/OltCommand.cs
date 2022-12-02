using FluentValidation.Results;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract record OltCommand() : IOltCommand
    {
        public abstract Task<ValidationResult> ValidateAsync();
        public string ActionName => GetType().FullName;

    }
}