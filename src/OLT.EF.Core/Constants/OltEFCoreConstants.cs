using System;

namespace OLT.Constants
{
    public static class OltEFCoreConstants
    {
        public const string DefaultAnonymousUser = "GUEST USER";
        public const string DefaultSeedUsername = "SystemLoad";
        public static readonly DateTimeOffset DefaultSeedCreateDate = new DateTimeOffset(1980, 1, 1, 0, 0, 0, 0, DateTimeOffset.UtcNow.Offset);
    }
}
