using Xunit;
using HungryPizza.API.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HungryPizza.API.Models;
using System.Collections.Generic;
using FluentAssertions;

namespace HungryPizza.API.UnitTests
{
    public class FlavourControllerTests
    {
        FlavourController _controller;

        public FlavourControllerTests()
        {
            _controller = new FlavourController();
        }

        [Fact]
        public async Task Flavour_Get_ReturnsOkResult()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Flavour_Get_ReturnsOkResult));

            // Act
            var response = await _controller.Get(context);
            context.Dispose();

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task Flavour_Get_ReturnsAllItems()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Flavour_Get_ReturnsAllItems));

            // Act
            var response = await _controller.Get(context);
            context.Dispose();

            // Assert
            var items = Assert.IsType<List<Flavour>>(((OkObjectResult)response.Result).Value);
            Assert.Equal(7, items.Count);
        }

        [Fact]
        public async Task Flavour_Post_ReturnsInsertedItem()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Flavour_Post_ReturnsInsertedItem));

            // Act
            Flavour flavour = new Flavour()
            {
                Name = "Corn Bacon",
                Price = 45
            };
            var response = await _controller.Post(context, flavour);
            context.Dispose();

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
            var item = Assert.IsType<Flavour>(((OkObjectResult)response.Result).Value);
            item.Name.Should().Be(flavour.Name);
            item.Price.Should().Be(flavour.Price);
        }

        [Fact]
        public async Task Flavour_GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Flavour_GetById_UnknownIdPassed_ReturnsNotFoundResult));

            // Act
            var notFoundResult = await _controller.GetById(context, -1);
            context.Dispose();

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public async Task Flavour_Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Flavour_Add_InvalidObjectPassed_ReturnsBadRequest));

            // Arrange
            Flavour flavour = new Flavour()
            {
                Price = 45
            };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = await _controller.Post(context, flavour);
            context.Dispose();

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }

        [Fact]
        public async Task Flavour_Remove_NotExistingItem_ReturnsNotFoundResponse()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Flavour_Remove_NotExistingItem_ReturnsNotFoundResponse));

            // Act
            var badResponse = await _controller.Delete(context, -1);
            context.Dispose();

            // Assert
            Assert.IsType<NotFoundResult>(badResponse.Result);
        }

        [Fact]
        public async Task Flavour_Remove_ExistingItem_ReturnsOkResponse()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Flavour_Remove_ExistingItem_ReturnsOkResponse));
            Flavour flavour = new Flavour()
            {
                Name = "Corn Bacon",
                Price = 45
            };
            var response = await _controller.Post(context, flavour);
            Flavour insertedFlavour = (Flavour)((OkObjectResult)response.Result).Value;
            // Act
            var deleteResponse = await _controller.Delete(context, insertedFlavour.Id);
            context.Dispose();

            // Assert
            var item = Assert.IsType<Flavour>(((OkObjectResult)response.Result).Value);
            Assert.IsType<OkObjectResult>(deleteResponse.Result);
        }
    }
}
