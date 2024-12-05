//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using OLT.Core;
//using OLT.EF.Core.Tests.Assets;
//using OLT.EF.Core.Tests.Assets.Entites;
//using System;
//using System.Linq;
//using Xunit;

//namespace OLT.EF.Core.Tests
//{
//    public class ContextExtensionsTests : BaseUnitTests
//    {
//        [Fact]
//        public void GetTableNameTests()
//        {
//            using (var provider = BuildProvider())
//            {
//                var context = provider.GetRequiredService<UnitTestContext>();
//                Assert.Equal($"{context.DefaultSchema}.{nameof(PersonEntity)}", OltContextExtensions.GetTableName<PersonEntity>(context));
//                Assert.Throws<ArgumentNullException>("context", () => OltContextExtensions.GetTableName<PersonEntity>(null!));

//                var context2 = provider.GetRequiredService<UnitTestAlternateContext>();
//                Assert.Equal($"{nameof(PersonEntity)}", OltContextExtensions.GetTableName<PersonEntity>(context2));
//            }
//        }

//        [Fact]
//        public void GetColumnsTests()
//        {
//            using (var provider = BuildProvider())
//            {
//                const string expected = "PersonEntityId";
//                var context = provider.GetRequiredService<UnitTestContext>();

//                var columns = OltContextExtensions.GetColumns<PersonEntity>(context).ToList();
//                //columns.ForEach(col =>
//                //{                    
//                //    //Logger.Debug(col.Name);
//                //});
//                Assert.Collection(columns,
//                    item => Assert.Equal(expected, item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.ActionCode)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.CreateDate)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.CreateUser)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.DeletedBy)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.DeletedOn)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.EffectiveDate)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.ModifyDate)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.ModifyUser)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.NameFirst)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.NameLast)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.NameMiddle)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.StatusTypeId)}", item.Name),
//                    item => Assert.Equal($"{nameof(PersonEntity.UniqueId)}", item.Name)
//                    );


//                Assert.Throws<ArgumentNullException>("dbContext", () => OltContextExtensions.GetColumns<PersonEntity>(null!));

//            }
//        }


//        [Fact]
//        public void InitializeQueryableWithQueryFilterTest()
//        {
//            using (var provider = BuildProvider())
//            {
//                var context = provider.GetRequiredService<UnitTestContext>();
//                var entity = PersonEntity.FakerEntity();
//                entity.DeletedOn = DateTime.UtcNow;

//                context.People.Add(PersonEntity.FakerEntity());
//                context.People.Add(PersonEntity.FakerEntity());
//                context.People.Add(entity);
//                context.People.Add(PersonEntity.FakerEntity());
//                context.People.Add(PersonEntity.FakerEntity());

//                context.SaveChanges();
//                var list = context.People.IgnoreQueryFilters().ToList();


//                OltContextExtensions.InitializeQueryable<PersonEntity>(context).Should().BeEquivalentTo(list);
//                OltContextExtensions.InitializeQueryable<PersonEntity>(context, false).Should().BeEquivalentTo(list.Where(p => p.DeletedOn == null));


//                Assert.Throws<ArgumentNullException>("context", () => OltContextExtensions.InitializeQueryable<PersonEntity>(null!));
//                Assert.Throws<ArgumentNullException>("context", () => OltContextExtensions.InitializeQueryable<PersonEntity>(null!, true));
//                Assert.Throws<ArgumentNullException>("context", () => OltContextExtensions.InitializeQueryable<PersonEntity>(null!, false));

//            }
//        }

//        [Fact]
//        public void InitializeQueryableWithoutQueryFilterTest()
//        {
//            using (var provider = BuildProvider())
//            {
//                var context = provider.GetRequiredService<UnitTestAlternateContext>();
//                var entity = PersonEntity.FakerEntity();
//                entity.DeletedOn = DateTime.UtcNow;

//                context.People.Add(PersonEntity.FakerEntity());
//                context.People.Add(PersonEntity.FakerEntity());
//                context.People.Add(entity);
//                context.People.Add(PersonEntity.FakerEntity());
//                context.People.Add(PersonEntity.FakerEntity());

//                context.SaveChanges();
//                var list = context.People.ToList();

//                OltContextExtensions.InitializeQueryable<PersonEntity>(context).Should().BeEquivalentTo(list);
//                OltContextExtensions.InitializeQueryable<PersonEntity>(context, false).Should().BeEquivalentTo(list.Where(p => p.DeletedOn == null));

//            }
//        }

//    }
//}