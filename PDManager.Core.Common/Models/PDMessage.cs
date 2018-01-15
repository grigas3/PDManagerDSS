using PDManager.Core.Common.Interfaces;

namespace PDManager.Core.Common.Models
{
    /// <summary>
    /// A core message implementation
    /// </summary>
    public class PDMessage:IPDMessage
    {

        /// <summary>
        /// Sender identification
        /// </summary>
        public string Sender { get; set; }


        /// <summary>
        /// PD Manager Sender Uri
        /// </summary>
        public string SenderUri { get; set; }



        /// <summary>
        /// Receiver
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// Receiver
        /// </summary>
        public string ReceiverUri { get; set; }


        /// <summary>
        /// Message Type
        /// </summary>
        public PDMessageType MessageType { get; set; }


        /// <summary>
        /// Message Body
        /// </summary>
        public string Body { get; set; }


        /// <summary>
        /// Message Subject
        /// </summary>
        public string Subject { get; set; }

    }
}
