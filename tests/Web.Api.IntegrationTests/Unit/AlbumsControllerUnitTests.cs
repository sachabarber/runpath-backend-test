using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Core.Interfaces;
using Web.Api.Entities;
using Xunit;

namespace Web.Api.Tests.Unit.Controllers
{
    public class AlbumsControllerUnitTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        public AlbumsControllerUnitTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Albums_WhenPhotoClientThrows_Get_Bad_Response_Test()
        {
            var fakeLogger = A.Fake<ILoggerService>();
            var fakePhotoClient = A.Fake<IPhotoClient>();
            var fakeAlbumClient = A.Fake<IAlbumClient>();
            A.CallTo(() => fakeAlbumClient.GetAlbumsAsync()).Returns(
                Task.FromResult<IEnumerable<Album>>(new List<Album>
                {
                    new Album { Id =1, Title="a", UserId = 1},
                    new Album { Id =2, Title="b", UserId = 1},
                    new Album { Id =3, Title="c", UserId = 1}
                }));

            A.CallTo(() => fakePhotoClient.GetPhotosAsync()).ThrowsAsync(new Exception("bad"));

            Action<IServiceCollection> applyTestServices = services =>
            {
                services.AddSingleton(serviceProvider => fakeLogger);
                services.AddSingleton(serviceProvider => fakeAlbumClient);
                services.AddSingleton(serviceProvider => fakePhotoClient);
            };

            var _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(applyTestServices);
            })
            .CreateClient();

            var httpResponse = await _client.GetAsync("/api/albums");
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Albums_WhenAlbumClientThrows_Get_Bad_Response_Test()
        {
            var fakeLogger = A.Fake<ILoggerService>();
            var fakeAlbumClient = A.Fake<IAlbumClient>();
            A.CallTo(() => fakeAlbumClient.GetAlbumsAsync()).ThrowsAsync(new Exception("bad"));

            Action<IServiceCollection> applyTestServices = services =>
            {
                services.AddSingleton(serviceProvider => fakeLogger);
                services.AddSingleton(serviceProvider => fakeAlbumClient);
            };

            var _client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(applyTestServices);
                })
            .CreateClient();

            var httpResponse = await _client.GetAsync("/api/albums");
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task AlbumsById_WhenPhotoClientThrows_Get_Bad_Response_Test(int id)
        {
            var fakeLogger = A.Fake<ILoggerService>();
            var fakePhotoClient = A.Fake<IPhotoClient>();
            var fakeAlbumClient = A.Fake<IAlbumClient>();
            A.CallTo(() => fakeAlbumClient.GetAlbumsAsync()).Returns(
                Task.FromResult<IEnumerable<Album>>(new List<Album>
                {
                    new Album { Id =1, Title="a", UserId = 1},
                    new Album { Id =2, Title="b", UserId = 1},
                    new Album { Id =3, Title="c", UserId = 1}
                }));

            A.CallTo(() => fakePhotoClient.GetPhotosAsync()).ThrowsAsync(new Exception("bad"));

            Action<IServiceCollection> applyTestServices = services =>
            {
                services.AddSingleton(serviceProvider => fakeLogger);
                services.AddSingleton(serviceProvider => fakeAlbumClient);
                services.AddSingleton(serviceProvider => fakePhotoClient);
            };

            var _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(applyTestServices);
            })
            .CreateClient();

            var httpResponse = await _client.GetAsync($"/api/albums/{id}");
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task AlbumsById_WhenAlbumClientThrows_Get_Bad_Response_Test(int id)
        {
            var fakeLogger = A.Fake<ILoggerService>();
            var fakeAlbumClient = A.Fake<IAlbumClient>();
            A.CallTo(() => fakeAlbumClient.GetAlbumsAsync()).ThrowsAsync(new Exception("bad"));

            Action<IServiceCollection> applyTestServices = services =>
            {
                services.AddSingleton(serviceProvider => fakeLogger);
                services.AddSingleton(serviceProvider => fakeAlbumClient);
            };

            var _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(applyTestServices);
            })
            .CreateClient();

            var httpResponse = await _client.GetAsync($"/api/albums/{id}");
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task When_Enough_Time_Has_Passed_That_Cache_Is_Cleared_And_New_Requests_Are_Made_Test()
        {
            var fakeLogger = A.Fake<ILoggerService>();
            var fakePhotoClient = A.Fake<IPhotoClient>();
            var fakeAlbumClient = A.Fake<IAlbumClient>();
            A.CallTo(() => fakeAlbumClient.GetAlbumsAsync()).Returns(
                Task.FromResult<IEnumerable<Album>>(new List<Album>
                {
                    new Album { Id =1, Title="a", UserId = 1},
                    new Album { Id =2, Title="b", UserId = 1},
                    new Album { Id =3, Title="c", UserId = 1}
                }));

            A.CallTo(() => fakePhotoClient.GetPhotosAsync()).Returns(
               Task.FromResult<IEnumerable<Photo>>(new List<Photo>
               {
                    new Photo { AlbumId =1, Title="a", Id=1, ThumbnailUrl="", Url ="" },
                    new Photo { AlbumId =2, Title="a", Id=2, ThumbnailUrl="", Url ="" },
               }));


            var fakeDateTimeProvider = A.Fake<IDateTimeProvider>();
            var now = DateTimeOffset.UtcNow;
            A.CallTo(() => fakeDateTimeProvider.UtcNow).Returns(now).NumberOfTimes(1).Then.Returns(now.AddMinutes(20));

            Action<IServiceCollection> applyTestServices = services =>
            {
               services.AddSingleton(serviceProvider => fakeLogger);
               services.AddSingleton(serviceProvider => fakeAlbumClient);
               services.AddSingleton(serviceProvider => fakePhotoClient);
               services.AddSingleton(serviceProvider => fakeDateTimeProvider);
            };

            var _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(applyTestServices);
            })
            .CreateClient();

            var httpResponse = await _client.GetAsync($"/api/albums");
            httpResponse = await _client.GetAsync($"/api/albums");

            A.CallTo(() => fakeAlbumClient.GetAlbumsAsync()).MustHaveHappenedANumberOfTimesMatching(x => x == 2);
        }
    }
}
