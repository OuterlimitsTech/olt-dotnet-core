using OLT.Core;

namespace OLT.EF.Core.SeedHelpers.Csv.Tests.Assets
{
    public class PersonTypeCsvModel : IOltCsvSeedModel<PersonTypeCodeEntity>
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }

        public void Map(PersonTypeCodeEntity entity)
        {
            entity.Id = Id;
            entity.Code = Code;
            entity.Name = Description;
        }
    }
}
