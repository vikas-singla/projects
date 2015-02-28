using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MVCControlsToolkit.Controller;
using MVCControlsToolkit.Linq;
using Oceanus.Data;

namespace Oceanus.ViewModels
{
    public class ListingsViewModel : ViewModelBase
    {
        public CategoryViewModel CategoryView { get; set; }
        public IEnumerable<ListingViewModel> Listings { get; set; }

        // paging & filtering
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PreviousPage { get; set; }
        public List<Tracker<ListingViewModel>> ListingList { get; set; }
        public List<KeyValuePair<LambdaExpression, OrderType>> ListingOrder { get; set; }
        public Expression<Func<ListingViewModel, bool>> ListingFilter { get; set; }

        public static List<Tracker<ListingViewModel>> GetPage(
            int resultsPerPage,
            out int totalPages,
            ref List<KeyValuePair<LambdaExpression, OrderType>> order,
            int page = 1,
            Expression<Func<ListingViewModel, bool>> filter = null)
        {
            if (order == null)
            {
                order = new List<KeyValuePair<LambdaExpression, OrderType>>();

            }

            //paging require ordering! Therefore we always need to add a default oredering
            if (order.Count == 0)
            {
                Expression<Func<ListingViewModel, string>> defaultOrder = m => m.Name;
                order.Add(new KeyValuePair<LambdaExpression, OrderType>(defaultOrder, OrderType.Ascending));
            }

            using (var context = new OceanusEntities())
            {
                int rowCount;

                if (filter == null)
                {
                    rowCount = context.Services.Count();
                }
                else
                {
                    rowCount = (from x in context.Services
                                select new ListingViewModel()
                                {
                                    Name = x.Name, 
                                    Id = x.Id
                                }).Where(filter).Count();
                }
                
                if (rowCount == 0)
                {
                    totalPages = 0;
                    return new List<Tracker<ListingViewModel>>();
                }
                
                totalPages = rowCount / resultsPerPage;

                if (rowCount % resultsPerPage > 0)
                {
                    totalPages++;
                }
                
                int toSkip = (page - 1) * resultsPerPage;

                List<Tracker<ListingViewModel>> result;

                if (filter == null)
                {
                    result = context.Services
                        .Select(item => 
                            new ListingViewModel()
                            {
                                Id = item.Id, 
                                Name = item.Name
                            })
                        .ApplyOrder(order)
                        .Select(viewItem =>
                            new Tracker<ListingViewModel>
                            {
                                Value = viewItem,
                                OldValue = viewItem,
                                Changed = false
                            })
                        .Skip(toSkip)
                        .Take(resultsPerPage)
                        .ToList();
                }
                else
                {
                    result = context.Services
                        .Select(item =>
                            new ListingViewModel()
                            {
                                Id = item.Id,
                                Name = item.Name
                            })
                        .Where(filter)
                        .ApplyOrder(order)
                        .Select(viewItem =>
                            new Tracker<ListingViewModel>
                            {
                                Value = viewItem,
                                OldValue = viewItem,
                                Changed = false
                            })
                        .Skip(toSkip)
                        .Take(resultsPerPage)
                        .ToList();
                }

                return result;
            }
        }
    }
}