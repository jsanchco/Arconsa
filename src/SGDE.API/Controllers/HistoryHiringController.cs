namespace SGDE.API.Controllers
{
    #region Using

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SGDE.Domain.Supervisor;
    using SGDE.Domain.ViewModels;
    using System;

    #endregion

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryHiringController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<HistoryHiringController> _logger;

        public HistoryHiringController(ILogger<HistoryHiringController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }


        [HttpGet("getbyuserid")]
        public object GetByUserId()
        {
            try
            {
                var queryString = Request.Query;
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var userId = Convert.ToInt32(queryString["userId"]);

                var queryResult = _supervisor.GetHistoryByUserId(userId, skip, take);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }


        [HttpGet]
        public object GetByWorkId()
        {
            try
            {
                var queryString = Request.Query;
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var workId = Convert.ToInt32(queryString["workId"]);

                var queryResult = _supervisor.GetHistoryByWorkId(workId, skip, take);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody] HistoryHiringViewModel historyHiringViewModel)
        {
            try
            {
                if (_supervisor.UpdateHistoryInWork(historyHiringViewModel))
                {
                    return _supervisor.GetUserHiringById((int)historyHiringViewModel.userHiringId);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("updateinwork")]
        public object UpdateInWork([FromBody] HistoryHiringViewModel historyHiringViewModel)
        {
            try
            {
                if (_supervisor.UpdateHistoryInWork(historyHiringViewModel))
                {
                    return _supervisor.GetUserHiringById((int)historyHiringViewModel.userHiringId);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
