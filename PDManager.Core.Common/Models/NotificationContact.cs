using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Models
{
    /// <summary>
    /// Notification Contact
    /// </summary>
    public class NotificationContact
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contact Device Uri. This is the device uri  for GCM or FCM notifications, email for Email notifications and phone number for SMS notifications
        /// </summary>
        public string Uri { get; }


        /// <summary>
        /// User or system preferred message type
        /// </summary>
        public PDMessageType PreferredMessageType { get;  }


    }
}
