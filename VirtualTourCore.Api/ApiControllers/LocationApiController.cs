using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtualTourCore.Common.Logging;
using VirtualTourCore.Common.ValueObjects;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Mapping;

namespace VirtualTourCore.Api.ApiControllers
{
    public class LocationApiController : ApiController
    {
        private INlogger _log;
        private ILookupService _lookupService;

        public LocationApiController(INlogger log, ILookupService lookupService)
        {
            _log = log;
            _lookupService = lookupService;
        }
        public HttpResponseMessage GetLocationByGuidAndId(Guid guid, int id)
        {
            var result = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Client code does not have an active account");
            var clientEntity = _lookupService.GetClientByGuid(guid);
            if (clientEntity == null)
            {
                _log.Error(string.Format("Failed to lookup Client by GUID: {0}", guid));
            }
            else
            {
                var locationEntity = _lookupService.GetLocationByIdAndClientId(new List<string>{ clientEntity.Id.ToString() }, id);
                var location = new LocationDTO();
                if (locationEntity != null)
                {
                    location = LocationMapper.ToDataTransferObject(locationEntity);
                    location.Areas = _lookupService.GetAreasByLocationId(location.Id).Select(x => AreaMapper.ToDataTransferObject(x)).ToList();
                }
                result = Request.CreateResponse(HttpStatusCode.OK, location);
            }
            return result;
        }
    }
}
