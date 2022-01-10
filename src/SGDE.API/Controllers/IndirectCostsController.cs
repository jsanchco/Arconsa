// ReSharper disable InconsistentNaming
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
    public class IndirectCostsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<IndirectCostsController> _logger;

        public IndirectCostsController(ILogger<IndirectCostsController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/indirectcosts/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetIndirectCostById(id);
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

                var queryResult = _supervisor.GetAllIndirectCost(skip, take);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]IndirectCostViewModel indirectCostViewModel)
        {
            try
            {
                var result = _supervisor.AddIndirectCost(indirectCostViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]IndirectCostViewModel indirectCostViewModel)
        {
            try
            {
                if (_supervisor.UpdateIndirectCost(indirectCostViewModel) && indirectCostViewModel.id != null)
                {
                    return _supervisor.GetIndirectCostById((int)indirectCostViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/indirectcosts/5
        [HttpDelete("{id:int}")]
        //[Route("work/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteIndirectCost(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [Route("addindirectcosts")]
        public object AddIndirectCosts([FromBody] IndirectCostCopyDataViewModel indirectCostCopyDataViewModel)
        {
            try
            {
                var result = _supervisor.AddIndirectCosts(indirectCostCopyDataViewModel);
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