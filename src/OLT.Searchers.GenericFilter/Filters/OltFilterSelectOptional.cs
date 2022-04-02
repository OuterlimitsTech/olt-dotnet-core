using System;
using System.Linq.Expressions;

namespace OLT.Core
{
    public class OltFilterSelectOptional<TEntity> : OltGenericFilter<TEntity, int?>, IOltGenericFilterTemplate
        where TEntity : class, IOltEntity
    {

        private readonly OltFilterTemplateSelectListNullable _filterTemplate;

        public OltFilterSelectOptional(OltFilterTemplateSelectListNullable filterTemplate, Expression<Func<TEntity, int?>> fieldExpression) : base(filterTemplate, new OltEntityExpressionIntNullable<TEntity>(fieldExpression))
        {
            _filterTemplate = filterTemplate;
        }

        public IOltFilterTemplate FilterTemplate => _filterTemplate;
    }
}
