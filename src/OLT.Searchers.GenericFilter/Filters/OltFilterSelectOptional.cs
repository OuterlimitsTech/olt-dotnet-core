using System;
using System.Linq.Expressions;

namespace OLT.Core
{
    public class OltFilterSelectOptional<TEntity> : OltGenericFilterTemplate<TEntity, int?>, IOltGenericFilterTemplate
        where TEntity : class, IOltEntity
    {

        public OltFilterSelectOptional(OltFilterTemplateSelectListNullable filterTemplate, Expression<Func<TEntity, int?>> fieldExpression) : base(filterTemplate, new OltEntityExpressionIntNullable<TEntity>(fieldExpression))
        {
        }
    }
}
