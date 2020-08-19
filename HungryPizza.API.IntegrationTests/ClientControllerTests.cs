using FluentAssertions;
using HungryPizza.API.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HungryPizza.API.IntegrationTests
{
    public class ClientControllerTests : IntegrationTests
    {
        [Fact]
        public async Task Get_List_Test()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(_baseURL + "clients");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            List<Client> clients = await response.Content.ReadAsAsync<List<Client>>();
            clients.Should().NotBeNull();
        }

        [Fact]
        public async Task Post_GetById_Test()
        {
            // Arrange
            Client newClient = new Client()
            {
                Name = "Test Client",
                Address = "Test",
                CEP = "8888888",
                PhoneNumber = "999999999"
            };
            var postResponse = await _client.PostAsJsonAsync(_baseURL + "clients", newClient);
            var createdClient = postResponse.Content.ReadAsAsync<Client>().Result;

            // Act
            var response = await _client.GetAsync(_baseURL + "clients/" + createdClient.Id);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Client client = await response.Content.ReadAsAsync<Client>();
            client.Id.Should().Be(createdClient.Id);
            client.Name.Should().Be(newClient.Name);
            client.Address.Should().Be(newClient.Address);
            client.CEP.Should().Be(newClient.CEP);
            client.PhoneNumber.Should().Be(newClient.PhoneNumber);
        }

        [Fact]
        public async Task Post_Update_Test()
        {
            // Arrange
            Client newClient = new Client()
            {
                Name = "Test Client",
                Address = "Test",
                CEP = "8888888",
                PhoneNumber = "999999999"
            };
            var postResponse = await _client.PostAsJsonAsync(_baseURL + "clients", newClient);
            var createdClient = postResponse.Content.ReadAsAsync<Client>().Result;

            // Act
            Client updateClient = new Client()
            {
                Id = createdClient.Id,
                Name = "John Doe",
                Address = "Madre Benvenuta 123",
                CEP = "88000-100",
                PhoneNumber = "+5548993939393"
            };
            var response = await _client.PutAsJsonAsync(_baseURL + "clients/" + createdClient.Id, updateClient);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Client client = await response.Content.ReadAsAsync<Client>();
            client.Id.Should().Be(createdClient.Id);
            client.Name.Should().Be(updateClient.Name);
            client.Address.Should().Be(updateClient.Address);
            client.CEP.Should().Be(updateClient.CEP);
            client.PhoneNumber.Should().Be(updateClient.PhoneNumber);
        }

        [Fact]
        public async Task Post_Delete_Test()
        {
            // Arrange
            Client newClient = new Client()
            {
                Name = "Test Client 2",
                Address = "Test",
                CEP = "8888888",
                PhoneNumber = "999999999"
            };
            var postResponse = await _client.PostAsJsonAsync(_baseURL + "clients", newClient);
            var createdClient = postResponse.Content.ReadAsAsync<Client>().Result;

            // Act
            var response = await _client.DeleteAsync(_baseURL + "clients/" + createdClient.Id);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Client client = await response.Content.ReadAsAsync<Client>();
            client.Id.Should().Be(createdClient.Id);
            client.Name.Should().Be(newClient.Name);
        }
    }
}
