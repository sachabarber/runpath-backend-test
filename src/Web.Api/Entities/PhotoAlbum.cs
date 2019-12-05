using System.Collections.Generic;

namespace Web.Api.Entities
{
    /// <summary>
    /// Combined album/photo entity
    /// </summary>
    public class PhotoAlbum
    {
        public Album Album { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }
}
