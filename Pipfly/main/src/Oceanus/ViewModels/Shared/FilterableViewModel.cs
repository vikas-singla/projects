using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MVCControlsToolkit.Linq;

namespace Oceanus.ViewModels.Shared
{
    public class FilterableViewModel<TEntity, TViewModel> : BaseFilterableViewModel
        where TViewModel : new()
    {

        #region properties

        /// <summary>
        /// The transform to perform from the model to the view model.
        /// </summary>
        public Func<TEntity, TViewModel> Transform { get; set; }

        /// <summary>
        /// The order to apply to the result set
        /// </summary>
        public List<KeyValuePair<LambdaExpression, OrderType>> OrderByExpressions { get; private set; }

        /// <summary>
        /// The result set items.
        /// </summary>
        public List<TViewModel> Results { get; private set; }

        /// <summary>
        /// The linq query to perform as part of the paging / filtering
        /// </summary>
        public Func<IEnumerable<TEntity>> FilterAction { get; set; }
        
        #endregion


        #region constructors

        /// <summary>
        /// Creates a new instance of the filterable viewmodel class.
        /// </summary>
        public FilterableViewModel()
        {
            OrderByExpressions = new List<KeyValuePair<LambdaExpression, OrderType>>();
            Page = 1;
            Results = new List<TViewModel>();
        }

        #endregion


        #region public methods

        /// <summary>
        /// Filters the context and makes results accessible fia the Results property.
        /// </summary>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="page">The page.</param>
        public void Filter(int resultsPerPage, int page = 1)
        {
            if (FilterAction == null)
            {
                throw new Exception("Filter action must be set");
            }

            var viewModels = FilterAction().ApplyOrder(OrderByExpressions);

            int rowCount = viewModels.Count();

            if (rowCount == 0)
            {
                TotalPages = 0;
                Results = new List<TViewModel>();
            }

            // make results per page as many as there are results if input was 0.
            if (resultsPerPage == 0)
            {
                resultsPerPage = rowCount + 1;
            }

            TotalPages = rowCount / resultsPerPage;

            if (rowCount % resultsPerPage > 0)
            {
                TotalPages++;
            }

            // cooerce results if page parameter is outside of the results
            if (page > TotalPages)
            {
                page = 1;
            }

            int toSkip = (page - 1) * resultsPerPage;

            TotalResults = rowCount;

            // page
            var pageResults = viewModels
                .Skip(toSkip)
                .Take(resultsPerPage);

            // flush results to view models
            var finalResults = pageResults
                .AsEnumerable()
                .Select(model => Transform(model))
                .ToList();

            Results = finalResults;

            if (Results.Count > 0)
            {
                Page = page;
            }

            ResultsPerPage = resultsPerPage;
        }

        #endregion

    }
}