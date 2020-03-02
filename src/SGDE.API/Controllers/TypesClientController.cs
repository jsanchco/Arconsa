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
    public class TypesClientController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<TypesClientController> _logger;

        public TypesClientController(ILogger<TypesClientController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/typedocument/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetTypeClientById(id);
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
                var data = _supervisor.GetAllTypeClient().ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]TypeClientViewModel typeClientViewModel)
        {
            try
            {
                var result = _supervisor.AddTypeClient(typeClientViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]TypeClientViewModel typeClientViewModel)
        {
            try
            {
                if (_supervisor.UpdateTypeClient(typeClientViewModel) && typeClientViewModel.id != null)
                {
                    return _supervisor.GetTypeClientById((int)typeClientViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/typesclient/5
        [HttpDelete("{id:int}")]
        //[Route("typesclient/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteTypeClient(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

    }
}