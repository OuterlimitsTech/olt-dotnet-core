//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using OLT.Constants;
//using OLT.Core;
//using OLT.EF.Core.Tests.Assets;
//using OLT.EF.Core.Tests.Assets.Entites;


//namespace OLT.EF.Core.Tests
//{

//    public class ContextTests : BaseUnitTests
//    {

//        [Fact]
//        public void ConfigTests()
//        {
//            using (var provider = BuildProvider())
//            {
//                var context = provider.GetRequiredService<UnitTestContext>();
//                var altContext = provider.GetRequiredService<UnitTestAlternateContext>();

//                Assert.Equal(OltContextStringTypes.Varchar, context.DefaultStringType);
//                Assert.Equal(OltContextStringTypes.NVarchar, altContext.DefaultStringType);

//                Assert.True(context.ApplyGlobalDeleteFilter);
//                Assert.False(altContext.ApplyGlobalDeleteFilter);

//                Assert.True(context.DisableCascadeDeleteConvention);
//                Assert.False(altContext.DisableCascadeDeleteConvention);

//                Assert.Equal("UnitTest", context.DefaultSchema);
//                Assert.Null(altContext.DefaultSchema);
//            }
//        }

//        [Fact]
//        public void ApplyGlobalDeleteFilterTests()
//        {
//            using (var provider = BuildProvider())
//            {
//                var context1 = provider.GetRequiredService<UnitTestContext>();
//                var context2 = provider.GetRequiredService<UnitTestAlternateContext>();

//                var entity1 = PersonEntity.FakerEntity();
//                entity1.DeletedBy = Faker.Internet.Email();
//                entity1.DeletedOn = System.DateTimeOffset.Now;

//                context1.People.Add(entity1);
//                context1.SaveChanges();
//                Assert.Null(context1.People.FirstOrDefault(p => p.Id == entity1.Id));


//                var entity2 = PersonEntity.FakerEntity();
//                entity2.DeletedBy = Faker.Internet.Email();
//                entity2.DeletedOn = System.DateTimeOffset.Now;
//                context2.People.Add(entity2);
//                context2.SaveChanges();
//                Assert.NotNull(context2.People.FirstOrDefault(p => p.Id == entity2.Id));

//            }
//        }

//        [Fact]
//        public void SortOrderTests()
//        {
//            using (var provider = BuildProvider())
//            {
//                var sortOrder = (short)Faker.RandomNumber.Next(1000, 1200);
//                var context = provider.GetRequiredService<UnitTestContext>();

//                var entityDefault = NoIdEntity.FakerEntity();
//                context.NoIdentifiers.Add(entityDefault);
//                context.SaveChanges();
//                Assert.Equal(OltCommonDefaults.SortOrder, entityDefault.SortOrder);  //Test default Sort Order


//                var entityNoDefault = NoIdEntity.FakerEntity();
//                entityNoDefault.SortOrder = sortOrder;
//                context.NoIdentifiers.Add(entityNoDefault);
//                context.SaveChanges();
//                Assert.NotEqual(OltCommonDefaults.SortOrder, entityNoDefault.SortOrder);
//                Assert.Equal(sortOrder, entityNoDefault.SortOrder);

//                var entityNegative = NoIdEntity.FakerEntity();
//                entityNegative.SortOrder = -1;
//                context.NoIdentifiers.Add(entityNegative);
//                context.SaveChanges();
//                Assert.Equal(OltCommonDefaults.SortOrder, entityNegative.SortOrder);
//            }
//        }


//        [Fact]
//        public void AuditUserTests()
//        {
//            using (var provider = BuildProvider())
//            {
//                var now = DateTimeOffset.UtcNow;

//                var context = provider.GetRequiredService<UnitTestContext>();
//                var auditUser = provider.GetRequiredService<IOltDbAuditUser>();
//                Assert.Equal(OltEFCoreConstants.DefaultAnonymousUser, context.DefaultAnonymousUser);
//                Assert.Equal(auditUser.GetDbUsername(), context.AuditUser);


//                var entity = PersonEntity.FakerEntity();
//                context.People.Add(entity);
//                context.SaveChanges();

//                var compare = context.People.FirstOrDefault(p => p.Id == entity.Id);
//                Assert.Equal(auditUser.GetDbUsername(), compare?.CreateUser);
//                Assert.True(compare?.CreateDate > now);
//                Assert.Equal(auditUser.GetDbUsername(), compare.ModifyUser);
//                Assert.True(compare.ModifyDate?.UtcDateTime >= compare.CreateDate.UtcDateTime);


//                var created = compare.CreateDate;
//                entity.NameFirst = Faker.Name.First();
//                context.SaveChanges();

//                Assert.Equal(auditUser.GetDbUsername(), compare.CreateUser);
//                Assert.Equal(created, compare.CreateDate);
//                Assert.Equal(auditUser.GetDbUsername(), compare.ModifyUser);
//                Assert.True(compare.ModifyDate > created);


//                var entity2 = PersonEntity.FakerEntity();
//                var createDate = DateTimeOffset.Now.AddHours(Faker.RandomNumber.Next(1, 5));
//                var createUser = Faker.Name.First();
//                entity2.CreateDate = createDate;
//                entity2.CreateUser = createUser;
//                entity2.ModifyDate = createDate;
//                entity2.ModifyUser = createUser;
//                context.People.Add(entity2);
//                context.SaveChanges();

//                compare = context.People.FirstOrDefault(p => p.Id == entity2.Id);
//                Assert.NotEqual(createDate, compare?.CreateDate);
//                Assert.NotEqual(createUser, compare?.CreateUser);
//                Assert.NotEqual(createDate, compare?.ModifyDate);
//                Assert.NotEqual(createUser, compare?.ModifyUser);

//            }


//            var services = new ServiceCollection();

//            services
//                //.AddLogging(config => config.AddConsole())
//                //.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: false))
//                .AddDbContextPool<UnitTestContext>((serviceProvider, optionsBuilder) =>
//                {
//                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}");
//                });

//            services.AddScoped<IOltDbAuditUser, EmtpyDbAuditUserService>();

//            using (var provider = services.BuildServiceProvider())
//            {
//                var context = provider.GetRequiredService<UnitTestContext>();
//                Assert.Equal(context.DefaultAnonymousUser, context.AuditUser);
//            }
//        }
//    }
//}