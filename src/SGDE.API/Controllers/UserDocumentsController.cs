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
    public class UserDocumentsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<UserDocumentsController> _logger;

        public UserDocumentsController(ILogger<UserDocumentsController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/userdocument/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetUserDocumentById(id);
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
                var data = _supervisor.GetAllUserDocument().ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]UserDocumentViewModel userDocumentViewModel)
        {
            try
            {
                var result = _supervisor.AddUserDocument(userDocumentViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]UserDocumentViewModel userDocumentViewModel)
        {
            try
            {
                if (_supervisor.UpdateUserDocument(userDocumentViewModel) && userDocumentViewModel.id != null)
                {
                    return _supervisor.GetUserDocumentById((int)userDocumentViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/userdocument/5
        [HttpDelete("{id:int}")]
        //[Route("userdocument/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteUserDocument(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}