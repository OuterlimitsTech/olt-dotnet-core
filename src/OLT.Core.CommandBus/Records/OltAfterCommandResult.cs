using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public record OltAfterCommandResult(List<Exception> Errors) : IOltAfterCommandResult
    {
        public virtual bool Success => !Errors.Any();

        public static OltAfterCommandResult Create(List<Exception> errors)
        {
            return new OltAfterCommandResult(errors);
        }
    }
}