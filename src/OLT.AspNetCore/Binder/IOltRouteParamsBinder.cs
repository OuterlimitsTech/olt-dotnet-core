using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OLT.Core
{
    public interface IOltRouteParamsBinder
    {
        string ParameterKey { get; }
        void BindParamModel(ModelBindingContext bindingContext);
    }

}

