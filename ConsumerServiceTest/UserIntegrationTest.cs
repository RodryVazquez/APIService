using ConsumerService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace ConsumerServiceTest
{   
    public class UserIntegrationTest
    {
        private readonly HttpClient _client;

        public UserIntegrationTest()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public void UserGetAllTest()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/Users");
            var response = _client.SendAsync(request).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(10)]
        public void UserGetById(int userId)
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/Users/{userId}");
            var response = _client.SendAsync(request).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
