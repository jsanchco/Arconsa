using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using SGDE.Domain.ViewModels;
using System;
using System.Collections.Generic;

namespace SGDE.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsInvoiceController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<DetailsInvoiceController> _logger;

        public DetailsInvoiceController(ILogger<DetailsInvoiceController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/detailsinvoice/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetDetailInvoiceById(id);
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
                var previousInvoice = Convert.ToBoolean(queryString["previousInvoice"]);
                var detailByHours = Convert.ToBoolean(queryString["detailByHours"]);
                var detailByPartidas = Convert.ToBoolean(queryString["detailByPartidas"]);
                var emptyDetails = Convert.ToBoolean(queryString["emptyDetails"]);      

                List<DetailInvoiceViewModel> data = null;

                if (previousInvoice)
                    data = _supervisor.GetDetailInvoiceFromPreviousInvoice(invoiceId);

                if (detailByHours)
                    data = _supervisor.GetDetailInvoiceFromWork(invoiceId);

                if (detailByPartidas)
                    data = _supervisor.GetDetailInvoiceFromPartidas(invoiceId);

                if (emptyDetails)
                    data = _supervisor.GetEmptyDetails(invoiceId);

                if (data == null)
                    data = _supervisor.GetAllDetailInvoice(invoiceId);

                return new { Items = data, Count = data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] DetailInvoiceViewModel detailInvoiceViewModel)
        {
            try
            {
                var result = _supervisor.AddDetailInvoice(detailInvoiceViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] DetailInvoiceViewModel detailInvoiceViewModel)
        {
            try
            {
                if (_supervisor.UpdateDetailInvoice(detailInvoiceViewModel) && detailInvoiceViewModel.id != null)
                {
                    return _supervisor.GetDetailInvoiceById((int)detailInvoiceViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/detailsinvoice/5
        [HttpDelete("{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteDetailInvoice(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
