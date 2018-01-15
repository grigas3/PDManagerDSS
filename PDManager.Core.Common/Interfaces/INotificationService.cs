namespace PDManager.Core.Common.Interfaces
{

    /// <summary>
    /// Notification Service interface
    /// </summary>
    public interface INotificationService
    {

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="message">Message parameter</param>
        void SendMessage(IPDMessage message);
    }
}
