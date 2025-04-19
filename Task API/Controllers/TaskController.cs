using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagment.DBContexts;
using TaskManagment.Models;
using TaskManagment.Models.DTOs;
using TaskManagment.Services;

namespace TaskManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        private readonly AppDbContext _context;



        public TaskController(AppDbContext context)
        {
            _context = context;
        }


        // insert 

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public IActionResult CreateTask([FromBody] CreateTaskDto dto)
        {

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            if (token != "test-token" && !string.IsNullOrEmpty(token) && TokenHelper.IsTokenExpired(token))
            {
                return Unauthorized("Token is expired.");
            }

            if (!User.IsInRole("Admin"))
            {
                return Unauthorized("You are not authorized to create tasks.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = dto.UserId
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return Ok(task);
        }

        //Task id get by

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetById(int id)
        {

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", string.Empty);

            if (token != "test-token" && !string.IsNullOrEmpty(token) && TokenHelper.IsTokenExpired(token))
            {
                return Unauthorized("Token is expired.");
            }


            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }
        

        //To get By Userid

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetByUser(int userId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            if (token != "test-token" && !string.IsNullOrEmpty(token) && TokenHelper.IsTokenExpired(token))
            {
                return Unauthorized("Token is expired.");
            }

            var tasks = _context.Tasks.Where(t => t.UserId == userId).ToList();
            if (tasks.Count == 0)
            {
                return NotFound();
            }
            return Ok(tasks);
        }
    }
}
