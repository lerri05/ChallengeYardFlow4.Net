using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ChallengeYardFlow.IntegrationTests
{
    public class BasicIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BasicIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HealthEndpoint_ShouldReturn200()
        {
            var client = _factory.CreateClient();
            var resp = await client.GetAsync("/health");
            resp.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task MlSentiment_ShouldReturnPrediction_WithValidJwt()
        {
            var client = _factory.CreateClient();
            // obter token
            var login = new { username = "tester", password = "password" };
            var loginContent = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
            var tokenResp = await client.PostAsync("/api/v1/auth/token", loginContent);
            tokenResp.IsSuccessStatusCode.Should().BeTrue();
            var tokenJson = await tokenResp.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(tokenJson);
            var token = doc.RootElement.GetProperty("access_token").GetString();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new { text = "gostei muito" };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var resp = await client.PostAsync("/api/v1/ml/sentiment", content);

            resp.IsSuccessStatusCode.Should().BeTrue();
            var json = await resp.Content.ReadAsStringAsync();
            json.Should().Contain("Prediction");
        }
    }
}


