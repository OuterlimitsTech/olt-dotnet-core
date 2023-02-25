using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Entites;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OLT.EF.Core.Tests
{

    public class CrudTests : BaseUnitTests
    {
        private async Task<PersonEntity> AddPerson(UnitTestContext context)
        {
            var entity = PersonEntity.FakerEntity();
            context.People.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        [Fact]
        public async Task AddTests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestContext>();

                var entity = await AddPerson(context);                
                context.People.FirstOrDefault(p => p.Id == entity.Id).Should().BeEquivalentTo(entity);
                Assert.Equal("Insert", entity.ActionCode);  //Test IOltInsertingRecord


                entity = PersonEntity.FakerEntity();
                context.People.Add(entity);
                context.SaveChanges();
                context.People.FirstOrDefault(p => p.Id == entity.Id).Should().BeEquivalentTo(entity);
                Assert.Equal("Insert", entity.ActionCode);  //Test IOltInsertingRecord
            }
        }

        [Fact]
        public async Task UpdateTests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestContext>();
                var address = AddressEntity.FakerEntity();

                var entity = await AddPerson(context);
                context.People.FirstOrDefault(p => p.Id == entity.Id).Should().BeEquivalentTo(entity);

                entity.NameFirst = Faker.Name.First();
                entity.Addresses.Add(address);
                await context.SaveChangesAsync();

                context.People.FirstOrDefault(p => p.Id == entity.Id).Should().BeEquivalentTo(entity);
                Assert.Equal("Update", entity.ActionCode);  //Test IOltUpdatingRecord


                entity.NameFirst = Faker.Lorem.GetFirstWord();
                address.Street = Faker.Address.SecondaryAddress();  
                context.SaveChanges();
                context.People.FirstOrDefault(p => p.Id == entity.Id).Should().BeEquivalentTo(entity);

                

                entity.NameMiddle = " ";
                context.SaveChanges();
                Assert.Null(context.People.FirstOrDefault(p => p.Id == entity.Id).NameMiddle); //Check to see if empty string is set to null

                entity = context.People.FirstOrDefault(p => p.Id == entity.Id);
                entity.NameMiddle = "";
                context.SaveChanges();
                Assert.Null(context.People.FirstOrDefault(p => p.Id == entity.Id).NameMiddle); //Check to see if empty string is set to null



            }
        }

        [Fact]
        public async Task ExceptionTests()
        {
            using (var provider = BuildProviderWithLogging())
            {
                var context = provider.GetService<UnitTestContext>();

                var entity = await AddPerson(context);

                entity.NameFirst = Faker.Lorem.Paragraph(20);  //overflow
                Assert.Throws<DbUpdateException>(() => context.SaveChanges());
                await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
            }

            using (var provider = BuildProviderWithLogging())
            {
                var context = provider.GetService<UnitTestContext>();

                var entity = await AddPerson(context);
                entity.NameFirst = null;
                Assert.Throws<Exception>(() => context.SaveChanges());
                await Assert.ThrowsAsync<Exception>(() => context.SaveChangesAsync());
            }
        }

        [Fact]
        public async Task HardDeleteTests()
        {
            using (var provider = BuildProvider())
            {
                var address1 = AddressEntity.FakerEntity();
                var address2 = AddressEntity.FakerEntity();
                var context = provider.GetService<UnitTestContext>();

                var entity = PersonEntity.FakerEntity();
                entity.Addresses.Add(AddressEntity.FakerEntity());
                entity.Addresses.Add(address1);
                entity.Addresses.Add(AddressEntity.FakerEntity());
                entity.Addresses.Add(address2);
                entity.Addresses.Add(AddressEntity.FakerEntity());

                context.People.Add(entity);
                await context.SaveChangesAsync();
                context.People.Include(i => i.Addresses).FirstOrDefault(p => p.Id == entity.Id).Should().BeEquivalentTo(entity);

                try
                {
                    var deleteAddress = context.Addresses.FirstOrDefault(p => p.Id == address1.Id);
                    context.Addresses.Remove(deleteAddress);
                    await context.SaveChangesAsync();
                    Assert.True(true);
                }
                catch
                {
                    Assert.True(false);
                }

                try
                {
                    var deleteAddress = context.Addresses.FirstOrDefault(p => p.Id == address2.Id);
                    context.Addresses.Remove(deleteAddress);
                    context.SaveChanges();
                    Assert.True(true);
                }
                catch
                {
                    Assert.True(false);
                }
            }

        }

        [Fact]
        public async Task CascadeDeleteDisabledTests()
        {
            using (var provider = BuildProvider())
            {
                var address1 = AddressEntity.FakerEntity();
                var address2 = AddressEntity.FakerEntity();
                var context = provider.GetService<UnitTestContext>(); //Cascade Delete disabled

                Assert.True(context.DisableCascadeDeleteConvention);

                var entity = PersonEntity.FakerEntity();
                entity.Addresses.Add(AddressEntity.FakerEntity());
                entity.Addresses.Add(address1);
                entity.Addresses.Add(AddressEntity.FakerEntity());
                entity.Addresses.Add(address2);
                entity.Addresses.Add(AddressEntity.FakerEntity());

                context.People.Add(entity);
                await context.SaveChangesAsync();

                var person = context.People.Include(i => i.Addresses).FirstOrDefault(p => p.Id == entity.Id);

                Assert.Throws<InvalidOperationException>(() => context.Remove(person));

            }
        }

        [Fact]
        public async Task CascadeDeleteEnabledTests()
        {
            using (var provider = BuildProvider())
            {
                var address1 = AddressEntity.FakerEntity();
                var address2 = AddressEntity.FakerEntity();
                var context = provider.GetService<UnitTestAlternateContext>(); //Cascade Delete disabled

                Assert.False(context.DisableCascadeDeleteConvention);

                var entity = PersonEntity.FakerEntity();
                entity.Addresses.Add(AddressEntity.FakerEntity());
                entity.Addresses.Add(address1);
                entity.Addresses.Add(AddressEntity.FakerEntity());
                entity.Addresses.Add(address2);
                entity.Addresses.Add(AddressEntity.FakerEntity());

                context.People.Add(entity);
                await context.SaveChangesAsync();

                var person = context.People.Include(i => i.Addresses).FirstOrDefault(p => p.Id == entity.Id);

                try
                {                    
                    context.Remove(person);
                    context.SaveChanges();
                    Assert.True(true);                    
                }
                catch
                {
                    throw;
                }

 
            }
        }


        [Fact]
        public async Task NoStringEntityTests()
        {
            // GetNullableStringPropertyMetaData
            using (var provider = BuildProvider())
            {
                var entity = NoStringEntity.FakerEntity();
                var context = provider.GetService<UnitTestContext>();
                await context.NoStringEntities.AddAsync(entity);
                context.SaveChanges();
                context.NoStringEntities.FirstOrDefault(p => p.Id == entity.Id).Should().BeEquivalentTo(entity);


                entity = NoStringEntity.FakerEntity();
                await context.NoStringEntities.AddAsync(entity);
                
                await context.SaveChangesAsync();
                context.NoStringEntities.FirstOrDefault(p => p.Id == entity.Id).Should().BeEquivalentTo(entity);

            }
        }


        [Fact]
        public async Task EmptyExceptionStringTests()
        {
            // CheckNullableStringFields Exception Logic
            using (var provider = BuildProvider())
            {
                var entity = EmptyExceptionStringEntity.FakerEntity();
                var context = provider.GetService<UnitTestContext>();
                await context.EmptyExceptionStringEntities.AddAsync(entity);
                OltException exception = Assert.Throws<OltException>(() => context.SaveChanges()); 
                Assert.Equal("CheckNullableStringFields: OLT.EF.Core.Tests.Assets.Entites.EmptyExceptionStringEntity -> Title", exception.Message);


                entity = EmptyExceptionStringEntity.FakerEntity();
                await context.EmptyExceptionStringEntities.AddAsync(entity);
                var asyncException = await Assert.ThrowsAsync<OltException>(() => context.SaveChangesAsync());
                Assert.Equal("CheckNullableStringFields: OLT.EF.Core.Tests.Assets.Entites.EmptyExceptionStringEntity -> Title", asyncException.Message);
            }
        }


    }
}