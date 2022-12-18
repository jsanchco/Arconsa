using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using SGDE.Domain.ViewModels;
using System;
using System.Linq;

namespace SGDE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkStatusHistoriesController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<WorkStatusHistoriesController> _logger;

        public WorkStatusHistoriesController(ILogger<WorkStatusHistoriesController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/workStatushistories/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetWorkStatusHistoryById(id);
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
                var workId = Convert.ToInt32(queryString["workId"]);

                var data = _supervisor.GetAllWorkStatusHistory(workId).ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] WorkStatusHistoryViewModel workStatusHistoryViewModel)
        {
            try
            {
                var result = _supervisor.AddWorkStatusHistory(workStatusHistoryViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] WorkStatusHistoryViewModel workStatusHistoryViewModel)
        {
            try
            {
                if (_supervisor.UpdateWorkStatusHistory(workStatusHistoryViewModel) && workStatusHistoryViewModel.id != null)
                {
                    return _supervisor.GetWorkHistoryById((int)workStatusHistoryViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/workstatushistories/5
        [HttpDelete("{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteWorkStatusHistory(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}