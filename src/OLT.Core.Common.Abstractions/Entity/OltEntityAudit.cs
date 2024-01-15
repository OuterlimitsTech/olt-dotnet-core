using System;
using System.ComponentModel.DataAnnotations;

namespace OLT.Core
{
    public abstract class OltEntityAudit : IOltEntityAudit
    {
       
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        [StringLength(100)]
        public string CreateUser { get; set; } = OLT.Constants.OltCommonDefaults.UnknownCreateUser;

        public DateTimeOffset? ModifyDate { get; set; }

        [StringLength(100)]
        public string? ModifyUser { get; set; }

    }
}
