using System.Web.Mvc;
using Oceanus.Data;
using Oceanus.ViewModels;
using System;
using System.Web.Security;

namespace Oceanus.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public enum SortContentCriteria
        {
            RecentlyAdded = 1,
            MostPopular
        };

        protected OceanusEntities Database { get; private set; }

        protected ControllerBase()
        {
            Database = new OceanusEntities();
        }
    }
}
