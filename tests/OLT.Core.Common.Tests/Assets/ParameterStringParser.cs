using System;

namespace OLT.Core.Common.Tests.Assets;

public class ParameterStringParser : OltGenericParameterParser<string>
{
    public ParameterStringParser() : base(nameof(ParameterStringParser))
    {
    }

    public override bool HasValue => Value.IsNotEmpty();

    public override bool Parse(IOltGenericParameter parameters)
    {
        if (parameters == null)
        {
            Value = null;
            return false;
        }

        var val = parameters.GetValue($"{Key}", string.Empty);
        Value = val.IsNotEmpty() ? val : null;

        return HasValue;
    }

}