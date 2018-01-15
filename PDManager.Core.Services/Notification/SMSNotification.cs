using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDManager.Core.Service.Notification
{

    /// <summary>
    /// SMS Notification implementation based on yuboto services
    /// </summary>
    public static class SMSNotification
    {

        /// <summary>
        /// Send SMS Through Yuboto
        /// </summary>
        /// <param name="from">From</param>
        /// <param name="phoneNumber">Phone Number</param>
         /// <param name="message">Message</param>
        /// <param name="username">User namey</param>
        /// <param name="password">Password</param>        
        /// <param name="accessKey">Acccess Key</param>
        /// <returns></returns>
        public static string Notify(string from,  string phoneNumber, string message, string username, string password, string accessKey)
        {
            StringBuilder url = new StringBuilder();

            message = string.Format(message, accessKey);

            url.Append("http://services.yuboto.com/sms/api/smsc.asp?");

            url.Append(String.Format("user={0}", username));

            url.Append(String.Format("&pass={0}", password));

            url.Append("&action=send");

            url.Append("&from=" + from);

            url.Append("&to=" + phoneNumber);

            url.Append("&text=" + message);

            return url.ToString();
        }
    }
}
