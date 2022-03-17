namespace OLT.Core
{

#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class OltFileBuilderNotFoundException : OltException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public OltFileBuilderNotFoundException(string builderName) : base($"FileBuilder {builderName} not found")
        {

        }
    }
}
