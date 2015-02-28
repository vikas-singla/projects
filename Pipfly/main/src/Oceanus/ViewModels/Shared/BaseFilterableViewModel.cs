using System.Collections.Generic;
using System.Web.Mvc;

namespace Oceanus.ViewModels.Shared
{
    public class BaseFilterableViewModel : ViewModelBase
    {
        public static List<SelectListItem> ResultsPerPageOptions =
            new List<SelectListItem>()
                {
                    new SelectListItem(){ Text = "15", Value = "15"},
                    new SelectListItem(){ Text = "25", Value = "25"},
                    new SelectListItem(){ Text = "50", Value = "50"},
                    //new SelectListItem(){ Text = "All", Value = "0"}
                };

        /// <summary>
        /// The number of results to return in a set.
        /// </summary>
        public int ResultsPerPage { get; protected set; }

        /// <summary>
        /// The page of results (used in conjunction with ResultsPerPage).
        /// </summary>
        public int Page { get; protected set; }

        /// <summary>
        /// The total number of pages the result set contains.
        /// </summary>
        public int TotalPages { get; protected set; }

        /// <summary>
        /// The total number of results given the query.
        /// </summary>
        public int TotalResults { get; protected set; }

    }
}