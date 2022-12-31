namespace SGDE.API.Controllers
{
    #region Using

    using Domain.Supervisor;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
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
                var showCeros = Convert.ToBoolean(queryString["showCeros"]);

                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                    throw new Exception("Informe mal configurado");

                var reportViewModel = new ReportQueryViewModel
                {
                    startDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null),
                    endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null),
                    workerId = workerId,
                    workId = workId,
                    clientId = clientId,
                    showCeros = showCeros
                };
                reportViewModel.endDate = reportViewModel.endDate.AddDays(1).AddSeconds(-1);

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

        [HttpGet("GetReportAll")]
        public object GetReportAll()
        {
            try
            {
                var queryString = Request.Query;
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var startDate = queryString["startDate"];
                var endDate = queryString["endDate"];
                var workers = Convert.ToBoolean(queryString["workers"]);
                var works = Convert.ToBoolean(queryString["works"]);
                var clients = Convert.ToBoolean(queryString["clients"]);
                var showCeros = Convert.ToBoolean(queryString["showCeros"]);

                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                    throw new Exception("Informe mal configurado");

                var reportAllViewModel = new ReportQueryAllViewModel
                {
                    startDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null),
                    endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null),
                    workers = workers,
                    works = works,
                    clients = clients,
                    showCeros = showCeros
                };
                reportAllViewModel.endDate = reportAllViewModel.endDate.AddDays(1).AddSeconds(-1);

                var data = new List<ReportVariousInfoViewModel>();
                if (workers && !works && !clients)
                    data = _supervisor.GetHoursByAllUser(reportAllViewModel);

                if (!workers && works && !clients)
                    data = _supervisor.GetHoursByAllWork(reportAllViewModel);

                if (!workers && !works && clients)
                    data = _supervisor.GetHoursByAllClient(reportAllViewModel);

                if (!workers && !works && !clients)
                    throw new Exception("Informe mal configurado");

                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("GetReportInvoices")]
        public object GetReportInvoices()
        {
            try
            {
                var queryString = Request.Query;
                var startDate = queryString["startDate"];
                var endDate = queryString["endDate"];
                var filter = queryString["filter"];

                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                    throw new Exception("Informe mal configurado");

                var reportAllViewModel = new ReportQueryAllViewModel
                {
                    startDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null),
                    endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null),
                    filter = filter
                };

                var data = _supervisor.GetAllInvoice(reportAllViewModel);

                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("GetReportResults")]
        public object GetReportResults()
        {
            try
            {
                var queryString = Request.Query;
                var startDate = queryString["startDate"];
                var endDate = queryString["endDate"];

                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                    throw new Exception("Informe mal configurado");

                var reportAllViewModel = new ReportQueryAllViewModel
                {
                    startDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null),
                    endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null)
                };

                var data = _supervisor.GetAllResultsByWork(reportAllViewModel);

                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("GetReportTracing")]
        public object GetReportTracing()
        {
            try
            {
                var queryString = Request.Query;
                var startDate = queryString["startDate"];
                var endDate = queryString["endDate"];

                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                    throw new Exception("Informe mal configurado");

                var reportAllViewModel = new ReportQueryAllViewModel
                {
                    startDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null),
                    endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null)
                };

                var data = _supervisor.GetTracing(reportAllViewModel);

                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("GetReportCurrentStatus")]
        public object GetReportCurrentStatus()
        {
            try
            {
                var queryString = Request.Query;
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var filter = Util.Helper.getSearch(queryString["$filter"]);

                var data = _supervisor.GetAllCurrentStatus(skip, take, filter);

                return new { Items = data.Data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }


        [HttpGet("GetWorkReportBetweenDates")]
        public object GetWorkReportBetweenDates()
        {
            try
            {
                var queryString = Request.Query;
                var startDate = queryString["startDate"].ToString();
                var endDate = queryString["endDate"].ToString();
                var filter = queryString["filter"].ToString();

                var reportAllViewModel = new ReportQueryAllViewModel
                {
                    filter = filter
                };
                if (string.IsNullOrEmpty(startDate))
                    reportAllViewModel.startDate = DateTime.MinValue;
                else
                    reportAllViewModel.startDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);

                if (string.IsNullOrEmpty(endDate))
                    reportAllViewModel.endDate = DateTime.Now;
                else
                    reportAllViewModel.endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);

                var data = _supervisor.GetAllWorkBetweenDates(reportAllViewModel);

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