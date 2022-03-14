using Microsoft.EntityFrameworkCore.ChangeTracking;
using OLT.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OLT.EF.Core.Tests.Assets.Entites
{
    [Table("People")]
    public class PersonEntity : OltEntityIdDeletable, IOltInsertingRecord, IOltUpdatingRecord, IOltDeletingRecord, IOltEntityUniqueId
    {
        public Guid UniqueId { get; set; }

        [StringLength(100)]
        public string NameFirst { get; set; }
        [StringLength(100)]
        public string NameMiddle { get; set; }
        [StringLength(100)]
        public string NameLast { get; set; }

        [StringLength(20)]
        public string ActionCode { get; set; }

        [StringLength(20)]
        [NotMapped]
        public string NoMapColumn { get; set; }

        public void InsertingRecord(IOltDbContext db, EntityEntry entityEntry)
        {
            ActionCode = "Insert";
        }

        public void UpdatingRecord(IOltDbContext db, EntityEntry entityEntry)
        {
            ActionCode = "Update";
        }

        public void DeletingRecord(IOltDbContext db, EntityEntry entityEntry)
        {
            DeletedOn = DateTimeOffset.Now;
            DeletedBy = db.AuditUser;
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
    }

}
