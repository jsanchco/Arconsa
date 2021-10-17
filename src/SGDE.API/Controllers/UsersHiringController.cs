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
    public class UsersHiringController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<UsersHiringController> _logger;

        public UsersHiringController(ILogger<UsersHiringController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/usershirings/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetUserHiringById(id);
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
                var filter = Util.Helper.getSearch(queryString["$filter"]);
                var userId = Convert.ToInt32(queryString["userId"]);
                var workId = Convert.ToInt32(queryString["workId"]);

                var queryResult = _supervisor.GetAllUserHiring(skip, take, filter, userId, workId);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]UserHiringViewModel userHiringViewModel)
        {
            try
            {
                var result = _supervisor.AddUserHiring(userHiringViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]UserHiringViewModel userHiringViewModel)
        {
            try
            {
                if (userHiringViewModel.professionId == null)
                    throw new Exception("Debes seleccionar una profesión");

                if (!_supervisor.IsProfessionInClient((int)userHiringViewModel.professionId, userHiringViewModel.workId, 0))
                    throw new Exception("Algunos de los Trabajadores asignados no están registrados en las profesiones del Cliente");

                if (_supervisor.UpdateUserHiring(userHiringViewModel) && userHiringViewModel.id != null)
                {
                    return _supervisor.GetUserHiringById((int)userHiringViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/usershirings/5
        [HttpDelete("{id:int}")]
        //[Route("usershirings/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteUserHiring(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("assignworkers")]
        public object AssignWorkers([FromBody]WorkersInWorkViewModel workersInWorkViewModel)
        {
            try
            {
                if (_supervisor.AssignWorkers(workersInWorkViewModel))
                    return true;
                else
                    throw new Exception("Algunos de los Trabajadores asignados no están registrados en las profesiones del Cliente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}