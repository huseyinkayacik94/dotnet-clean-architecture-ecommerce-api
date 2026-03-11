using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests
{
    public class ProductsIntegrationTests
    : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductsIntegrationTests(
            CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Should_Create_Product()
        {
            var product = new
            {
                Name = "Mouse",
                Price = 50
            };

            var response = await _client.PostAsJsonAsync("/api/products", product);

            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine("STATUS: " + response.StatusCode);
            Console.WriteLine("BODY: " + body);

            response.EnsureSuccessStatusCode();
        }

    }
}
