﻿namespace SGDE.API.Controllers
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
    public class ProfessionInClientsController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<ProfessionInClientsController> _logger;

        public ProfessionInClientsController(ILogger<ProfessionInClientsController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }

        // GET api/professioninclients/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetProfessionInClientById(id);
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
                var professionId = Convert.ToInt32(queryString["professionId"]);
                var clientId = Convert.ToInt32(queryString["clientId"]);

                var queryResult = _supervisor.GetAllProfessionInClient(skip, take, filter, professionId, clientId);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]ProfessionInClientViewModel professionInClientViewModel)
        {
            try
            {
                var result = _supervisor.AddProfessionInClient(professionInClientViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]ProfessionInClientViewModel professionInClientViewModel)
        {
            try
            {
                if (_supervisor.UpdateProfessionInClient(professionInClientViewModel) && professionInClientViewModel.id != null)
                {
                    return _supervisor.GetProfessionInClientById((int)professionInClientViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/professioninclient/5
        [HttpDelete("{id:int}")]
        //[Route("professioninclient/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteProfessionInClient(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}