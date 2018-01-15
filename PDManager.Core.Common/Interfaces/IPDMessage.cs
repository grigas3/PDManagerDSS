
using PDManager.Core.Common.Models;

namespace PDManager.Core.Common.Interfaces
{
    /// <summary>
    /// Base PD Message interface
    /// </summary>
     public interface IPDMessage
    {
        /// <summary>
        /// Sender identification
        /// </summary>
         string Sender { get; set; }


        /// <summary>
        /// PD Manager Sender Uri
        /// </summary>
         string SenderUri { get; set; }



        /// <summary>
        /// Receiver
        /// </summary>
         string Receiver { get; set; }

        /// <summary>
        /// Receiver
        /// </summary>
         string ReceiverUri { get; set; }


        /// <summary>
        /// 
        /// </summary>
        PDMessageType MessageType { get; set; }


        /// <summary>
        /// Message Body
        /// </summary>
         string Body { get; set; }


        /// <summary>
        /// Message Subject
        /// </summary>
         string Subject { get; set; }
    }
}
