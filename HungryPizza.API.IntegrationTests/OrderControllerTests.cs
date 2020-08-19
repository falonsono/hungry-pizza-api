using FluentAssertions;
using HungryPizza.API.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HungryPizza.API.IntegrationTests
{
    public class OrderControllerTests : IntegrationTests
    {
        [Fact]
        public async Task Get_List_Test()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(_baseURL + "orders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            List<Order> orders = await response.Content.ReadAsAsync<List<Order>>();
            orders.Should().NotBeNull();
        }

        [Fact]
        public async Task Order_2FlavourPizza_Test()
        {
            // Arrange
            // Create client
            Client newClient = new Client()
            {
                Name = "Test Client",
                Address = "Test",
                CEP = "8888888",
                PhoneNumber = "999999999"
            };
            var postResponse = await _client.PostAsJsonAsync(_baseURL + "clients", newClient);
            var createdClient = postResponse.Content.ReadAsAsync<Client>().Result;

            // Create flavours
            Flavour flavour1 = new Flavour()
            {
                Name = "Chicken Supreme",
                Price = decimal.Parse("56,50")
            };
            postResponse = await _client.PostAsJsonAsync(_baseURL + "flavours", flavour1);
            var createdFlavour1 = postResponse.Content.ReadAsAsync<Flavour>().Result;

            Flavour flavour2 = new Flavour()
            {
                Name = "3 Queijos",
                Price = decimal.Parse("50")
            };
            postResponse = await _client.PostAsJsonAsync(_baseURL + "flavours", flavour2);
            var createdFlavour2 = postResponse.Content.ReadAsAsync<Flavour>().Result;

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
            var response = await _client.PostAsJsonAsync(_baseURL + "orders", newOrder);
            var createdOrder = response.Content.ReadAsAsync<Order>().Result;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            createdOrder.ClientId.Should().Be(createdClient.Id);
            createdOrder.Client.Name.Should().Be(createdClient.Name);

            // Pizza total should be the sum of each part
            decimal totalValue = createdFlavour1.Price / 2 + createdFlavour2.Price / 2;
            createdOrder.Total.Should().Be(totalValue);
        }

        [Fact]
        public async Task Order_3FlavourPizza_Test()
        {
            // Arrange
            // Create client
            Client newClient = new Client()
            {
                Name = "Test Client",
                Address = "Test",
                CEP = "8888888",
                PhoneNumber = "999999999"
            };
            var postResponse = await _client.PostAsJsonAsync(_baseURL + "clients", newClient);
            var createdClient = postResponse.Content.ReadAsAsync<Client>().Result;

            // Create flavours
            Flavour flavour1 = new Flavour()
            {
                Name = "Chicken Supreme",
                Price = decimal.Parse("56,50")
            };
            postResponse = await _client.PostAsJsonAsync(_baseURL + "flavours", flavour1);
            var createdFlavour1 = postResponse.Content.ReadAsAsync<Flavour>().Result;

            Flavour flavour2 = new Flavour()
            {
                Name = "3 Queijos",
                Price = decimal.Parse("50")
            };
            postResponse = await _client.PostAsJsonAsync(_baseURL + "flavours", flavour2);
            var createdFlavour2 = postResponse.Content.ReadAsAsync<Flavour>().Result;

            Flavour flavour3 = new Flavour()
            {
                Name = "Pepperoni",
                Price = decimal.Parse("55")
            };
            postResponse = await _client.PostAsJsonAsync(_baseURL + "flavours", flavour2);
            var createdFlavour3 = postResponse.Content.ReadAsAsync<Flavour>().Result;

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
                            },
                            new PizzaFlavours()
                            {
                                FlavourId = createdFlavour3.Id,
                            }
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync(_baseURL + "orders", newOrder);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Order_Over_Pizza_Limit_Test()
        {
            // Arrange
            // Create client
            Client newClient = new Client()
            {
                Name = "Test Client",
                Address = "Test",
                CEP = "8888888",
                PhoneNumber = "999999999"
            };
            var postResponse = await _client.PostAsJsonAsync(_baseURL + "clients", newClient);
            var createdClient = postResponse.Content.ReadAsAsync<Client>().Result;

            // Create flavours
            Flavour flavour1 = new Flavour()
            {
                Name = "Chicken Supreme",
                Price = decimal.Parse("56,50")
            };
            postResponse = await _client.PostAsJsonAsync(_baseURL + "flavours", flavour1);
            var createdFlavour1 = postResponse.Content.ReadAsAsync<Flavour>().Result;

            Pizza pizza = new Pizza();
            pizza.PizzaFlavours = new List<PizzaFlavours>() { new PizzaFlavours() { FlavourId = createdFlavour1.Id } };

            Order newOrder = new Order()
            {
                ClientId = createdClient.Id,
                Items = new List<Pizza>()
            };

            for (int i = 0; i <= 10; i++)
            {
                newOrder.Items.Add(pizza);
            }

            // Act
            var response = await _client.PostAsJsonAsync(_baseURL + "orders", newOrder);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
