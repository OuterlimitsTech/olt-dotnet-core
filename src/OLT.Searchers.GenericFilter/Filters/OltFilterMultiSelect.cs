using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OLT.Core
{
    public class OltFilterMultiSelect<TEntity> : OltGenericFilterTemplate<TEntity, List<int>>, IOltGenericFilterTemplate
       where TEntity : class, IOltEntity
    {
        public OltFilterMultiSelect(OltFilterTemplateMultiSelectList filterTemplate, Expression<Func<TEntity, int>> fieldExpression) : base(filterTemplate, new OltEntityExpressionIntContains<TEntity>(fieldExpression))
        {

        }
    }
}
