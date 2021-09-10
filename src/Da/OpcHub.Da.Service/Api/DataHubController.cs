using System.Web.Http;
using System.Web.Http.Results;
using OpcHub.Da.Contract;
using OpcHub.Da.Service.Hub;

namespace OpcHub.Da.Service.Api
{
    public class DataHubController : ApiController
    {
        [HttpPost]
        public JsonResult<ReadCommandResult> Read([FromBody] ReadCommandRequest request)
        {
            return Json(DataHub.Current.Read(request.Tags, request.ShortPooling));
        }

        [HttpPost]
        public JsonResult<WriteCommandResult> Write([FromBody] WriteCommandRequest request)
        {
            return Json(DataHub.Current.Write(request.ItemValues, request.ShortPolling));
        }
    }
}
