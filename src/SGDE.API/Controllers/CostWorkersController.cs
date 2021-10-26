namespace SGDE.API.Controllers
{
    #region Using

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Domain.Supervisor;
    using Microsoft.Extensions.Logging;
    using System;
    using Domain.ViewModels;
    using System.Linq;

    #endregion

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CostWorkersController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<CostWorkersController> _logger;

        public CostWorkersController(ILogger<CostWorkersController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/costworkers/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetCostWorkerById(id);
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
                var filter = Util.Helper.getSearch(queryString["$filter"]);
                var userId = Convert.ToInt32(queryString["userId"]);

                var queryResult = _supervisor.GetAllCostWorker(skip, take, filter, userId);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]CostWorkerViewModel costWorkerViewModel)
        {
            try
            {
                var result = _supervisor.AddCostWorker(costWorkerViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public object Put([FromBody]CostWorkerViewModel costWorkerViewModel)
        {
            try
            {
                if (_supervisor.UpdateCostWorker(costWorkerViewModel) && costWorkerViewModel.id != null)
                {
                    return _supervisor.GetCostWorkerById((int)costWorkerViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/costworker/5
        [HttpDelete("{id:int}")]
        //[Route("costworker/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteCostWorker(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("getprofessionsbyuser")]
        public object GetProfessionsByUser()
        {
            try
            {
                var queryString = Request.Query;
                var userId = Convert.ToInt32(queryString["userId"]);

                var professions = _supervisor.GetProfessionsByUserId(userId);

                return new { Items = professions, Count = professions.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}