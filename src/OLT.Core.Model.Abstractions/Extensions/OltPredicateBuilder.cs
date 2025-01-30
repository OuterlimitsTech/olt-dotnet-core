using OLT.Core;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class OltPredicateBuilder
    {


        public static Expression<TEntity> Compose<TEntity>(this Expression<TEntity> first, Expression<TEntity> second, Func<Expression, Expression, Expression> merge)
        {

            // build parameter map (from parameters of second to parameters of first)

            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);



            // replace parameters in the second lambda expression with parameters from the first

            var secondBody = OltParameterRebinder.ReplaceParameters(map, second.Body);



            // apply composition of lambda expression bodies to parameters from the first expression 

            return Expression.Lambda<TEntity>(merge(first.Body, secondBody), first.Parameters);

        }



        public static Expression<Func<TEntity, bool>> And<TEntity>(this Expression<Func<TEntity, bool>> first, Expression<Func<TEntity, bool>> second)
        {

            return first.Compose(second, Expression.And);

        }


        public static Expression<Func<TEntity, bool>> Or<TEntity>(this Expression<Func<TEntity, bool>> first, Expression<Func<TEntity, bool>> second)
        {

            return first.Compose(second, Expression.Or);

        }



        public static Expression ReplaceParameter(this Expression expression, ParameterExpression source, Expression target)
        {
            return new OltParameterReplacer { Source = source, Target = target }.Visit(expression);
        }

      

        #region [ OltParameterRebinder ExpressionVisitor ]

        class OltParameterRebinder : ExpressionVisitor
        {

            private readonly Dictionary<ParameterExpression, ParameterExpression> map;



            public OltParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {

                this.map = map;

            }



            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {

                return new OltParameterRebinder(map).Visit(exp);

            }




            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (map.TryGetValue(node, out var replacement))
                {

                    node = replacement;

                }

                return base.VisitParameter(node);

            }
        }

        #endregion

        #region [ OltParameterReplacer ExpressionVisitor ]

        class OltParameterReplacer : ExpressionVisitor
        {
            public ParameterExpression Source = default!;
            public Expression Target = default!;
            protected override Expression VisitParameter(ParameterExpression node) => node == Source ? Target : base.VisitParameter(node);
        }

        #endregion

    }
}