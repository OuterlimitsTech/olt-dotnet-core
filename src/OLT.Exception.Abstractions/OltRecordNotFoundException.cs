namespace OLT.Core
{
    /// <summary>
    /// Record Not Found
    /// </summary>
    public class OltRecordNotFoundException : OltException
    {

        public OltRecordNotFoundException(string message) : base(message)
        {

        }
    }


    public class OltRecordNotFoundException<TServiceMessageEnum> : OltRecordNotFoundException where TServiceMessageEnum : System.Enum
    {
        public OltRecordNotFoundException(TServiceMessageEnum messageType) : base($"{messageType.GetDescriptionInternal()} Not Found")
        {

        }
    }
}