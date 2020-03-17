namespace SGDE.API.Controllers
{
    #region Using

    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Domain.Supervisor;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Models;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Options;
    using SGDE.Domain.Helpers;

    #endregion

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly ISupervisor _supervisor;
        private readonly ILogger<UsersController> _logger;
        private readonly IOptions<JwtAppSettings> _config;

        public UsersController(ILogger<UsersController> logger, ISupervisor supervisor, IOptions<JwtAppSettings> config)
        {
            _logger = logger;
            _supervisor = supervisor;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(Login login)
        {
            try
            {

                var userAuthenticate = await _supervisor.Authenticate(login.username, login.password);

                if (userAuthenticate == null)
                {
                    _logger.LogWarning("Error in Authenticate: username [{Username}] not registered or incorrect password", login.username);
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                return new ObjectResult(new Session
                {
                    user = userAuthenticate,
                    token = getToken(userAuthenticate.id.ToString())
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAsyn()
        //{
        //    try
        //    {
        //        return new ObjectResult(await _caSupervisor.GetAllUserAsync());
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Exception: ");
        //        return StatusCode(500, ex);
        //    }
        //}

        [HttpGet]
        //[AllowAnonymous]
        public object Get()
        {
            try
            {
                var queryString = Request.Query;
                var skip = Convert.ToInt32(queryString["$skip"]);
                var take = Convert.ToInt32(queryString["$top"]);
                var filter = Util.Helper.getSearch(queryString["$filter"]);
                var roles = string.IsNullOrEmpty(queryString["roles"].ToString()) ? null : queryString["roles"].ToString().Split(',').Select(int.Parse).ToList();

                var queryResult = _supervisor.GetAllUsers(skip, take, filter, roles);

                return new { Items = queryResult.Data, Count = queryResult.Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            try
            {
                return _supervisor.GetUserById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // GET api/users/5/jesus
        [HttpGet("{id}/{name}")]
        public object Get(int id, string name)
        {
            try
            {
                return _supervisor.GetUserById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public object Post([FromBody]UserViewModel userViewModel)
        {
            try
            {
                var result = _supervisor.AddUser(userViewModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public object Put([FromBody]UserViewModel userViewModel)
        {
            try
            {
                if (_supervisor.UpdateUser(userViewModel) && userViewModel.id != null)
                {
                    return _supervisor.GetUserById((int)userViewModel.id);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id:int}")]
        //[Route("users/{id:int}")]
        public object Delete(int id)
        {
            try
            {
                return _supervisor.DeleteUser(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: ");
                return StatusCode(500, ex);
            }
        }

        private string getToken(string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Value.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                //Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token));
        }
    }
}