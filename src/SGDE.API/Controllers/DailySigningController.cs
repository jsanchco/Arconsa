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
    public class DailySigningController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<DailySigningController> _logger;

        public DailySigningController(ILogger<DailySigningController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/dailysigning/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetDailySigningById(id);
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
                var data = _supervisor.GetAllDailySigning().ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]DailySigningViewModel dailySigningViewModel)
        {
            try
            {
                var result = _supervisor.AddDailySigning(dailySigningViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]DailySigningViewModel dailySigningViewModel)
        {
            try
            {
                if (_supervisor.UpdateDailySigning(dailySigningViewModel) && dailySigningViewModel.id != null)
                {
                    return _supervisor.GetDailySigningById((int)dailySigningViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/dailysigning/5
        [HttpDelete("{id:int}")]
        //[Route("dailysigning/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteDailySigning(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}