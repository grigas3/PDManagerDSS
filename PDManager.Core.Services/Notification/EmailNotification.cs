using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PDManager.Core.Service.Notification
{
    /// <summary>
    /// A Email Notification implementation
    /// </summary>
    public static class EmailNotification
    {
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        /// <param name="body">body</param>
        /// <param name="subject">Subject</param>
        /// <param name="userName">User Name</param>
        /// <param name="password">Passwrod</param>
        /// <param name="smtpServer">Server</param>
        /// <param name="smtpServerPort">Port</param>
        /// <param name="cc">CC</param>
        public static void Notify(string from, string to, string body, string subject, string userName, string password, string smtpServer, string smtpServerPort, IEnumerable<string> cc)
        {
            using (var mail = new MailMessage(from, to, subject, body))
            {

                mail.IsBodyHtml = true;
                if (cc != null)
                {
                    foreach (var c in cc)
                        mail.CC.Add(c);

                }

                try
                {

                    var port = int.Parse(smtpServerPort);
                    var sc = new SmtpClient(smtpServer, port);      // port=25

                    if (!string.IsNullOrEmpty(userName))
                        sc.Credentials = new System.Net.NetworkCredential(userName, password);
                    sc.Send(mail);
                }
                catch (SmtpException ex)
                {
                    throw ex;
                }
            }
        }
    }
}
