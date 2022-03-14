using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Constants;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Entites;
using System;
using System.Linq;
using Xunit;

namespace OLT.EF.Core.Tests
{

    public class ContextTests : BaseUnitTests
    {



        [Fact]
        public void AddTests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestDatabaseContext>();

                var entity = PersonEntity.FakerEntity();
                context.People.Add(entity);
                context.SaveChanges();
                var compare = context.People.FirstOrDefault(p => p.Id == entity.Id);
                compare.Should().BeEquivalentTo(entity);
            }
        }


        [Fact]
        public void ApplyGlobalDeleteFilterTests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestDatabaseContext>();

                var entity = PersonEntity.FakerEntity();
                entity.DeletedBy = Faker.Internet.Email();
                entity.DeletedOn = System.DateTimeOffset.Now;
                context.People.Add(entity);
                context.SaveChanges();
                var compare = context.People.FirstOrDefault(p => p.Id == entity.Id);
                Assert.Null(compare);                
            }
        }



        [Fact]
        public void AuditUserTests()
        {
            using (var provider = BuildProvider())
            {
                var now = DateTimeOffset.UtcNow;

                var context = provider.GetService<UnitTestDatabaseContext>();
                var auditUser = provider.GetService<IOltDbAuditUser>();
                Assert.Equal(OltEFCoreConstants.DefaultAnonymousUser, context.DefaultAnonymousUser);
                Assert.Equal(auditUser.GetDbUsername(), context.AuditUser);


                var entity = PersonEntity.FakerEntity();
                context.People.Add(entity);
                context.SaveChanges();

                var compare = context.People.FirstOrDefault(p => p.Id == entity.Id);
                Assert.Equal(auditUser.GetDbUsername(), compare.CreateUser);
                Assert.True(compare.CreateDate > now);
                Assert.Equal(auditUser.GetDbUsername(), compare.ModifyUser);
                Assert.True(compare.ModifyDate?.UtcDateTime >= compare.CreateDate.UtcDateTime);


                var created = compare.CreateDate;
                entity.NameFirst = Faker.Name.First();
                context.SaveChanges();

                Assert.Equal(auditUser.GetDbUsername(), compare.CreateUser);
                Assert.Equal(created, compare.CreateDate);
                Assert.Equal(auditUser.GetDbUsername(), compare.ModifyUser);
                Assert.True(compare.ModifyDate > created);


                var entity2 = PersonEntity.FakerEntity();
                var createDate = DateTimeOffset.Now.AddHours(Faker.RandomNumber.Next(1, 5));
                var createUser = Faker.Name.First();
                entity2.CreateDate = createDate;
                entity2.CreateUser = createUser;
                entity2.ModifyDate = createDate;
                entity2.ModifyUser = createUser;
                context.People.Add(entity2);
                context.SaveChanges();

                compare = context.People.FirstOrDefault(p => p.Id == entity2.Id);
                Assert.NotEqual(createDate, compare.CreateDate);
                Assert.NotEqual(createUser, compare.CreateUser);
                Assert.NotEqual(createDate, compare.ModifyDate);
                Assert.NotEqual(createUser, compare.ModifyUser);

            }
        }
    }
}