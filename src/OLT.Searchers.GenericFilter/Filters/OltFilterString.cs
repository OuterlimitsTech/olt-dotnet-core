using System;
using System.Linq.Expressions;

namespace OLT.Core
{
    public class OltFilterString<TEntity> : OltGenericFilter<TEntity, string>, IOltGenericFilterTemplate
      where TEntity : class, IOltEntity
    {

        private readonly OltFilterTemplateString _filterTemplate;

        public OltFilterString(OltFilterTemplateString filterTemplate, IOltEntityExpression<TEntity, string> fieldExpression) : base(filterTemplate, fieldExpression)
        {
            _filterTemplate = filterTemplate;
        }

        public IOltFilterTemplate FilterTemplate => _filterTemplate;
    }
}
