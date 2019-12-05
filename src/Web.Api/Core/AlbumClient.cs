using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Api.Core.Interfaces;
using Web.Api.Entities;
using Web.Api.Settings;

namespace Web.Api.Core
{
    ///<inheritdoc/>
    public class AlbumClient : IAlbumClient
    {
        private string _albumEndpoint;
        private HttpClient _httpClient;

        public AlbumClient(IOptions<Endpoints> endPointOptions)
        {
            if(endPointOptions.Value == null)
            {
                throw new ArgumentNullException(nameof(endPointOptions));
            }

            if (string.IsNullOrEmpty(endPointOptions.Value.AlbumsUrl))
            {
                throw new ArgumentNullException(nameof(endPointOptions.Value.AlbumsUrl));
            }

            _albumEndpoint = endPointOptions.Value.AlbumsUrl;
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Album>> GetAlbumsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_albumEndpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Could not get albums at '{_albumEndpoint}'");
            }

            var albums = await response.Content.ReadAsAsync<IEnumerable<Album>>();
            return albums;
        }
    }
}
