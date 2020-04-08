namespace SGDE.API.Controllers
{
    #region Using

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Domain.Supervisor;
    using Microsoft.Extensions.Logging;
    using System;
    using SGDE.Domain.ViewModels;

    #endregion

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceResponsesController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<InvoiceResponsesController> _logger;

        public InvoiceResponsesController(ILogger<InvoiceResponsesController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        [HttpPost]
        public object Post([FromBody]InvoiceQueryViewModel invoiceQueryViewModel)
        {
            try
            {
                var queryResult = _supervisor.GetInvoice(invoiceQueryViewModel);

                return new { Items = queryResult, Count = 1 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("getdetailinvoicebyhoursworker")]
        public object GetDetailInvoiceByHoursWorker([FromBody]InvoiceQueryViewModel invoiceQueryViewModel)
        {
            try
            {
                var queryResult = _supervisor.GetDetailInvoiceByHoursWorker(invoiceQueryViewModel);

                return new { Items = queryResult, Count = 1 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}