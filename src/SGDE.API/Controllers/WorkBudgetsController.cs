using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using SGDE.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkBudgetsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<WorkBudgetsController> _logger;

        public WorkBudgetsController(ILogger<WorkBudgetsController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/workbudget/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetWorkBudgetById(id);
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
                var workBudgetDataId = Convert.ToInt32(queryString["workBudgetDataId"]);

                var data = _supervisor.GetAllWorkBudget(workId, workBudgetDataId).ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] WorkBudgetViewModel workBudgetViewModel)
        {
            try
            {
                var result = _supervisor.AddWorkBudget(workBudgetViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] WorkBudgetViewModel workBudgetViewModel)
        {
            try
            {
                if (_supervisor.UpdateWorkBudget(workBudgetViewModel) && workBudgetViewModel.id != null)
                {
                    return _supervisor.GetWorkBudgetById((int)workBudgetViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/workbudget/5
        [HttpDelete("{id:int}")]
        //[Route("userdocument/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteWorkBudget(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("removeall")]
        public object RemoveAll([FromBody] List<int> listRemove)
        {
            try
            {
                var result = true;
                foreach (var item in listRemove)
                {
                    if (!_supervisor.DeleteWorkBudget(item))
                    {
                        result = false;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getallbudgetslite")]
        public object GetAllBudgetsLite(int workId = 0)
        {
            try
            {
                return _supervisor.GetAllWorkBudgetLite(workId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}