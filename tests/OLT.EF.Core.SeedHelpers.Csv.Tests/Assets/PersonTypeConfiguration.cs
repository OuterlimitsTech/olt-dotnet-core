using System.Collections.Generic;
using System.Reflection;
using OLT.Core;


namespace OLT.EF.Core.SeedHelpers.Csv.Tests.Assets
{
    public class PersonTypeConfiguration : OltEntityTypeConfigurationFromCsv<PersonTypeCodeEntity, PersonTypeCsvModel>
    {
        protected override string ResourceName => "person_type.csv";
        protected override Assembly ResourceAssembly => this.GetType().Assembly;
        public List<PersonTypeCodeEntity> Results { get; } = new List<PersonTypeCodeEntity>();

        protected override void Map(PersonTypeCodeEntity entity, PersonTypeCsvModel csvRecord)
        {
            base.Map(entity, csvRecord);
            Results.Add(entity);
        }
    }
}
