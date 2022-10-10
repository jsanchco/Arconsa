using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkHistoriesController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<WorkHistoriesController> _logger;

        public WorkHistoriesController(ILogger<WorkHistoriesController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/workhistories/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetWorkHistoryById(id);
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
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var workId = Convert.ToInt32(queryString["workId"]);
                var filter = Util.Helper.getSearch(queryString["$filter"]);

                var queryResult = _supervisor.GetAllWorkHistory(workId, skip, take, filter);
                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] WorkHistoryViewModel workHistoryViewModel)
        {
            try
            {
                var result = _supervisor.AddWorkHistory(workHistoryViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] WorkHistoryViewModel workHistoryViewModel)
        {
            try
            {
                if (_supervisor.UpdateWorkHistory(workHistoryViewModel) && workHistoryViewModel.id != null)
                {
                    return _supervisor.GetWorkHistoryById((int)workHistoryViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/workhistories/5
        [HttpDelete("{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteWorkHistory(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}