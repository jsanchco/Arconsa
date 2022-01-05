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
    public class WorkCostsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<WorkCostsController> _logger;

        public WorkCostsController(ILogger<WorkCostsController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/workcost/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetWorkCostById(id);
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

                var data = _supervisor.GetAllWorkCost(workId).ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] WorkCostViewModel workCostViewModel)
        {
            try
            {
                var result = _supervisor.AddWorkCost(workCostViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] WorkCostViewModel workCostViewModel)
        {
            try
            {
                if (_supervisor.UpdateWorkCost(workCostViewModel) && workCostViewModel.id != null)
                {
                    return _supervisor.GetWorkCostById((int)workCostViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/workcost/5
        [HttpDelete("{id:int}")]
        //[Route("userdocument/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteWorkCost(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}