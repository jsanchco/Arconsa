using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGDE.Domain.Supervisor;
using SGDE.Domain.ViewModels;
using System;

namespace SGDE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibrariesController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<LibrariesController> _logger;

        public LibrariesController(ILogger<LibrariesController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/libraries/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetLibraryById(id);
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
                var enterpriseId = Convert.ToInt32(queryString["enterpriseId"]);
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var filter = Util.Helper.getSearch(queryString["$filter"]);

                var queryResult = _supervisor.GetAllLibrary(enterpriseId, skip, take, filter);
                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody] LibraryViewModel libraryViewModel)
        {
            try
            {
                var result = _supervisor.AddLibrary(libraryViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] LibraryViewModel libraryViewModel)
        {
            try
            {
                if (_supervisor.UpdateLibrary(libraryViewModel) && libraryViewModel.id != null)
                {
                    return _supervisor.GetLibraryById((int)libraryViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/libraries/5
        [HttpDelete("{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteLibrary(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}
