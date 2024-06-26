﻿namespace SGDE.API.Controllers
{
    #region Using

    using Domain.Supervisor;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;

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

        // GET api/invoice/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetInvoiceById(id);
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
                var enterpriseId = Convert.ToInt32(queryString["enterpriseId"]);
                var workId = Convert.ToInt32(queryString["workId"]);
                var clientId = Convert.ToInt32(queryString["clientId"]);

                var queryResult = _supervisor.GetAllInvoice(skip, take, enterpriseId, filter, workId, clientId);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] InvoiceViewModel invoiceViewModel)
        {
            try
            {
                var result = _supervisor.AddInvoice(invoiceViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] InvoiceViewModel invoiceViewModel)
        {
            try
            {
                if (_supervisor.UpdateInvoice(invoiceViewModel) && invoiceViewModel.id != null)
                {
                    return _supervisor.GetInvoiceById((int)invoiceViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/invoice/5
        [HttpDelete("{id:int}")]
        //[Route("invoice/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteInvoice(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("print/{invoiceId:int}")]
        public object Print(int invoiceId)
        {
            try
            {
                var queryResult = _supervisor.PrintInvoice(invoiceId);

                return new { items = queryResult, Count = 1 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("billpaymentwithamount")]
        public object BillPaymentWithAmount(CancelInvoiceWithAmount cancelInvoiceWithAmount)
        {
            try
            {
                return _supervisor.BillPayment(cancelInvoiceWithAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("importpreviousinvoice")]
        public object ImportPreviousInvoice([FromBody] InvoiceViewModel invoiceViewModel)
        {
            try
            {
                var result = _supervisor.GetPreviousInvoice(invoiceViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}