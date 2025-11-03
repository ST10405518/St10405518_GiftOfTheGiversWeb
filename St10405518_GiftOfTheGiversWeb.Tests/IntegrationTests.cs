using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using St10405518_GiftOfTheGiversWeb.Controllers;
using St10405518_GiftOfTheGiversWeb.Data;
using St10405518_GiftOfTheGiversWeb.Models;
using System.Security.Claims;

namespace St10405518_GiftOfTheGiversWeb.Tests.Integration
{
    [TestClass]
    public class IntegrationTests
    {
        private ApplicationDbContext _context;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            // Create real database context with InMemory database
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "IntegrationTestDb_" + Guid.NewGuid()));

            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Seed test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            // Add test disasters
            _context.Disaster.AddRange(
                new Disaster { DISTATER_ID = 1, USERNAME = "user1@test.com", LOCATION = "Location 1", AID_TYPE = "Food", STARTDATE = DateTime.Now.Date, ENDDATE = DateTime.Now.Date.AddDays(5) },
                new Disaster { DISTATER_ID = 2, USERNAME = "user2@test.com", LOCATION = "Location 2", AID_TYPE = "Medical", STARTDATE = DateTime.Now.Date, ENDDATE = DateTime.Now.Date.AddDays(3) }
            );

            // Add test donations
            _context.GoodsDonation.AddRange(
                new GoodsDonation { GOODS_DONATION_ID = 1, USERNAME = "user1@test.com", CATEGORY = "Food", DESCRIPTION = "Canned goods", ITEM_COUNT = 50, DATE = DateTime.Now.Date },
                new GoodsDonation { GOODS_DONATION_ID = 2, USERNAME = "user2@test.com", CATEGORY = "Medical", DESCRIPTION = "First aid kits", ITEM_COUNT = 20, DATE = DateTime.Now.Date }
            );

            _context.SaveChanges();
        }

        [TestMethod]
        public async Task UserDisastersController_Integration_CreateAndRetrieveDisaster()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "user1@test.com")
            }, "TestAuth"));

            var controller = new UserDisastersController(_context)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
            };

            var newDisaster = new Disaster
            {
                LOCATION = "Integration Test Location",
                AID_TYPE = "Shelter",
                STARTDATE = DateTime.Now.Date,
                ENDDATE = DateTime.Now.Date.AddDays(7)
            };

            // Act - Create disaster
            var createResult = await controller.Create(newDisaster);

            // Assert - Check creation was successful
            Assert.IsInstanceOfType(createResult, typeof(RedirectToActionResult));

            // Act - Retrieve disasters
            var indexResult = await controller.Index();

            // Assert - Check retrieval was successful
            Assert.IsInstanceOfType(indexResult, typeof(ViewResult));
            var viewResult = indexResult as ViewResult;
            Assert.IsNotNull(viewResult.Model);
        }

        [TestMethod]
        public async Task UserGoodsDonationsController_Integration_CreateAndUpdateInventory()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "user1@test.com")
            }, "TestAuth"));

            var controller = new UserGoodsDonationsController(_context)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
            };

            var newDonation = new GoodsDonation
            {
                CATEGORY = "Clothing",
                DESCRIPTION = "Winter jackets",
                ITEM_COUNT = 30,
                DATE = DateTime.Now.Date,
                DONOR = "Test Donor"
            };

            // Act - Create donation
            var createResult = await controller.Create(newDonation);

            // Assert - Check inventory was updated
            var inventoryItem = _context.GoodsInventory.FirstOrDefault(g => g.CATEGORY == "Clothing");
            Assert.IsNotNull(inventoryItem);
            Assert.AreEqual(30, inventoryItem.ITEM_COUNT);
        }

        [TestMethod]
        public async Task Database_Integration_DataPersistence()
        {
            // Arrange - Data was seeded in Setup

            // Act - Retrieve data
            var disastersCount = await _context.Disaster.CountAsync();
            var donationsCount = await _context.GoodsDonation.CountAsync();

            // Assert - Data persists between operations
            Assert.IsTrue(disastersCount > 0, "Disasters should be persisted");
            Assert.IsTrue(donationsCount > 0, "Donations should be persisted");
        }
    }
}