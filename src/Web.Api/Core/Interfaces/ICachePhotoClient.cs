using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Core.Interfaces
{
    /// <summary>
    /// Simple caching client, which will hold a cache for hardcoded time (10 mins)
    /// after that a new fetch is done to get the data.
    /// 
    /// The 2 calls will use the cache to serve results from
    /// </summary>
    public interface ICachePhotoClient
    {
        Task<List<PhotoAlbum>> GetPhotoAlbumsAsync();

        Task<List<PhotoAlbum>> GetPhotoAlbumsByIdAsync(int id);

    }
}
