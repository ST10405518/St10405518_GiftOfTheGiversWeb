using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using St10405518_GiftOfTheGiversWeb.Controllers;
using St10405518_GiftOfTheGiversWeb.Data;
using St10405518_GiftOfTheGiversWeb.Models;
using System.Security.Claims;

namespace St10405518_GiftOfTheGiversWeb.Tests.Controllers
{
    [TestClass]
    public class UserDisastersControllerTests
    {
        private ApplicationDbContext _context;
        private UserDisastersController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Use InMemory database with unique name for each test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name
                .Options;

            _context = new ApplicationDbContext(options);

            // Setup mock user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser@example.com"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "TestAuthentication"));

            _controller = new UserDisastersController(_context)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
        }

        [TestMethod]
        public async Task Index_UserAuthenticated_ReturnsView()
        {
            // Arrange - Add test data to in-memory database
            _context.Disaster.Add(new Disaster
            {
                DISTATER_ID = 1,
                USERNAME = "testuser@example.com",
                LOCATION = "Test Location",
                AID_TYPE = "Food",
                STARTDATE = DateTime.Now.Date,
                ENDDATE = DateTime.Now.Date.AddDays(1)
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Create_Get_ReturnsViewWithDefaultDates()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(Disaster));
        }

        [TestMethod]
        public async Task Details_ValidId_ReturnsView()
        {
            // Arrange
            var disaster = new Disaster
            {
                DISTATER_ID = 1,
                USERNAME = "testuser@example.com",
                LOCATION = "Test Location",
                AID_TYPE = "Food",
                STARTDATE = DateTime.Now.Date,
                ENDDATE = DateTime.Now.Date.AddDays(1)
            };

            _context.Disaster.Add(disaster);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(999); // Non-existent ID

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}