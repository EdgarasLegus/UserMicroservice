using AutoMapper;
using EasyNetQ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.Domain;
using UserApi.Domain.Entities;
using UserApi.Extensions;
using UserApi.Models;
using UserApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserApiController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>  
        /// Retrieve all users. 
        /// </summary>  
        /// <returns>Returns all users</returns>  
        /// <response code="200">Returned if operation is successful</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult Get()
        {
            var users = _userService.GetUsers();
            return Ok(_mapper.Map<IEnumerable<UserModel>>(users));
        }

        /// <summary>  
        /// Retrieve wanted user. 
        /// </summary>  
        /// <param name="userId">User to find</param>  
        /// <returns>Returns found user</returns>  
        /// <response code="200">Returned if user was found</response>  
        /// <response code="404">Returned if there is no such user</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{userId}", Name = "Get")]
        public async Task<IActionResult> Get(int userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserModel>(user));
        }

        /// <summary>  
        /// Create a new user, publish a message to RabbitMQ.  
        /// </summary>  
        /// <param name="userModel">Model to create a new user</param>  
        /// <returns>Returns created user</returns>  
        /// <response code="201">Returned if user was created</response>  
        /// <response code="400">Returned if there were problems with the creation</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserModel userModel)
        {
            var result = await _userService.CreateUser(_mapper.Map<User>(userModel));
            if (result.Status != Status.ValidationError)
            {
                var bus = RabbitHutch.CreateBus("host=localhost");
                    bus.PubSub.Publish(userModel);
                return CreatedAtAction(nameof(Get), new { id = userModel.UserId }, userModel);
            }
            return BadRequest(result.ValidationResult.ToModelState());
        }

        /// <summary>  
        /// Delete an existing user.  
        /// </summary>  
        /// <param name="userId">User to delete</param>  
        /// <response code="200">Returned if user was deleted</response>  
        /// <response code="404">Returned if user was not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var result = await _userService.DeleteUser(userId);
            if (result.Status != Status.NotFound)
            {
                return Ok();
            }
            return NotFound(result.ValidationResult.ToModelState());
        }
    }
}
