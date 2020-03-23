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
    public class SettingsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        [AllowAnonymous]
        [HttpGet("{name}")]
        public object Get(string name)
        {
            try
            {
                return _supervisor.GetSettingByName(name);
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
                var data = _supervisor.GetAllSetting().ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]SettingViewModel settingViewModel)
        {
            try
            {
                var result = _supervisor.AddSetting(settingViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]SettingViewModel settingViewModel)
        {
            try
            {
                if (_supervisor.UpdateSetting(settingViewModel) && settingViewModel.id != null)
                {
                    return _supervisor.GetSettingById((int)settingViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/setting/5
        [HttpDelete("{id:int}")]
        //[Route("setting/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteSetting(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}