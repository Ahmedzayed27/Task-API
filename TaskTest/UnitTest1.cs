using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagment.Controllers;
using TaskManagment.Models.DTOs;
using TaskManagment.Models;
using TaskManagment.Services;
using System.Linq;
using Microsoft.Extensions.Logging;
using TaskManagment.DBContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace TaskManagment.Tests
{
    [TestFixture]
    public class TaskControllerTests
    {
        private TaskController _controller;
        

        [SetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory()) // Ensure this is the test project's output dir
        .AddJsonFile("appsettings.json")
        .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString) // use a unique DB name per test run
                .Options;

            var context = new AppDbContext(options);

            // Optionally seed data here if needed
            context.Tasks.AddRange(new[]
{
    new TaskItem { Title = "Task 1", Description = "Desc", UserId = 1 },
    new TaskItem { Title = "Task 2", Description = "Desc", UserId = 1 }
});

            context.SaveChanges();

            // No need to mock DbContext or DbSet
            _controller = new TaskController(context); // use real context
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.HttpContext.Request.Headers["Authorization"] = "Bearer faketoken"; // fake token
        }


        [Test]
        public void CreateTask_ShouldReturnOk_WhenModelIsValid()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                Title = "New Task",
                Description = "Task description",
                UserId = 1
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"))
                }
            };

            _controller.HttpContext.Request.Headers["Authorization"] = "Bearer test-token";

            // Act
            var result = _controller.CreateTask(createTaskDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

   



        [Test]
        public void GetById_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            

            // Arrange
            int nonExistentTaskId = 999;

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, "1") // UserId matches seeded tasks
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            _controller.HttpContext.Request.Headers["Authorization"] = "Bearer test-token";

            // Act
            var result = _controller.GetById(nonExistentTaskId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

       


        [Test]
        public void GetByUser_ShouldReturnNotFound_WhenNoTasksExistForUser()
        {
            int nonExistentUserId = 999;

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Role, "Admin"),
        new Claim(ClaimTypes.NameIdentifier, "1") // UserId matches seeded tasks
    }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            _controller.HttpContext.Request.Headers["Authorization"] = "Bearer test-token";

            // Act
            var result = _controller.GetByUser(nonExistentUserId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }




    }
}
