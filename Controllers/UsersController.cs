using EgoUserApp.Data;
using EgoUserApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EgoUserApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<UsersController>
        // GET: api/<UsersController>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            var response = new ResponseModel();
            response.Data = users;
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 404)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var response = new ResponseModel();
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Success = false;
                response.Message = $"User with the id '{id}' was not found.";
                return NotFound(response);
            }
            response.Data = user;
            return Ok(response);
        }

        // POST api/<UsersController>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ResponseModel), 201)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [ProducesResponseType(typeof(ResponseModel), 500)]
        public async Task<IActionResult> Create([FromBody] CreateModel model)
        {
            var response = new ResponseModel();
            if(!ModelState.IsValid)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = "Name, Email and Role are required.";
                return BadRequest(response);
            }
            if(await _context.Users.AnyAsync(itm => itm.Email == model.Email))
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = $"User with the email '{model.Email}' already exists.";
                return BadRequest(response);
            }
            try
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    Email = model.Email,
                    Role = model.Role,
                    UserName = model.Email,
                    CreatedAt = DateTime.Now
                };
                await _context.Users.AddAsync(user);
               await _context.SaveChangesAsync();
                response.StatusCode = (int)HttpStatusCode.Created;
                response.Success = true;
                response.Message = "User created successfully.";
                response.Data = user;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Success = false;
            response.Message = "Error occurred while creating user.";
            return BadRequest(response);
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [ProducesResponseType(typeof(ResponseModel), 500)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateModel model)
        {
            var response = new ResponseModel();
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = "Id, Name, Email and Role are required.";
                return BadRequest(response);
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Success = false;
                response.Message = $"User with the id '{id}' was not found.";
                return NotFound(response);
            }
            user.Name = model.Name;
            user.Email = model.Email;
            user.Role = model.Role;
            user.UpdatedAt = DateTime.Now;
            _context.Update(user);
            await _context.SaveChangesAsync();
            response.Data = user;
            return Ok(response);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 404)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var response = new ResponseModel();
            if (string.IsNullOrEmpty(id))
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = "Id is required.";
                return BadRequest(response);
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Success = false;
                response.Message = $"User with the id '{id}' was not found.";
                return NotFound(response);
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Success = true;
            response.Message = "User deleted successully.";
            return Ok(response);
        }
    }
}
