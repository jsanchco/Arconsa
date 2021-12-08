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
    public class DetailsEmbargoController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<DetailsEmbargoController> _logger;

        public DetailsEmbargoController(ILogger<DetailsEmbargoController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/deatilsembargo/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetDetailEmbargoById(id);
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
                var embargoId = Convert.ToInt32(queryString["embargoId"]);

                var queryResult = _supervisor.GetAllDetailEmbargo(skip, take, embargoId);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] DetailEmbargoViewModel detailEmbargoViewModel)
        {
            try
            {
                var result = _supervisor.AddDetailEmbargo(detailEmbargoViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] DetailEmbargoViewModel detailEmbargoViewModel)
        {
            try
            {
                if (_supervisor.UpdateDetailEmbargo(detailEmbargoViewModel) && detailEmbargoViewModel.id != null)
                {
                    return _supervisor.GetDetailEmbargoById((int)detailEmbargoViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/detailsembargo/5
        [HttpDelete("{id:int}")]
        //[Route("client/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteDetailEmbargo(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}