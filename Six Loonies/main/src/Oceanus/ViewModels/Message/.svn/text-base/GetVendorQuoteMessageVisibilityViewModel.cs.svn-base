using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class GetVendorQuoteMessageVisibilityViewModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int? VendorQuoteMessageVisibilityId { get; set; }

        /// <summary>
        /// Reference to the message
        /// </summary>
        public int VendorQuoteMessageId { get; set; }

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

        public GetVendorQuoteMessageVisibilityViewModel()
        {                
        }

        public GetVendorQuoteMessageVisibilityViewModel(int? vendorQuoteMessageVisibilityId_, int vendorQuoteMessageId_, Guid userId_, DateTime addedOn_, bool messageIsUnread_)
        {
            VendorQuoteMessageVisibilityId = vendorQuoteMessageVisibilityId_;
            VendorQuoteMessageId = vendorQuoteMessageId_;
            UserId = userId_;
            AddedOn = addedOn_;
            MessageIsUnread = messageIsUnread_;
        }
    }
}