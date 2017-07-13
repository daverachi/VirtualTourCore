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
    public class AreaApiController : ApiController
    {
        private INlogger _log;
        private ILookupService _lookupService;

        public AreaApiController(INlogger log, ILookupService lookupService)
        {
            _log = log;
            _lookupService = lookupService;
        }
        public HttpResponseMessage GetAreaByGuidAndId(Guid guid, int id)
        {
            var result = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Client code does not have an active account");
            var clientEntity = _lookupService.GetClientByGuid(guid);
            if (clientEntity == null)
            {
                _log.Error(string.Format("Failed to lookup Client by GUID: {0}", guid));
            }
            else
            {
                var areaEntity = _lookupService.GetAreaByIdAndClientId(new List<string> { clientEntity.Id.ToString() }, id);
                var area = new AreaDTO();
                if (areaEntity != null)
                {
                    area = AreaMapper.ToDataTransferObject(areaEntity);
                    area.Tours = _lookupService.GetToursByAreaId(area.Id).Select(x => TourMapper.ToDataTransferObject(x)).ToList();
                }
                result = Request.CreateResponse(HttpStatusCode.OK, area);
            }
            return result;
        }
    }
}
