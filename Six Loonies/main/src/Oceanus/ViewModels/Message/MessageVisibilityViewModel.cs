using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class MessageVisibilityViewModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int? MessageVisibilityId { get; set; }

        /// <summary>
        /// Reference to the message
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Reference to the ID of the user who obtains the visibility
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Reference to the user who obtains the visibility
        /// </summary>
        public BaseUserViewModel User { get; set; }

        /// <summary>
        /// Date timestamp for when the message visibility was added
        /// </summary>
        public DateTime AddedOn { get; set; }

        /// <summary>
        /// Flag: Indicates if the message thread is unread by the user (based on last responses)
        /// </summary>
        public bool MessageIsUnread { get; set; }

        public MessageVisibilityViewModel()
        {                
        }

        public MessageVisibilityViewModel(int? messageVisibilityId_, int messageId_, Guid userId_, DateTime addedOn_, bool messageIsUnread_)
        {
            MessageVisibilityId = messageVisibilityId_;
            MessageId = messageId_;
            UserId = userId_;
            AddedOn = addedOn_;
            MessageIsUnread = messageIsUnread_;
        }
    }
}