using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using St10405518_GiftOfTheGiversWeb.Controllers;
using St10405518_GiftOfTheGiversWeb.Data;
using St10405518_GiftOfTheGiversWeb.Models;
using System.Security.Claims;

namespace St10405518_GiftOfTheGiversWeb.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private ApplicationDbContext _context;
        private ILogger<HomeController> _logger;

        [TestInitialize]
        public void Setup()
        {
            // Use InMemory database with unique name
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _logger = new Mock<ILogger<HomeController>>().Object;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
        }

        [TestMethod]
        public void Index_UserNotAuthenticated_ReturnsView()
        {
            // Arrange
            var controller = new HomeController(_logger, _context);

            // Setup unauthenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void About_ReturnsView()
        {
            // Arrange
            var controller = new HomeController(_logger, _context);

            // Act
            var result = controller.About();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Contact_ReturnsView()
        {
            // Arrange
            var controller = new HomeController(_logger, _context);

            // Act
            var result = controller.Contact();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Error_ReturnsViewWithRequestId()
        {
            // Arrange
            var controller = new HomeController(_logger, _context);

            // Set up HttpContext with TraceIdentifier
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = controller.Error();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ErrorViewModel));

            var model = viewResult.Model as ErrorViewModel;
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.RequestId);
        }
    }
}