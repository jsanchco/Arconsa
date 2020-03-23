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
    public class InvoicesController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(ILogger<InvoicesController> logger, ISupervisor supervisor)
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
    }
}