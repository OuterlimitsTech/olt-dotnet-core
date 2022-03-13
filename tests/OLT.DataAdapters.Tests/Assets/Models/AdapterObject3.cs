﻿using OLT.Core;
using System.Collections.Generic;

namespace OLT.DataAdapters.Tests.Assets.Models
{
    public class AdapterObject3 : OltPersonName
    {
        public static AdapterObject3 FakerData()
        {
            var result = new AdapterObject3
            {
                First = Faker.Name.First(),
                Last = Faker.Name.Last(),
            };
            return result;
        }

        public static List<AdapterObject3> FakerList(int number)
        {
            var list = new List<AdapterObject3>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }


    public class AdapterObject4 : OltPersonName
    {
        public static AdapterObject4 FakerData()
        {
            var result = new AdapterObject4
            {
                First = Faker.Name.First(),
                Last = Faker.Name.Last(),
            };
            return result;
        }

        public static List<AdapterObject4> FakerList(int number)
        {
            var list = new List<AdapterObject4>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }

    public class AdapterObject5 : AdapterObject1
    {
        
    }
}