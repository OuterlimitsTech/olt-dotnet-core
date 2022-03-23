using OLT.Core;

namespace OLT.AspNetCore.Tests.Assets
{

    public enum SecurityPermissions
    {
        [Code("read-only")]
        ReadOnly = 1000,

        [Code("update-data")]
        UpdateData = 4000,
    }
}
