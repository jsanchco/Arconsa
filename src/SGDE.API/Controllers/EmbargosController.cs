using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmbargosController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<EmbargosController> _logger;

        public EmbargosController(ILogger<EmbargosController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/embargos/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetEmbargoById(id);
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

                var queryResult = _supervisor.GetAllEmbargo(skip, take, userId);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] EmbargoViewModel embargoViewModel)
        {
            try
            {
                var result = _supervisor.AddEmbargo(embargoViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] EmbargoViewModel embargoViewModel)
        {
            try
            {
                if (_supervisor.UpdateEmbargo(embargoViewModel) && embargoViewModel.id != null)
                {
                    return _supervisor.GetEmbargoById((int)embargoViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/embargos/5
        [HttpDelete("{id:int}")]
        //[Route("client/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteEmbargo(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}
