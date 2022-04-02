using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OLT.Core
{
    public class OltFilterMultiSelect<TEntity> : OltGenericFilter<TEntity, List<int>>, IOltGenericFilterTemplate
       where TEntity : class, IOltEntity
    {

        private readonly OltFilterTemplateMultiSelectList _filterTemplate;

        public OltFilterMultiSelect(OltFilterTemplateMultiSelectList filterTemplate, Expression<Func<TEntity, int>> fieldExpression) : base(filterTemplate, new OltEntityExpressionIntContains<TEntity>(fieldExpression))
        {
            _filterTemplate = filterTemplate;
        }

        public IOltFilterTemplate FilterTemplate => _filterTemplate;
    }
}
