using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Core.Interfaces
{
    /// <summary>
    /// Simple REST client for fetch albums from http://jsonplaceholder.typicode.com/photos
    /// </summary>
    public interface IPhotoClient
    {
        Task<IEnumerable<Photo>> GetPhotosAsync();
    }
}
