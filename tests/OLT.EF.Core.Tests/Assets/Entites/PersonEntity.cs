using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OLT.EF.Core.Tests.Assets.Entites
{        
    public class PersonEntity : OltEntityIdDeletable, IOltInsertingRecord, IOltUpdatingRecord, IOltEntityUniqueId
    {
        public Guid UniqueId { get; set; }

        [MaxLength(50)]
        [Required]
        public string NameFirst { get; set; }
        [MaxLength(50)]
        public string NameMiddle { get; set; }
        [MaxLength(50)]
        [Required]
        public string NameLast { get; set; }

        [MaxLength(20)]
        public string ActionCode { get; set; }

        [MaxLength(20)]
        [NotMapped]
        public string NoMapColumn { get; set; }

        [Column("Effective", TypeName = "Date")]
        public DateTime? EffectiveDate { get; set; }  //Used to Test GetColumns<> where the DB type is different from the ClrType


        public int? StatusTypeId { get; set; }
        public StatusTypeCodeTableEntity StatusType { get; set; }

        public List<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();

        public void InsertingRecord(IOltDbContext db, EntityEntry entityEntry)
        {
            ActionCode = "Insert";
        }

        public void UpdatingRecord(IOltDbContext db, EntityEntry entityEntry)
        {
            ActionCode = "Update";
        }

        public static PersonEntity FakerEntity()
        {
            return new PersonEntity
            {
                NameFirst = Faker.Name.First(),
                NameMiddle = Faker.Name.Middle(),
                NameLast = Faker.Name.Last(),
            };
        }

        public static List<PersonEntity> FakerList(int number)
        {
            var list = new List<PersonEntity>();

            for (int i = 0; i < number; i++)
            {
                list.Add(FakerEntity());
            }

            return list;
        }
    }

}
