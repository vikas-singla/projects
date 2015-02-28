using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class GetVendorQuoteMessageResponsesViewModel
    {
        /// <summary>
        /// ID for the message response
        /// </summary>
        public int? VendorQuoteMessageResponseId { get; set; }
        
        /// <summary>
        /// Reference to the parent Message
        /// </summary>
        public int VendorQuoteMessageId { get; set; }

        /// <summary>
        /// Text in the message response
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Optional price specified in the response
        /// </summary>
        public double? Price { get; set; }

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

        public GetVendorQuoteMessageResponsesViewModel()
        {
        }

        public GetVendorQuoteMessageResponsesViewModel(int? vendorQuoteMessageResponseId_, int vendorQuoteMessageId_, string text_, double? price_, DateTime contributionDate_, Guid contributingUserId_)
        {
            VendorQuoteMessageResponseId = vendorQuoteMessageResponseId_;
            VendorQuoteMessageId = vendorQuoteMessageId_;
            Text = text_;
            Price = price_;
            ContributionDate = contributionDate_;
            ContributingUserId = contributingUserId_;
        }
    }
}