using Xunit;
using HungryPizza.API.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HungryPizza.API.Models;
using System.Collections.Generic;
using FluentAssertions;

namespace HungryPizza.API.UnitTests
{
    public class ClientControllerTests
    {
        ClientController _controller;

        public ClientControllerTests()
        {
            _controller = new ClientController();
        }

        [Fact]
        public async Task Client_Get_ReturnsOkResult()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Client_Get_ReturnsOkResult));

            // Act
            var response = await _controller.Get(context);
            context.Dispose();

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task Client_Get_ReturnsAllItems()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Client_Get_ReturnsAllItems));

            // Act
            var response = await _controller.Get(context);
            context.Dispose();

            // Assert
            var items = Assert.IsType<List<Client>>(((OkObjectResult)response.Result).Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task Client_Post_ReturnsInsertedItem()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Client_Post_ReturnsInsertedItem));

            // Act
            Client client = new Client()
            {
                Name = "Peter Doe",
                Address = "Avenida Beiramar 123",
                CEP = "88000-123",
                PhoneNumber = "+5548993939393"
            };
            var response = await _controller.Post(context, client);
            context.Dispose();

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
            var item = Assert.IsType<Client>(((OkObjectResult)response.Result).Value);
            item.Name.Should().Be(client.Name);
            item.Address.Should().Be(client.Address);
            item.CEP.Should().Be(client.CEP);
            item.PhoneNumber.Should().Be(client.PhoneNumber);
        }

        [Fact]
        public async Task Client_GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Client_GetById_UnknownIdPassed_ReturnsNotFoundResult));

            // Act
            var notFoundResult = await _controller.GetById(context, -1);
            context.Dispose();

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public async Task Client_Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Client_Add_InvalidObjectPassed_ReturnsBadRequest));

            // Arrange
            var nameMissingItem = new Client()
            {
                CEP = "88000-100",
                Address = "Rua Aristides da Silva"
            };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = await _controller.Post(context, nameMissingItem);
            context.Dispose();

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }

        [Fact]
        public async Task Client_Remove_NotExistingItem_ReturnsNotFoundResponse()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Client_Remove_NotExistingItem_ReturnsNotFoundResponse));

            // Act
            var badResponse = await _controller.Delete(context, -1);
            context.Dispose();

            // Assert
            Assert.IsType<NotFoundResult>(badResponse.Result);
        }

        [Fact]
        public async Task Client_Remove_ExistingItem_ReturnsOkResponse()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Client_Remove_ExistingItem_ReturnsOkResponse));
            Client client = new Client()
            {
                Name = "Peter Doe",
                Address = "Avenida Beiramar 123",
                CEP = "88000-123",
                PhoneNumber = "+5548993939393"
            };
            var response = await _controller.Post(context, client);
            Client insertedClient = (Client)((OkObjectResult)response.Result).Value;
            // Act
            var deleteResponse = await _controller.Delete(context, insertedClient.Id);
            context.Dispose();

            // Assert
            var item = Assert.IsType<Client>(((OkObjectResult)response.Result).Value);
            Assert.IsType<OkObjectResult>(deleteResponse.Result);
        }
    }
}
