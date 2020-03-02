namespace SGDE.API.Controllers
{
    #region Using

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Domain.Supervisor;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    #endregion

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadBoxController : ControllerBase
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<UploadBoxController> _logger;

        public UploadBoxController(ILogger<UploadBoxController> logger, ISupervisor supervisor)
        {
            _logger = logger;
            _supervisor = supervisor;
        }


        [HttpPost]
        public async Task<IActionResult> Post(IList<IFormFile> uploadFiles)
        {            
            try
            {
                var httpPostedFile = HttpContext.Request.Form.Files["fileUpload"];
                var data = (JObject)JsonConvert.DeserializeObject(HttpContext.Request.Form["data"]);
                var userId = data["userId"].Value<int>();
                var type = data["type"].Value<string>();

                using (var memoryStream = new MemoryStream())
                {
                    await httpPostedFile.CopyToAsync(memoryStream);
                    _supervisor.UpdateUserPhoto(userId, memoryStream.ToArray());
                }

                return Ok(new { httpPostedFile.Length });
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ex.Message;

                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }
    }
}