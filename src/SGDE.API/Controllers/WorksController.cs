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
    public class WorksController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<WorksController> _logger;

        public WorksController(ILogger<WorksController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/work/5
        [HttpGet("{id}/{selectedTab:int?}")]
        public object Get(int id, int? selectedTab = null)
        {
            try
            {
                return _supervisor.GetWorkById(id);
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
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var filter = Util.Helper.getSearch(queryString["$filter"]);
                var clientId = Convert.ToInt32(queryString["clientId"]);
                var showCloseWorks = Convert.ToBoolean(queryString["showCloseWorks"]);

                var queryResult = _supervisor.GetAllWork(skip, take, enterpriseId, filter, clientId, showCloseWorks);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] WorkViewModel workViewModel)
        {
            try
            {
                var result = _supervisor.AddWork(workViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] WorkViewModel workViewModel)
        {
            try
            {
                if (_supervisor.UpdateWork(workViewModel) && workViewModel.id != null)
                {
                    return _supervisor.GetWorkById((int)workViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut("updatedateswork")]
        public object UpdateDatesWork([FromBody] WorkViewModel workViewModel)
        {
            try
            {
                if (_supervisor.UpdateDatesWork(workViewModel) && workViewModel.id != null)
                {
                    return _supervisor.GetWorkById((int)workViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/work/5
        [HttpDelete("{id:int}")]
        //[Route("work/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteWork(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("getallworkslite")]
        public object GetAllWorksLite(int clientId = 0)
        {
            try
            {
                var queryString = Request.Query;
                var filter = Util.Helper.getSearchLite(queryString["$filter"]);

                return _supervisor.GetAllWorkLite(filter, clientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("getclosepage/{workId:int}")]
        public object GetClosePage(int workId)
        {
            try
            {
                return _supervisor.GetWorkClosePage(workId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}