using System;
using System.Collections.Generic;

namespace OLT.Core.Common.Tests.Assets.Models;

public abstract class BaseEntityPersonModel : IOltEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

}

public class EntityPersonModel : BaseEntityPersonModel, IOltEntityId, IOltEntityDeletable, IOltEntityUniqueId
{
    public int Id { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }
    public string DeletedBy { get; set; }
    public Guid UniqueId { get; set; }

    public static EntityPersonModel FakerData(bool deleted = false)
    {
        var result = new EntityPersonModel
        {
            Id = Faker.RandomNumber.Next(1000, 900000),
            UniqueId = Guid.NewGuid(),
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
        };

        if (deleted)
        {
            result.DeletedOn = DateTimeOffset.Now.AddMinutes(Faker.RandomNumber.Next(10, 1000) * -1);
            result.DeletedBy = Faker.Internet.UserName();
        }
        return result;
    }

    public static List<EntityPersonModel> FakerList(int number, bool deleted = false)
    {
        var list = new List<EntityPersonModel>();
        for (int i = 0; i < number; i++)
        {
            var item = FakerData(deleted);
            list.Add(item);
        }
        return list;
    }

}