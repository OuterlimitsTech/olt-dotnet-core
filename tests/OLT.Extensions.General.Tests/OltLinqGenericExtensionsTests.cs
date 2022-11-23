using FluentAssertions;
using Microsoft.Extensions.Options;
using OLT.Extensions.General.Tests.Assets.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Extensions.General.Tests
{
    public class OltLinqGenericExtensionsTests
    {

        private List<TestItem> BuildItems(int numItems)
        {
            var list = new List<TestItem>();

            for (var idx = 0; idx < numItems; idx++)
            {
                list.Add(new TestItem(Faker.RandomNumber.Next()));
            }
            return list;
        }

        [Fact]
        public void ExceptTests()
        {
            var rnd = new Random();
            var list1 = BuildItems(Faker.RandomNumber.Next(50,80));
            var list2 = BuildItems(Faker.RandomNumber.Next(5, 10));
            var inBothList = new List<TestItem>();

            
            list1.OrderBy(p => Guid.NewGuid())
                .Take(Faker.RandomNumber.Next(3,6))
                .ToList()
                .ForEach(item =>
                {                    
                    inBothList.Add(item);
                    list2.Add(item);
                    list2.AddRange(BuildItems(Faker.RandomNumber.Next(2, 6)));
                });

            
            var found1 = OltLinqGenericExtensions.Except(list1, list2, new OltLambdaComparer<TestItem>((p, q) => p.Uid == q.Uid)).ToList();
            var found2 = OltLinqGenericExtensions.Except(list1, list2, (p, q) => p.Uid == q.Uid).ToList();
            var diff = list1.Count() - inBothList.Count();

            found1.Should().HaveCount(diff);
            found1.Should().NotContain(inBothList);

            found2.Should().HaveCount(diff);
            found2.Should().NotContain(inBothList);

            found1.Should().Contain(found2);


            Func<TestItem, TestItem, bool> nullFuncComparer = null;
            Assert.Throws<ArgumentNullException>("first", () => OltLinqGenericExtensions.Except(null, list2, (p, q) => p.Uid == q.Uid));
            Assert.Throws<ArgumentNullException>("second", () => OltLinqGenericExtensions.Except(list1, null, (p, q) => p.Uid == q.Uid));
            Assert.Throws<ArgumentNullException>("comparer", () => OltLinqGenericExtensions.Except(list1, list2, nullFuncComparer));

            OltLambdaComparer<TestItem> nullLambdaComparer = null;
            Assert.Throws<ArgumentNullException>("first", () => OltLinqGenericExtensions.Except(null, list2, new OltLambdaComparer<TestItem>((p, q) => p.Uid == q.Uid)));
            Assert.Throws<ArgumentNullException>("second", () => OltLinqGenericExtensions.Except(list1, null, new OltLambdaComparer<TestItem>((p, q) => p.Uid == q.Uid)));
            Assert.Throws<ArgumentNullException>("comparer", () => OltLinqGenericExtensions.Except(list1, list2, nullLambdaComparer));
        }

        [Fact]
        public void IntersectTests()
        {
            var rnd = new Random();
            var list1 = BuildItems(Faker.RandomNumber.Next(50, 80));
            var list2 = BuildItems(Faker.RandomNumber.Next(5, 10));
            var inBothList = new List<TestItem>();


            list1.OrderBy(p => Guid.NewGuid())
                .Take(Faker.RandomNumber.Next(3, 6))
                .ToList()
                .ForEach(item =>
                {
                    inBothList.Add(item);
                    list2.Add(item);
                    list2.AddRange(BuildItems(Faker.RandomNumber.Next(2, 6)));
                });


            var found1 = OltLinqGenericExtensions.Intersect(list1, list2, new OltLambdaComparer<TestItem>((p, q) => p.Uid == q.Uid)).ToList();
            var found2 = OltLinqGenericExtensions.Intersect(list1, list2, (p, q) => p.Uid == q.Uid).ToList();            

            found1.Should().HaveCount(inBothList.Count());
            found1.Should().Contain(inBothList);

            found2.Should().HaveCount(inBothList.Count());
            found2.Should().Contain(inBothList);
            found1.Should().Contain(found2);

            Func<TestItem, TestItem, bool> nullFuncComparer = null;
            Assert.Throws<ArgumentNullException>("first", () => OltLinqGenericExtensions.Intersect(null, list2, (p, q) => p.Uid == q.Uid));
            Assert.Throws<ArgumentNullException>("second", () => OltLinqGenericExtensions.Intersect(list1, null, (p, q) => p.Uid == q.Uid));
            Assert.Throws<ArgumentNullException>("comparer", () => OltLinqGenericExtensions.Intersect(list1, list2, nullFuncComparer));

            OltLambdaComparer<TestItem> nullLambdaComparer = null;
            Assert.Throws<ArgumentNullException>("first", () => OltLinqGenericExtensions.Intersect(null, list2, new OltLambdaComparer<TestItem>((p, q) => p.Uid == q.Uid)));
            Assert.Throws<ArgumentNullException>("second", () => OltLinqGenericExtensions.Intersect(list1, null, new OltLambdaComparer<TestItem>((p, q) => p.Uid == q.Uid)));
            Assert.Throws<ArgumentNullException>("comparer", () => OltLinqGenericExtensions.Intersect(list1, list2, nullLambdaComparer));


        }
    }
}