using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class ErrorMessageViewModel
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        public ErrorMessageViewModel(string message_)
        {
            Message = message_;
        }
    }
}