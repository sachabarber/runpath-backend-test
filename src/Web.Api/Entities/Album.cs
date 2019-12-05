namespace Web.Api.Entities
{
    /// <summary>
    /// Simple Album entity for the url : http://jsonplaceholder.typicode.com/albums
    /// </summary>
    public class Album
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
