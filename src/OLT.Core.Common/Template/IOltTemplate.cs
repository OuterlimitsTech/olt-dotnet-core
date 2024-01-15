using System;
using System.Collections.Generic;
using System.Text;

namespace OLT.Core
{
    /// <summary>
    /// Used by Angular dynamic component process to render a component using the template name
    /// </summary>
    [Obsolete("Being Removed in 9.x")]
    public interface IOltTemplate
    {
        string TemplateName { get; }
    }

    /// <summary>
    /// Used by Angular dynamic component process to render a component using the template name with metadata
    /// </summary>
    /// <typeparam name="TMetadata"></typeparam>
    [Obsolete("Being Removed in 9.x")]
    public interface IOltTemplate<out TMetadata> : IOltTemplate
    {
        TMetadata Metadata { get; }
    }

}
