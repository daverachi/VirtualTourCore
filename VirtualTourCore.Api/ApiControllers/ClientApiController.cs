using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtualTourCore.Common.Logging;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Mapping;

namespace VirtualTourCore.Api.ApiControllers
{
    public class ClientApiController : ApiController
    {
        private INlogger _log;
        private ILookupService _lookupService;

        public ClientApiController(INlogger log, ILookupService lookupService)
        {
            _log = log;
            _lookupService = lookupService;
        }
        public HttpResponseMessage GetClientByGuid(Guid guid)
        {
            var result = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Client code does not have an active account");
            var clientEntity = _lookupService.GetClientByGuid(guid);
            if(clientEntity == null)
            {
                _log.Error(string.Format("Failed to lookup Client by GUID: {0}", guid));
            }
            else
            {
                var client = ClientMapper.ToDataTransferObject(clientEntity);
                var locations = _lookupService.GetLocationsByClientId(clientEntity.Id);
                if(locations != null && locations.Count() > 0)
                {
                    client.Locations = locations.Select(x => LocationMapper.ToDataTransferObject(x)).ToList();
                }
                result = Request.CreateResponse(HttpStatusCode.OK, client);
            }
            result.Headers.Add("Access-Control-Allow-Origin", "*");
            return result;
        }
    }
}
