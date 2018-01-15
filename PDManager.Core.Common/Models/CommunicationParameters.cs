namespace PDManager.Core.Common.Models
{


    /// <summary>
    /// Communication Parameters
    /// </summary>
    public class CommunicationParameters
    {

        /// <summary>
        /// Email account user Name
        /// </summary>
        public string EmailUserName { get; set; }

        /// <summary>
        /// Email account password
        /// </summary>
        public string EmailPasword { get; set; }

        /// <summary>
        /// Email Server
        /// </summary>
        public string EmailServer{ get; set; }

        /// <summary>
        /// Email Port
        /// </summary>
        public string EmailPort { get; set; }


        /// <summary>
        /// SMS (Yuboto) user name
        /// </summary>
        public string SMSUserName { get; set; }
        /// <summary>
        /// SMS (Yuboto) password
        /// </summary>
        public string SMSPasword { get; set; }

        /// <summary>
        /// SMS (Yuboto) key
        /// </summary>
        public string SMSKey { get; set; }



        /// <summary>
        /// GCM Application iD
        /// Retrieved from Google developer Console
        /// </summary>
        public string GCMAppID { get; set; }

        /// <summary>
        /// FCM Application iD
        /// Retrieved from Google developer Console
        /// </summary>
        public string FCMAppID { get; set; }



        /// <summary>
        /// GCM Sender iD
        /// Retrieved from Google developer Console
        /// </summary>
        public string FCMSenderID { get; set; }

        /// <summary>
        /// GCM Sender iD
        /// Retrieved from Google developer Console
        /// </summary>
        public string GCMSenderID { get; set; }

    }
}
