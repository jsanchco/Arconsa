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
    public class WorkersHiringController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<WorkersHiringController> _logger;

        public WorkersHiringController(ILogger<WorkersHiringController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
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
                var workId = Convert.ToInt32(queryString["workId"]);

                var queryResult = _supervisor.GetAllWorkerHiring(skip, take, filter, workId);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}