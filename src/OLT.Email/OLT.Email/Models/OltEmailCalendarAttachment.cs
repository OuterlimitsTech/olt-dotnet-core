namespace OLT.Email
{
    public class OltEmailCalendarAttachment : IOltEmailAttachment
    {
        public const string DefaultFileName = "invite.ics";
        public const string DefaultContentType = "text/calendar";

        public virtual string ContentType => DefaultContentType;
        public virtual string FileName => DefaultFileName;        
        public virtual byte[] Bytes { get; set; }
    }
}