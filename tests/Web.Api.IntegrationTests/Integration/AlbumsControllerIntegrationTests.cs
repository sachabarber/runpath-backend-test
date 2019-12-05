using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Web.Api.Entities;
using Xunit;

namespace Web.Api.Tests.Integration.Controllers
{
    public class AlbumsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        public AlbumsControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
         
        }

        [Fact]
        public async Task CanGetCombinedAlbumsAndPhotos_Test()
        {
            var _client = _factory.CreateClient();
            var httpResponse = await _client.GetAsync("/api/albums");
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var albums = JsonConvert.DeserializeObject<IEnumerable<PhotoAlbum>>(stringResponse);
            Assert.NotNull(albums);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task CanGetCombinedAlbumsAndPhotosById_Test(int id)
        {
            var _client = _factory.CreateClient();
            var httpResponse = await _client.GetAsync($"/api/albums/{id}");
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var albums = JsonConvert.DeserializeObject<IEnumerable<PhotoAlbum>>(stringResponse);
            var userIds = albums.Select(x => x.Album).Select(x => x.UserId).ToList();
            Assert.True(userIds.All(x => x == id));
        }
    }
}
