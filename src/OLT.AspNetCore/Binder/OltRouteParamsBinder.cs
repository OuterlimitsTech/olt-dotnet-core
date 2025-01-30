using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace OLT.Core
{

    public class OltRouteParamsBinder<TModelType, TValue> : IOltRouteParamsBinder
        where TModelType : notnull
    {
        public OltRouteParamsBinder(string parameterKey, IOltRouteParamsParser<TValue> parser, Action<TModelType, TValue> action)
        {
            ParameterKey = parameterKey;
            Action = action;
            Parser = parser;
        }

        public virtual string ParameterKey { get; }
        protected virtual Action<TModelType, TValue> Action { get; }
        protected virtual IOltRouteParamsParser<TValue> Parser { get; }

        public virtual void BindParamModel(ModelBindingContext bindingContext)
        {
            if (bindingContext.Result.Model is TModelType paramModel)
            {
                var param = bindingContext.ValueProvider.GetValue(ParameterKey).FirstValue;
                if (Parser.TryParse(param, out var value))
                {
                    Action(paramModel, value);
                }
                bindingContext.Result = ModelBindingResult.Success(paramModel);
            }
        }
    }

}

