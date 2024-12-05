//using OLT.Core.Common.Tests.Assets.Models;
//using Xunit;

//namespace OLT.Core.Common.Tests;

//public class MaintainableAttributeTests
//{
//    public enum AttributeTest
//    {
//        [Maintainable(Create = OltEntityMaintainable.Yes, Delete = OltEntityMaintainable.No)]
//        Value1,

//        Value2,

//        [Maintainable(Create = OltEntityMaintainable.Yes, Update = OltEntityMaintainable.No)]
//        Value3,

//        [Maintainable(Delete = OltEntityMaintainable.Yes, Update = OltEntityMaintainable.Yes)]
//        Value4,
//    }



//    [Theory]
//    [InlineData(OltEntityMaintainable.Yes, AttributeTest.Value1)]
//    [InlineData(OltEntityMaintainable.NotSet, AttributeTest.Value2)]
//    [InlineData(OltEntityMaintainable.Yes, AttributeTest.Value3)]
//    [InlineData(OltEntityMaintainable.NotSet, AttributeTest.Value4)]
//    [InlineData(OltEntityMaintainable.NotSet, null)]
//    public void CreateTests(OltEntityMaintainable expected, AttributeTest? value)
//    {
//        Assert.Equal(expected, value.GetMaintainable()?.Create);
//    }


//    [Theory]
//    [InlineData(OltEntityMaintainable.NotSet, AttributeTest.Value1)]
//    [InlineData(OltEntityMaintainable.NotSet, AttributeTest.Value2)]
//    [InlineData(OltEntityMaintainable.No, AttributeTest.Value3)]
//    [InlineData(OltEntityMaintainable.Yes, AttributeTest.Value4)]
//    [InlineData(OltEntityMaintainable.NotSet, null)]
//    public void UpdateTests(OltEntityMaintainable expected, AttributeTest? value)
//    {
//        Assert.Equal(expected, value.GetMaintainable()?.Update);
//    }

//    [Theory]
//    [InlineData(OltEntityMaintainable.No, AttributeTest.Value1)]
//    [InlineData(OltEntityMaintainable.NotSet, AttributeTest.Value2)]
//    [InlineData(OltEntityMaintainable.NotSet, AttributeTest.Value3)]
//    [InlineData(OltEntityMaintainable.Yes, AttributeTest.Value4)]
//    [InlineData(OltEntityMaintainable.NotSet, null)]
//    public void DeleteTests(OltEntityMaintainable expected, AttributeTest? value)
//    {
//        Assert.Equal(expected, value.GetMaintainable()?.Delete);
//    }

//    [Theory]
//    [InlineData(true, null, false, AttributeTest.Value1)]
//    [InlineData(null, null, null, AttributeTest.Value2)]
//    [InlineData(true, false, null, AttributeTest.Value3)]
//    [InlineData(null, true, true, AttributeTest.Value4)]
//    public void EntityMaintainableTest(bool? created, bool? updated, bool? deleted, AttributeTest value)
//    {
//        var model = EntityMaintainableModel.FakerData();
//        model.SetMaintainable(value);

//        Assert.Equal(created, model.MaintAdd);
//        Assert.Equal(updated, model.MaintUpdate);
//        Assert.Equal(deleted, model.MaintDelete);
//    }

//}