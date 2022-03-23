using OLT.Core;
using System.Collections.Generic;
using System.Linq;

namespace OLT.AspNetCore.Tests.Assets
{
    public class RequirePermissionAttribute : OltRequirePermissionAttribute
    {
        public RequirePermissionAttribute(params SecurityPermissions[] permissions)
        {
            RoleClaims = permissions.Select(s => s.GetCodeEnum()).ToList();
        }

        public override List<string> RoleClaims { get; }
    }

    public enum SecurityPermissions
    {
        [Code("read-only")]
        ReadOnly = 1000,

        [Code("update-data")]
        UpdateData = 4000,
    }
}
