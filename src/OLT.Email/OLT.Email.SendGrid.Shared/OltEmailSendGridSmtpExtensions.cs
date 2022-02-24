using System;
using System.Collections.Generic;
using System.Text;

namespace OLT.Email.SendGrid.Common
{
    public static class OltEmailSendGridSmtpExtensions
    {

        /// <summary>
        /// Sends email with exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="server"></param>
        /// <param name="rethrowException"></param>
        public static void OltEmailError(this Exception ex, string apiKey, IOltApplicationErrorEmail template, bool rethrowException = false)
        {
            OltSmtpEmailExtensions.OltEmailError(ex, new OltSmtpServerSendGrid(apiKey), template, rethrowException);
        }

    }
}
