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
    public class UserGoodsDonationsControllerTests
    {
        private ApplicationDbContext _context;
        private UserGoodsDonationsController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Use InMemory database with unique name
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser@example.com"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "TestAuthentication"));

            _controller = new UserGoodsDonationsController(_context)
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
            // Arrange - Add test data
            _context.GoodsDonation.Add(new GoodsDonation
            {
                GOODS_DONATION_ID = 1,
                USERNAME = "testuser@example.com",
                CATEGORY = "Food",
                DESCRIPTION = "Test Description",
                ITEM_COUNT = 10,
                DATE = DateTime.Now.Date
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Create_Get_ReturnsView()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}