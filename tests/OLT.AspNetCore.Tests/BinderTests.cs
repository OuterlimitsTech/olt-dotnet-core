using Microsoft.AspNetCore.Mvc.ModelBinding;
using OLT.Core;
using System;
using Xunit;

namespace OLT.AspNetCore.Tests
{

    public class BinderTests
    {
        [Theory]
        [InlineData("1234", 1234)]
        [InlineData("abc", null)]
        [InlineData(" ", null)]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void OltRouteParamsBinderTests(string value, int? expectedValue)
        {
            
            Action<IdBinderTestModel, int> action = (model, paramValue) =>
            {
                model.Id = paramValue;
            };
            var model = new IdBinderTestModel();

            var binder = new OltRouteParamsBinder<IdBinderTestModel, int>("Id", new OltRouteParamsParserInt(), action);           

            var bindingContext = new FakeBindingContext();
            bindingContext.Model = model;
            bindingContext.Result = ModelBindingResult.Success(model);
            bindingContext.ValueProvider = new FakeValueProvider("Id", value);

            binder.BindParamModel(bindingContext);

            Assert.Equal("Id", binder.ParameterKey);
            Assert.True(bindingContext.Result.IsModelSet);
            Assert.Equal(expectedValue, model.Id);
        }
    }
}
