using OLT.Core;

namespace OLT.Core.Rules.Tests.Assets.Rules
{
    public interface INotValidRule : IOltRule
    {

    }

    public class NotValidRule : INotValidRule
    {
        public string RuleName => nameof(NotValidRule);
    }
}