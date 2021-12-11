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
    public class SSHiringsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<SSHiringsController> _logger;

        public SSHiringsController(ILogger<SSHiringsController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/sshirings/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetSSHiringById(id);
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

                var queryResult = _supervisor.GetAllSSHiring(skip, take, userId);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] SSHiringViewModel sSHiringViewModel)
        {
            try
            {
                var result = _supervisor.AddSSHiring(sSHiringViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] SSHiringViewModel sSHiringViewModel)
        {
            try
            {
                if (_supervisor.UpdateSSHiring(sSHiringViewModel) && sSHiringViewModel.id != null)
                {
                    return _supervisor.GetSSHiringById((int)sSHiringViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/sshirings/5
        [HttpDelete("{id:int}")]
        //[Route("client/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteSSHiring(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}
