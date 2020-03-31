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
    public class DailySigningsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<DailySigningsController> _logger;

        public DailySigningsController(ILogger<DailySigningsController> logger, ISupervisor supervisor)
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
                var queryString = Request.Query;
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var userId = Convert.ToInt32(queryString["userId"]);

                var queryResult = _supervisor.GetAllDailySigning(skip, take, userId);

                return new { Items = queryResult.Data, Count = queryResult.Count };
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
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
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

        [HttpPost("massivesigning")]
        public object MassiveSigning([FromBody]MassiveSigningQueryViewModel massiveSigningQueryViewModel)
        {
            try
            {
                return _supervisor.MassiveSigning(massiveSigningQueryViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}