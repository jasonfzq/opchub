using OpcHub.Ae.Contract;
using OpcHub.Ae.Service.Health;
using OpcHub.Ae.Service.Hub;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace OpcHub.Ae.Service.Api
{
    public class EventHubController : ApiController
    {
        [HttpGet]
        public JsonResult<AeHealthInfo> GetHealthInfo()
        {
            return Json(HealthMonitor.Current.GetHealthStatus());
        }

        [HttpGet]
        public JsonResult<List<AeEvent>> GetQueuedEvents()
        {
            return Json(AeEventHub.Current.GetQueuedEvents());
        }
    }
}
