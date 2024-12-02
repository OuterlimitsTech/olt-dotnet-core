using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;

namespace OLT.EF.Core.Services.Tests.Automapper.Adapters;

public class PersonWithAddressDtoAdapter : OltAdapter<PersonEntity, PersonWithAddressDto>
{
    public override void Map(PersonEntity source, PersonWithAddressDto destination)
    {
        destination.PersonId = source.Id;
        destination.UniqueId = source.UniqueId;
        destination.First = source.NameFirst;
        destination.Middle = source.NameMiddle;
        destination.Last = source.NameLast;

        source.Addresses.ForEach(item =>
        {
            destination.Addresses.Add(new AddressDto
            {
                AddressId = item.Id,
                PersonId = item.PersonId,
                Street = item.Street,
                City = item.City,
            });
        });
    }

    public override void Map(PersonWithAddressDto source, PersonEntity destination)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(source.First);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(source.Last);

        destination.UniqueId = source.UniqueId.HasValue ? source.UniqueId.Value : throw new NullReferenceException(nameof(source.UniqueId));
        destination.NameFirst = source.First;
        destination.NameMiddle = source.Middle;
        destination.NameLast = source.Last;

        source.Addresses.ForEach(item =>
        {
            var entity = destination.Addresses.FirstOrDefault(p => p.Id == item.AddressId);
            if (entity == null)
            {
                destination.Addresses.Add(new AddressEntity
                {
                    Street = item.Street,
                    City = item.City,
                });
            }
            else
            {
                entity.Street = item.Street;
                entity.City = item.City;
            }
        });

    }
}
