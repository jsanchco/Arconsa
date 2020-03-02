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
    public class TrainingsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<TrainingsController> _logger;

        public TrainingsController(ILogger<TrainingsController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/training/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetTrainingById(id);
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
                var userId = Convert.ToInt32(queryString["userId"]);

                var data = _supervisor.GetAllTraining(userId).ToList();
                return new { Items = data, data.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]TrainingViewModel trainingViewModel)
        {
            try
            {
                var result = _supervisor.AddTraining(trainingViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]TrainingViewModel trainingViewModel)
        {
            try
            {
                if (_supervisor.UpdateTraining(trainingViewModel) && trainingViewModel.id != null)
                {
                    return _supervisor.GetTrainingById((int)trainingViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/training/5
        [HttpDelete("{id:int}")]
        //[Route("training/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteTraining(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}