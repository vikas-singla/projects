using System.Web.Mvc;
using Oceanus.Data;

namespace Oceanus.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected OceanusEntities Database { get; private set; }

        public ControllerBase()
        {
            Database = new OceanusEntities();
        }
    }
}
