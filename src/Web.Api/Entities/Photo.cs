namespace Web.Api.Entities
{
    /// <summary>
    /// Simple Album entity for the url : http://jsonplaceholder.typicode.com/photos
    /// </summary>
    public class Photo
    {
        public int AlbumId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
