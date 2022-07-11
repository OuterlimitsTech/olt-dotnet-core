using System;
using System.Linq.Expressions;

namespace OLT.Core
{
    public class OltFilterString<TEntity> : OltGenericFilterTemplate<TEntity, string>, IOltGenericFilterTemplate
      where TEntity : class, IOltEntity
    {
        public OltFilterString(OltFilterTemplateString filterTemplate, IOltEntityExpression<TEntity, string> fieldExpression) : base(filterTemplate, fieldExpression)
        {
        }
    }
}
