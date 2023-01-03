using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicePaymentsHistoryController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<InvoicePaymentsHistoryController> _logger;

        public InvoicePaymentsHistoryController(ILogger<InvoicePaymentsHistoryController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/invoicePaymentHistory/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetInvoicePaymentHistoryById(id);
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
                var invoiceId = Convert.ToInt32(queryString["invoiceId"]);

                var data = _supervisor.GetAllInvoicePaymentHistory(invoiceId);
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] InvoicePaymentHistoryViewModel invoicePaymentHistoryViewModel)
        {
            try
            {
                var result = _supervisor.AddInvoicePaymentHistory(invoicePaymentHistoryViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] InvoicePaymentHistoryViewModel invoicePaymentHistoryViewModel)
        {
            try
            {
                if (_supervisor.UpdateInvoicePaymentHistory(invoicePaymentHistoryViewModel) && invoicePaymentHistoryViewModel.id != null)
                {
                    return _supervisor.GetInvoicePaymentHistoryById((int)invoicePaymentHistoryViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/invoicePaymentHistory/5
        [HttpDelete("{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteInvoicePaymentHistory(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}
