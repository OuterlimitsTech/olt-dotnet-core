using FluentDateTimeOffset;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Searchers.Tests.Assets
{
    public static class TestHelper
    {
        public static OltGenericParameter BuildGenericParameter(string key, OltDateRange value)
        {
            var dict = new Dictionary<string, string>();
            dict.Add($"{key}-start", value.Start.Midnight().ToString());
            dict.Add($"{key}-end", value.End.Midnight().ToString());
            return new OltGenericParameter(dict);
        }

        public static OltGenericParameter BuildGenericParameter(string key, OltValueListItem<int> item)
        {
            var dict = new Dictionary<string, string>();
            dict.Add(key, item.Value.ToString());
            return new OltGenericParameter(dict);
        }

        public static OltGenericParameter BuildGenericParameter(string key, List<OltValueListItem<int>> list)
        {
            var dict = new Dictionary<string, string>();
            dict.Add(key, string.Join(",", list.Select(s => s.Value)));
            return new OltGenericParameter(dict);
        }

        public static OltGenericParameter BuildGenericParameter<T>(string key, T value)
        {
            var dict = new Dictionary<string, string>();
            dict.Add(key, value?.ToString());
            return new OltGenericParameter(dict);
        }

        public static List<OltValueListItem<int>> ValueList(int number)
        {
            var result = new List<OltValueListItem<int>>();
            for (int i = 1; i <= number; i++)
            {
                result.Add(new OltValueListItem<int> { Label = Faker.Name.First(), Value = i });
            }
            return result;
        }

        public static List<OltValueListItem<OltDateRange>> DateRangeList()
        {
            var result = new List<OltValueListItem<OltDateRange>>();
            result.Add(new OltValueListItem<OltDateRange>("Today", OltDateRange.Today));
            result.Add(new OltValueListItem<OltDateRange>("Yesterday", OltDateRange.Yesterday));
            result.Add(new OltValueListItem<OltDateRange>("Last7Days", OltDateRange.Last7Days));
            result.Add(new OltValueListItem<OltDateRange>("LastMonth", OltDateRange.LastMonth));
            return result;
        }

        /// <summary>
        /// Randomize the List
        /// </summary>
        /// <param name="expected">Expected list of data to randomly add to results</param>
        /// <param name="startingId"></param>
        /// <param name="minMix"></param>
        /// <param name="maxMix"></param>
        /// <returns></returns>
        public static List<FakeEntity> BuildRandomList(List<FakeEntity> expected, List<int> selectValues, int startingId = 1000, int minMix = 0, int maxMix = 6)
        {
            var randomized = expected.OrderBy(x => Guid.NewGuid()).ToList();
            var list = new List<FakeEntity>();
            for (int i = 0; i < randomized.Count; i++)
            {
                var rangeList = FakeEntity.FakerList(Faker.RandomNumber.Next(minMix, maxMix), selectValues);
                rangeList.ForEach(item =>
                {
                    item.Id = startingId;
                    startingId++;
                    list.Add(item);
                });

                randomized[i].Id = startingId;
                startingId++;
                list.Add(randomized[i]);
            }
            return list;
        }
    }
}
