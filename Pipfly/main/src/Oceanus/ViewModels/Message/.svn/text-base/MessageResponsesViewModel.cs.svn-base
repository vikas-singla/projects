using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class MessageResponsesViewModel
    {
        /// <summary>
        /// ID for the message response
        /// </summary>
        public int? MessageResponseId { get; set; }
        
        /// <summary>
        /// Reference to the parent Message
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Text in the message response
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Timestamp for when the response was added
        /// </summary>
        public DateTime ContributionDate { get; set; }

        /// <summary>
        /// Reference to the ID of the response contributing user
        /// </summary>
        public Guid ContributingUserId { get; set; }

        /// <summary>
        /// Reference to the response contributing user
        /// </summary>
        public BaseUserViewModel ContributingUser { get; set; }

        public MessageResponsesViewModel()
        {
        }

        public MessageResponsesViewModel(int? messageResponseId_, int messageId_, string text_, DateTime contributionDate_, Guid contributingUserId_)
        {
            MessageResponseId = messageResponseId_;
            MessageId = messageId_;
            Text = text_;
            ContributionDate = contributionDate_;
            ContributingUserId = contributingUserId_;
        }
    }
}