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
    public class HourTypesController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<HourTypesController> _logger;

        public HourTypesController(ILogger<HourTypesController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/hourtype/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetHourTypeById(id);
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
                var data = _supervisor.GetAllHourType().ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]HourTypeViewModel hourTypeViewModel)
        {
            try
            {
                var result = _supervisor.AddHourType(hourTypeViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]HourTypeViewModel hourTypeViewModel)
        {
            try
            {
                if (_supervisor.UpdateHourType(hourTypeViewModel) && hourTypeViewModel.id != null)
                {
                    return _supervisor.GetHourTypeById((int)hourTypeViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/hourtype/5
        [HttpDelete("{id:int}")]
        //[Route("hourtype/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteHourType(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}