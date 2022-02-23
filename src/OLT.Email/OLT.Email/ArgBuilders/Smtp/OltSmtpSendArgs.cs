using System.Threading.Tasks;

namespace OLT.Email
{
    public abstract class OltSmtpSendArgs<T> : OltCalendarInviteArgs<T>
      where T : OltSmtpSendArgs<T>
    {
        public abstract OltEmailResult Send();
        public abstract Task<OltEmailResult> SendAsync();
    }


}
