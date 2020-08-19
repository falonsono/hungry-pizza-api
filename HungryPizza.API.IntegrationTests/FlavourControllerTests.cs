using FluentAssertions;
using HungryPizza.API.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HungryPizza.API.IntegrationTests
{
    public class FlavourControllerTests : IntegrationTests
    {
        [Fact]
        public async Task Get_List_Test()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(_baseURL + "flavours");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            List<Flavour> flavours = await response.Content.ReadAsAsync<List<Flavour>>();
            flavours.Should().NotBeNull();
        }

        [Fact]
        public async Task Post_GetById_Test()
        {
            // Arrange
            Flavour newFlavour = new Flavour()
            {
                Name = "Chicken Supreme",
                Price = decimal.Parse("56,50")
            };
            var postResponse = await _client.PostAsJsonAsync(_baseURL + "flavours", newFlavour);
            var createdFlavour = postResponse.Content.ReadAsAsync<Flavour>().Result;

            // Act
            var response = await _client.GetAsync(_baseURL + "flavours/" + createdFlavour.Id);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Flavour flavour = await response.Content.ReadAsAsync<Flavour>();
            flavour.Id.Should().Be(createdFlavour.Id);
            flavour.Name.Should().Be(newFlavour.Name);
            flavour.Price.Should().Be(newFlavour.Price);
        }

        [Fact]
        public async Task Post_Update_Test()
        {
            // Arrange
            Flavour newFlavour = new Flavour()
            {
                Name = "Chicken Supreme",
                Price = decimal.Parse("56,50")
            };
            var postResponse = await _client.PostAsJsonAsync(_baseURL + "flavours", newFlavour);
            var createdFlavour = postResponse.Content.ReadAsAsync<Flavour>().Result;

            // Act
            Flavour updateFlavour = new Flavour()
            {
                Id = createdFlavour.Id,
                Name = "Chicken Super Supreme",
                Price = decimal.Parse("58,50")
            };
            var response = await _client.PutAsJsonAsync(_baseURL + "flavours/" + createdFlavour.Id, updateFlavour);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Flavour flavour = await response.Content.ReadAsAsync<Flavour>();
            flavour.Id.Should().Be(createdFlavour.Id);
            flavour.Name.Should().Be(updateFlavour.Name);
            flavour.Price.Should().Be(updateFlavour.Price);
        }

        [Fact]
        public async Task Post_Delete_Test()
        {
            // Arrange
            Flavour newFlavour = new Flavour()
            {
                Name = "Corn Bacon",
                Price = decimal.Parse("66.50")
            };
            var postResponse = await _client.PostAsJsonAsync(_baseURL + "flavours", newFlavour);
            var createdFlavour = postResponse.Content.ReadAsAsync<Flavour>().Result;

            // Act
            var response = await _client.GetAsync(_baseURL + "flavours/" + createdFlavour.Id);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Flavour flavour = await response.Content.ReadAsAsync<Flavour>();
            flavour.Id.Should().Be(createdFlavour.Id);
            flavour.Name.Should().Be(newFlavour.Name);
        }
    }
}
