namespace OLT.Email
{
    public class OltEmailCalendarAttachment : IOltEmailAttachment
    {
        public const string DefaultFileName = "invite.ics";
        public const string DefaultTextCalendar = "text/calendar";

        public virtual string ContentType => DefaultTextCalendar;
        public virtual string FileName => DefaultFileName;        
        public virtual byte[] Bytes { get; set; }
    }
}