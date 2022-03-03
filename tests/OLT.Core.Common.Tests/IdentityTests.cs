using FluentAssertions;
using OLT.Core.Common.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Common.Tests
{
    public class IdentityTests
    {
        [Fact]
        public void IdentityTest()
        {
            var name = new OltPersonName
            {
                First = Faker.Name.First(),
                Middle = Faker.Name.Middle(),
                Last = Faker.Name.Last(),
                Suffix = Faker.Name.Suffix(),
            };

            var upn = Faker.RandomNumber.Next();
            var username = Faker.Internet.UserName();
            var emailAddress = Faker.Internet.Email();
            var phone = Faker.Phone.Number();


            var list = new List<Claim>();
            list.Add(new Claim(ClaimTypes.NameIdentifier, username));
            list.Add(new Claim(ClaimTypes.GivenName, name.First));
            list.Add(new Claim(OltClaimTypes.MiddleName, name.Middle));
            list.Add(new Claim(ClaimTypes.Surname, name.Last));
            list.Add(new Claim(ClaimTypes.Email, emailAddress));
            list.Add(new Claim(ClaimTypes.HomePhone, phone));
            list.Add(new Claim(ClaimTypes.Upn, upn.ToString()));

            var roles = new List<string>();
            var permissions = new List<string>();
            for (int i = 1; i <= Faker.RandomNumber.Next(8, 13); i++)
            {
                roles.Add($"role-{i}");
            }

            roles.Add(TestSecurityRoles.RoleOne.GetCodeEnum());
            roles.Add(TestSecurityRoles.RoleThree.GetCodeEnum());

            for (int i = 1; i <= Faker.RandomNumber.Next(11, 23); i++)
            {
                permissions.Add($"perm-{i}");
            }

            roles.ForEach(role => list.Add(new Claim(ClaimTypes.Role, role)));

            var identity = new GenericIdentity(name.FullName);
            identity.AddClaims(list);

            var model = new TestIdentity();
            Assert.Null(model.Identity);
            Assert.True(model.IsAnonymous);
            Assert.Empty(model.GetAllClaims());
            Assert.Empty(model.GetClaims(ClaimTypes.Name));
            Assert.Empty(model.GetClaims(null));
            Assert.Null(model.GetDbUsername());
            Assert.False(model.HasRole(null));


            model = new TestIdentity(identity);

            Assert.NotNull(model as IOltIdentity);
            Assert.NotNull(model.Identity);
            Assert.Equal(username, model.Username);
            Assert.Equal(name.Last, model.LastName);
            Assert.Equal(name.Middle, model.MiddleName);
            Assert.Equal(name.Last, model.LastName);
            Assert.Equal(emailAddress, model.Email);
            Assert.Equal(phone, model.Phone);
            Assert.Equal(name.FullName, model.FullName);
            Assert.Equal(upn.ToString(), model.UserPrincipalName);

            roles.ForEach(role => Assert.True(model.HasRole(role)));
            permissions.ForEach(perm => Assert.False(model.HasRole(perm)));

            TestSecurityRoles[] nullTest = null;

            Assert.False(model.HasRole(nullTest));
            Assert.True(model.HasRole(TestSecurityRoles.RoleOne));
            Assert.False(model.HasRole(TestSecurityRoles.RoleTwo));
            Assert.True(model.HasRole(TestSecurityRoles.RoleThree));
            Assert.False(model.HasRole(TestSecurityPermissions.PermOne));
            Assert.False(model.HasRole(TestSecurityPermissions.PermTwo));

            Assert.True(model.HasRole(TestSecurityRoles.RoleTwo, TestSecurityRoles.RoleThree));

            list.Add(new Claim(ClaimTypes.Name, name.FullName));
            Assert.Equal(list.Count, model.GetAllClaims().Count);

            //list.ForEach(item => Assert.True(model.HasRole(item.Type)));
            
        }
    }
}
