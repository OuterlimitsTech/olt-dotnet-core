using System;
using System.Linq.Expressions;

namespace OLT.Core
{
    public class OltFilterSelect<TEntity> : OltGenericFilterTemplate<TEntity, int>, IOltGenericFilterTemplate
     where TEntity : class, IOltEntity
    {

        public OltFilterSelect(OltFilterTemplateSelectList filterTemplate, Expression<Func<TEntity, int>> fieldExpression) : base(filterTemplate, new OltEntityExpressionInt<TEntity>(fieldExpression))
        {
        }

    }
}
