using OLT.Core;
using OLT.EF.Common.Tests.Assets.Models;
using System;
using Xunit;
using Xunit.Sdk;

namespace OLT.EF.Common.Tests
{
    public class EntityModelTests
    {
        [Fact]
        public void EntityIdDeletableTest()
        {
            
            var model = new EntityIdDeletableModel();
            Assert.Equal(0, model.Id);
            Assert.True(model.CreateDate < DateTimeOffset.Now.AddSeconds(2));
            Assert.True(model.CreateDate > DateTimeOffset.Now.AddSeconds(-2));
            Assert.Null(model.CreateUser);
            Assert.Null(model.ModifyDate);
            Assert.Null(model.ModifyUser);
            Assert.Null(model.DeletedBy);
            Assert.Null(model.DeletedOn);


            model = EntityIdDeletableModel.FakerData();
            Assert.NotEqual(0, model.Id);
            Assert.NotEqual(DateTimeOffset.MinValue, model.CreateDate);
            Assert.True(model.CreateDate < DateTimeOffset.Now);
            Assert.NotNull(model.CreateUser);
            Assert.NotNull(model.ModifyDate);
            Assert.NotNull(model.ModifyUser);
            Assert.NotNull(model.DeletedBy);
            Assert.NotNull(model.DeletedOn);


            Assert.IsAssignableFrom<IOltEntity>(model);
            Assert.IsAssignableFrom<IOltEntityId>(model);
            Assert.IsAssignableFrom<IOltEntityAudit>(model);
            Assert.IsAssignableFrom<IOltEntityDeletable>(model);
        }

        [Fact]
        public void EntityDeletableModelTest()
        {

            var model = new EntityDeletableModel();
            Assert.True(model.CreateDate < DateTimeOffset.Now.AddSeconds(2));
            Assert.True(model.CreateDate > DateTimeOffset.Now.AddSeconds(-2));
            Assert.Null(model.CreateUser);
            Assert.Null(model.ModifyDate);
            Assert.Null(model.ModifyUser);
            Assert.Null(model.DeletedBy);
            Assert.Null(model.DeletedOn);


            model = EntityDeletableModel.FakerData();
            Assert.NotEqual(DateTimeOffset.MinValue, model.CreateDate);
            Assert.True(model.CreateDate < DateTimeOffset.Now);
            Assert.NotNull(model.CreateUser);
            Assert.NotNull(model.ModifyDate);
            Assert.NotNull(model.ModifyUser);
            Assert.NotNull(model.DeletedBy);
            Assert.NotNull(model.DeletedOn);
                        
            Assert.IsAssignableFrom<IOltEntity>(model);
            Assert.Throws<IsAssignableFromException>(() => Assert.IsAssignableFrom<IOltEntityId>(model));            
            Assert.IsAssignableFrom<IOltEntityAudit>(model);
            Assert.IsAssignableFrom<IOltEntityDeletable>(model);
        }   
    }
}