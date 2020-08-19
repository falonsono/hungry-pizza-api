using Xunit;
using HungryPizza.API.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HungryPizza.API.Models;
using System.Collections.Generic;
using FluentAssertions;


namespace HungryPizza.API.UnitTests
{
    public class OrderControllerTests
    {
        OrderController _controller;

        public OrderControllerTests()
        {
            _controller = new OrderController();
        }

        [Fact]
        public async Task Order_Get_ReturnsOkResult()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Order_Get_ReturnsOkResult));

            // Act
            var response = await _controller.Get(context);
            context.Dispose();

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task Order_Get_ReturnsAllItems()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Order_Get_ReturnsAllItems));

            // Act
            var response = await _controller.Get(context);
            context.Dispose();

            // Assert
            var items = Assert.IsType<List<Order>>(((OkObjectResult)response.Result).Value);
        }

        [Fact]
        public async Task Order_Order_2FlavourPizza_Test()
        {
            // Arrange
            var context = DataContextMocker.GetDbContext(nameof(Order_Order_2FlavourPizza_Test));

            // Create client
            Client newClient = new Client()
            {
                Name = "Test Client",
                Address = "Test",
                CEP = "8888888",
                PhoneNumber = "999999999"
            };
            var responseClient = await new ClientController().Post(context, newClient);
            var createdClient = (Client)((OkObjectResult)responseClient.Result).Value;

            // Create flavours
            Flavour flavour1 = new Flavour()
            {
                Name = "Chicken Supreme",
                Price = decimal.Parse("56,50")
            };
            var responseFlavour1 = await new FlavourController().Post(context, flavour1);
            var createdFlavour1 = (Flavour)((OkObjectResult)responseFlavour1.Result).Value;

            Flavour flavour2 = new Flavour()
            {
                Name = "3 Queijos",
                Price = decimal.Parse("50")
            };
            var responseFlavour2 = await new FlavourController().Post(context, flavour2);
            var createdFlavour2 = (Flavour)((OkObjectResult)responseFlavour2.Result).Value;

            Order newOrder = new Order()
            {
                ClientId = createdClient.Id,
                Items = new List<Pizza>()
                {
                    new Pizza()
                    {
                        PizzaFlavours = new List<PizzaFlavours>()
                        {
                            new PizzaFlavours()
                            {
                                FlavourId = createdFlavour1.Id,
                            },
                            new PizzaFlavours()
                            {
                                FlavourId = createdFlavour2.Id,
                            }
                        }
                    }
                }
            };

            // Act
            var responseOrder = await _controller.Post(context, newOrder);
            var createdOrder = (Order)((OkObjectResult)responseOrder.Result).Value;
            context.Dispose();

            // Assert
            Assert.IsType<OkObjectResult>(responseOrder.Result);
            createdOrder.ClientId.Should().Be(createdClient.Id);
            createdOrder.Client.Name.Should().Be(createdClient.Name);

            // Pizza total should be the sum of each part
            decimal totalValue = createdFlavour1.Price / 2 + createdFlavour2.Price / 2;
            createdOrder.Total.Should().Be(totalValue);
        }

    }
}
