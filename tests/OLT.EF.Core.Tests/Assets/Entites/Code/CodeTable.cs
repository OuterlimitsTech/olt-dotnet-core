using OLT.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OLT.EF.Core.Tests.Assets.Entites.Code
{
    public abstract class BaseCodeValueEntity : OltEntityIdDeletable, IOltEntityCodeValue
    {
        [StringLength(50)]
        public virtual string Code { get; set; }

        [StringLength(255), Required]
        public virtual string Name { get; set; }

        public short SortOrder { get; set; } = 9999;
    }

    //TPH Model
    [Table(nameof(CodeTableEntity), Schema = "Code")]
    public abstract class CodeTableEntity : BaseCodeValueEntity
    {
        public string GetDiscriminator()
        {
            return GetType().Name;
        }
    }

    public class StatusTypeCodeTableEntity : CodeTableEntity
    {
        
    }

}
