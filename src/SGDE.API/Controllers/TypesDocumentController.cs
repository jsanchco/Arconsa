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
    public class TypesDocumentController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<TypesDocumentController> _logger;

        public TypesDocumentController(ILogger<TypesDocumentController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/typesdocument/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetTypeDocumentById(id);
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
                var data = _supervisor.GetAllTypeDocument().ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]TypeDocumentViewModel typeDocumentViewModel)
        {
            try
            {
                var result = _supervisor.AddTypeDocument(typeDocumentViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]TypeDocumentViewModel typeDocumentViewModel)
        {
            try
            {
                if (_supervisor.UpdateTypeDocument(typeDocumentViewModel) && typeDocumentViewModel.id != null)
                {
                    return _supervisor.GetTypeDocumentById((int)typeDocumentViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/typesdocument/5
        [HttpDelete("{id:int}")]
        //[Route("typesdocument/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteTypeDocument(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}