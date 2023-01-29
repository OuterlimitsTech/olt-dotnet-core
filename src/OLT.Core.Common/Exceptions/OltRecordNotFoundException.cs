﻿namespace OLT.Core
{
    public class OltRecordNotFoundException : OltException
    {

        public OltRecordNotFoundException(string message) : base(message)
        {

        }
    }


    public class OltRecordNotFoundException<TServiceMessageEnum> : OltRecordNotFoundException where TServiceMessageEnum : System.Enum
    {
        public OltRecordNotFoundException(TServiceMessageEnum messageType) : base($"{messageType.GetDescription()} Not Found")
        {

        }
    }
}