using BoldareApp.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BoldareApp.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var fakeData = new[]
            {
                new BreweryModel { Id = Guid.NewGuid().ToString(), Name = "Test", City = "Katowice", PhoneNumber = "123456" }
            };

            var json = JsonSerializer.Serialize(fakeData);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            return Task.FromResult(response);
        }
    }
}
