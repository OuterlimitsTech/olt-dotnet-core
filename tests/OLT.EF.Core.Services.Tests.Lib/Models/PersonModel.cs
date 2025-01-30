namespace OLT.EF.Core.Services.Tests.Assets.Models;

public class PersonModel
{
    public int? PersonId { get; set; }
    public Guid? UniqueId { get; set; }
    public NameModel Name { get; set; } = new NameModel();

   


    public static PersonModel FakerEntity()
    {
        return new PersonModel
        {
            UniqueId = Guid.NewGuid(),
            Name = NameModel.FakerEntity(),
        };
    }

    public static List<PersonModel> FakerList(int number)
    {
        var list = new List<PersonModel>();

        for (int i = 0; i < number; i++)
        {
            list.Add(FakerEntity());
        }

        return list;
    }
}
