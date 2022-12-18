using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdvancesController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<AdvancesController> _logger;

        public AdvancesController(ILogger<AdvancesController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/advances/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetAdvanceById(id);
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
                var userId = Convert.ToInt32(queryString["userId"]);

                var queryResult = _supervisor.GetAllAdvance(skip, take, userId);
                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] AdvanceViewModel advanceViewModel)
        {
            try
            {
                var result = _supervisor.AddAdvance(advanceViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] AdvanceViewModel advanceViewModel)
        {
            try
            {
                if (_supervisor.UpdateAdvance(advanceViewModel) && advanceViewModel.id != null)
                {
                    return _supervisor.GetAdvanceById((int)advanceViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/advances/5
        [HttpDelete("{id:int}")]
        //[Route("userdocument/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteAdvance(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}
