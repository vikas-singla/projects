using System;

namespace Oceanus.ViewModels
{
    public class BaseMessageViewModel
    {
        /// <summary>
        /// Reference to the ID of the message
        /// </summary>
        public int? MessageId { get; set; }

        /// <summary>
        /// Subject of the message
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Initial timestamp for when the message was first created
        /// </summary>
        public DateTime InitialSendTimestamp { get; set; }

        /// <summary>
        /// Timestamp for when the message was last updated
        /// </summary>
        public DateTime LastUpdatedOn { get; set; }

        /// <summary>
        /// Flag: Indicates if the message has been flagged as spam
        /// </summary>
        public bool? IsSpam { get; set; }

        /// <summary>
        /// Reference to the ID of the user who originally sent the message
        /// </summary>
        public Guid InitiatingUserId { get; set; }

        /// <summary>
        /// Reference to the user who originally sent the message
        /// </summary>
        public BaseUserViewModel InitiatingUser { get; set; }

        /// <summary>
        /// Flag: Indicates if this message is unread (because it's new or because of responses) for currently logged in user
        /// </summary>
        public bool MessageIsUnreadForCurrentUser { get; set; }

        public BaseMessageViewModel()
        {                
        }

        public BaseMessageViewModel(int? messageId_, string subject_, DateTime initialSendTimestamp_, DateTime lastUpdatedOn_, bool? isSpam_, Guid initiatingUserId_)
        {
            MessageId = messageId_;
            Subject = subject_;
            InitialSendTimestamp = initialSendTimestamp_;
            LastUpdatedOn = lastUpdatedOn_;
            IsSpam = isSpam_;
            InitiatingUserId = initiatingUserId_;
            MessageIsUnreadForCurrentUser = false;
        }
    }
}