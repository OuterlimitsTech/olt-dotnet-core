using System;
using System.Linq.Expressions;

namespace OLT.Core
{
    public class OltFilterSelect<TEntity> : OltGenericFilter<TEntity, int>, IOltGenericFilterTemplate
     where TEntity : class, IOltEntity
    {

        private readonly OltFilterTemplateSelectList _filterTemplate;

        public OltFilterSelect(OltFilterTemplateSelectList filterTemplate, Expression<Func<TEntity, int>> fieldExpression) : base(filterTemplate, new OltEntityExpressionInt<TEntity>(fieldExpression))
        {
            _filterTemplate = filterTemplate;
        }

        public IOltFilterTemplate FilterTemplate => _filterTemplate;
    }
}
