using System;
using System.ComponentModel.DataAnnotations;
using OLT.Core;


namespace OLT.EF.Core.SeedHelpers.Csv.Tests.Assets
{
    public class PersonTypeCodeEntity : OltEntityId, IOltEntityCodeValue, IOltEntitySortable
    {
        [StringLength(20)]
        public string? Code { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        public short SortOrder { get; set; }
    }
}
