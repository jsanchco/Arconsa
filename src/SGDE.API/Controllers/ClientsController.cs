// ReSharper disable InconsistentNaming
namespace SGDE.API.Controllers
{
    #region Using

    using Domain.Supervisor;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;

    #endregion

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(ILogger<ClientsController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/client/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetClientById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public object Get()
        {
            try
            {
                var queryString = Request.Query;
                var enterpriseId = Convert.ToInt32(queryString["enterpriseId"]);
                var allClients = Convert.ToBoolean(queryString["allClients"]);
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var filter = Util.Helper.getSearch(queryString["$filter"]);

                var queryResult = _supervisor.GetAllClient(skip, take, enterpriseId, allClients, filter);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("getclientswithoutfilter")]
        public object GetClientsWithoutFilter()
        {
            try
            {
                var queryResult = _supervisor.GetAllClientWithoutFilter();

                return new { Items = queryResult, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] ClientViewModel clientViewModel)
        {
            try
            {
                var result = _supervisor.AddClient(clientViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] ClientViewModel clientViewModel)
        {
            try
            {
                if (_supervisor.UpdateClient(clientViewModel) && clientViewModel.id != null)
                {
                    return _supervisor.GetClientById((int)clientViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/client/5
        [HttpDelete("{id:int}")]
        //[Route("client/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteClient(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("getallclientslite")]
        public object GetAllClientsLite()
        {
            try
            {
                var queryString = Request.Query;
                var enterpriseId = Convert.ToInt32(queryString["enterpriseId"]);
                var filter = Util.Helper.getSearchLite(queryString["$filter"]);

                return _supervisor.GetAllClientLite(enterpriseId, filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}