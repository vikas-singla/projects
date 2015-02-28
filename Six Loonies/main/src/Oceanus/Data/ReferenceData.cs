using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Oceanus.Data
{
    public class ReferenceData
    {

        #region singleton

        private static ReferenceData _instance;

        public static ReferenceData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReferenceData();
                }

                return _instance;
            }
        }

        #endregion


        #region properties

        public List<SelectListItem> AvailableCities { get; private set; }

        #endregion


        #region constructors

        public void Initialize()
        {
            //InitializeAvailableCities();
        }

        #endregion


        #region helper methods

        //private void InitializeAvailableCities()
        //{
        //    using (var context = new OceanusEntities())
        //    {
        //        var results =
        //            from city in context.VendorLocations
        //            group city by city.ReferenceCitiesId
        //            into groups
        //            select groups.FirstOrDefault();

        //        AvailableCities =
        //            (from result in results.AsEnumerable().Where(r => r.ReferenceCity.IsInitialMarketingTarget)
        //             orderby result.ReferenceCity.City
        //             select new SelectListItem()
        //            {
        //                Text = string.Format("{0}, {1}", result.ReferenceCity.AccentCity, result.ReferenceCity.Region),
        //                Value = result.ReferenceCitiesId.ToString()
        //            }).ToList();
        //    }

        //    AvailableCities.Insert(0, new SelectListItem(){Text = "Select a City", Value = "0"});

        //    StringBuilder sb = new StringBuilder();
        //    foreach (var city in AvailableCities)
        //    {
        //        sb.AppendFormat("{0}, ", city.Text);
        //    }
        //    if (sb.Length > 2)
        //    {
        //        sb.Remove(sb.Length - 2, 2);
        //    }

        //    Logging.Logger.Instance.Info(string.Format("Reference cities loaded, the following cities are available: {0}.", sb.ToString()));
        //}

        #endregion
    }
}