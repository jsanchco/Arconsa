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
    using System.Collections.Generic;

    #endregion

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(ILogger<ReportsController> logger, ISupervisor supervisor)
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
                var startDate = queryString["startDate"];
                var endDate = queryString["endDate"];
                var workerId = Convert.ToInt32(queryString["workerId"]);
                var workId = Convert.ToInt32(queryString["workId"]);
                var clientId = Convert.ToInt32(queryString["clientId"]);

                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                    throw new Exception("Informe mal configurado");

                var reportViewModel = new ReportQueryViewModel
                {
                    startDate = startDate,
                    endDate = endDate,
                    workerId = workerId,
                    workId = workId,
                    clientId = clientId
                };

                var data = new List<ReportResultViewModel>();
                if (workerId != 0 && workId == 0 && clientId == 0)
                    data = _supervisor.GetHoursByUser(reportViewModel);

                if (workerId == 0 && workId != 0 && clientId == 0)
                    data = _supervisor.GetHoursByWork(reportViewModel);

                if (workerId == 0 && workId == 0 && clientId != 0)
                    data = _supervisor.GetHoursByClient(reportViewModel);

                if (workerId == 0 && workId == 0 && clientId == 0)
                    throw new Exception("Informe mal configurado");

                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}