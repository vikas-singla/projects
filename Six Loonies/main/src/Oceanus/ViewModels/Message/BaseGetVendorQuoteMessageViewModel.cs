using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class BaseGetVendorQuoteMessageViewModel
    {
        /// <summary>
        /// Reference to the ID of the message
        /// </summary>
        public int? VendorQuoteMessageId { get; set; }

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
        /// Reference to the ID of the user who originally sent the message
        /// </summary>
        public Guid InitiatingUserId { get; set; }

        /// <summary>
        /// Not stored in DB, Used by UI
        /// </summary>
        public int LastStatedPriceInMessage { get; set; }

        /// <summary>
        /// Reference to the user who originally sent the message
        /// </summary>
        public BaseUserViewModel InitiatingUser { get; set; }

        /// <summary>
        /// Flag: Indicates if this message is unread (because it's new or because of responses) for currently logged in user
        /// </summary>
        public bool MessageIsUnreadForCurrentUser { get; set; }

        public BaseGetVendorQuoteMessageViewModel()
        {                
        }

        public BaseGetVendorQuoteMessageViewModel(int? vendorQuoteMessageId_, string subject_, DateTime initialSendTimestamp_, DateTime lastUpdatedOn_, Guid initiatingUserId_)
        {
            VendorQuoteMessageId = vendorQuoteMessageId_;
            Subject = subject_;
            InitialSendTimestamp = initialSendTimestamp_;
            LastUpdatedOn = lastUpdatedOn_;
            InitiatingUserId = initiatingUserId_;
            MessageIsUnreadForCurrentUser = false;
        }
    }
}