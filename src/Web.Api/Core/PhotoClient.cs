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
    public class PhotoClient : IPhotoClient
    {
        private string _photosUrl;
        private HttpClient _httpClient;

        public PhotoClient(IOptions<Endpoints> endPointOptions)
        {
            if(endPointOptions.Value == null)
            {
                throw new ArgumentNullException(nameof(endPointOptions));
            }

            if (string.IsNullOrEmpty(endPointOptions.Value.PhotosUrl))
            {
                throw new ArgumentNullException(nameof(endPointOptions.Value.PhotosUrl));
            }

            _photosUrl = endPointOptions.Value.PhotosUrl;
            _httpClient = new HttpClient();
        }


        public async Task<IEnumerable<Photo>> GetPhotosAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_photosUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Could not get photos at '{_photosUrl}'");
            }

            var photos = await response.Content.ReadAsAsync<IEnumerable<Photo>>();
            return photos;

        }
    }
}
