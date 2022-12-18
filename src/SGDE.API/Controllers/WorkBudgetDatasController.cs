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
    public class WorkBudgetDatasController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<WorkBudgetDatasController> _logger;

        public WorkBudgetDatasController(ILogger<WorkBudgetDatasController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/workbudgetdatas/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetWorkBudgetDataById(id);
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

                var data = _supervisor.GetAllWorkBudgetData(workId).ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] WorkBudgetDataViewModel workBudgetDataViewModel)
        {
            try
            {
                var result = _supervisor.AddWorkBudgetData(workBudgetDataViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] WorkBudgetDataViewModel workBudgetDataViewModel)
        {
            try
            {
                if (_supervisor.UpdateWorkBudgetData(workBudgetDataViewModel) && workBudgetDataViewModel.id != null)
                {
                    return _supervisor.GetWorkBudgetDataById((int)workBudgetDataViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/workbudgetdatas/5
        [HttpDelete("{id:int}")]
        //[Route("userdocument/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteWorkBudgetData(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}