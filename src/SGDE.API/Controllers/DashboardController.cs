using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using System;

namespace SGDE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        [HttpGet]
        [Route("{enterpriseId}")]
        public object Get(int enterpriseId)
        {
            try
            {
                var result = _supervisor.GetDashboard(enterpriseId);

                return new { data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("costsandincomes")]
        public object GetCostsAndIncomes()
        {
            try
            {
                var result = _supervisor.GetCostsAndIncomes();

                return new { chart = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("enterprise/{enterpriseId}/worksopenedandclosed")]
        public object GetWorksOpenedAndClosed(int enterpriseId)
        {
            try
            {
                var result = _supervisor.GetWorksOpenedAndClosed(enterpriseId);

                return new { chart = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("enterprise/{enterpriseId}/dates/{start}/{end}")]
        public object GetWithDates(int enterpriseId, string start, string end)
        {
            try
            {
                var dtStart = DateTime.ParseExact(start, "dd-MM-yyyy", null);
                var dtEnd = DateTime.ParseExact(end, "dd-MM-yyyy", null);
                if (dtStart > dtEnd)
                    throw new Exception("Fechas mal configuradas");

                var queryString = Request.Query;
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var filter = Util.Helper.getSearch(queryString["$filter"]);

                var queryResult = _supervisor.GetAllClient(skip, take, enterpriseId, true, filter);

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
