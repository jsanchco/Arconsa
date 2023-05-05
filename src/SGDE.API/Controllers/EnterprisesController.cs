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
    public class EnterprisesController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<RolesController> _logger;

        public EnterprisesController(ILogger<RolesController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/enterprise/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetEnterpriseById(id);
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
                var data = _supervisor.GetAllEnterprise();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("user/{userId}")]
        public object GetByUser(int userId)
        {
            try
            {
                return _supervisor.GetEnterpriseByUser(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]EnterpriseViewModel enterpriseViewModel)
        {
            try
            {
                var result = _supervisor.AddEnterprise(enterpriseViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]EnterpriseViewModel enterpriseViewModel)
        {
            try
            {
                if (_supervisor.UpdateEnterprise(enterpriseViewModel) && enterpriseViewModel.id != null)
                {
                    return _supervisor.GetEnterpriseById((int)enterpriseViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/enterprise/5
        [HttpDelete("{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteEnterprise(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}