using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDatasController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<CompanyDatasController> _logger;

        public CompanyDatasController(ILogger<CompanyDatasController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/companydatas/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetCompanyDataById(id);
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

                var queryResult = _supervisor.GetAllCompanyData(skip, take, filter);
                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] CompanyDataViewModel companyDataViewModel)
        {
            try
            {
                var result = _supervisor.AddCompanyData(companyDataViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] CompanyDataViewModel companyDataViewModel)
        {
            try
            {
                if (_supervisor.UpdateCompanyData(companyDataViewModel) && companyDataViewModel.id != null)
                {
                    return _supervisor.GetCompanyDataById((int)companyDataViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/companydatas/5
        [HttpDelete("{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteCompanyData(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}
