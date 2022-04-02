using System.Collections.Generic;

namespace OLT.Core
{
    public interface IOltFilterTemplateValueList<T> : IOltFilterTemplate
    {
        List<IOltValueListItem<T>> ValueList { get; }
    }
}
