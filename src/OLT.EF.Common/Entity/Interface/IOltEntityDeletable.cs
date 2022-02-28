using System;

namespace OLT.Core
{
    public interface IOltEntityDeletable : IOltEntity
    {
        DateTimeOffset? DeletedOn { get; set; }
        string DeletedBy { get; set; }
    }
}