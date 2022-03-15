using OLT.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace OLT.EF.Core.Tests.Assets.Entites
{
    public class NoIdEntity : IOltEntitySortable
    {
        [Key]
        [StringLength(50)]
        public string TableKey { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        public short SortOrder { get; set; }

        public static NoIdEntity FakerEntity()
        {
            return new NoIdEntity
            {
                TableKey = $"Key_{Guid.NewGuid()}",
                FirstName = Faker.Name.First()
            };
        }
    }

}
