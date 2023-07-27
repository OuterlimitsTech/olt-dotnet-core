using System;

namespace OLT.Core
{

#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    [Obsolete("Removing in 8.x")]
    public class OltFileBuilderNotFoundException : OltException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public OltFileBuilderNotFoundException(string builderName) : base($"FileBuilder {builderName} not found")
        {

        }
    }
}
