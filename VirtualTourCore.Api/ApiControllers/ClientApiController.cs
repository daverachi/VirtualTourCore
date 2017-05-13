using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtualTourCore.Api.Models;
using VirtualTourCore.Common.Logging;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

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
        public Response<Client> GetClientByGuid(Guid guid)
        {
            Response<Client> response = new Response<Client>();
            response.Content = _lookupService.GetClientByGuid(guid);
            if(response.Content == null)
            {
                _log.Error(string.Format("Failed to lookup Client by GUID: {0}", guid));
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            return response;
        }
    }
}
