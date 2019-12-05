using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private ICachePhotoClient _cachePhotoClient;

        public AlbumsController(
            ILoggerService loggerService,
            ICachePhotoClient cachePhotoClient)
        {
            _loggerService = loggerService;
            _cachePhotoClient = cachePhotoClient;
        }

        // GET api/albums
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _loggerService.Log("GET api/albums");
                var photoAlbums = await _cachePhotoClient.GetPhotoAlbumsAsync();
                return Ok(photoAlbums);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/albums/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                _loggerService.Log($"GET api/albums/{id}");
                var photoAlbums = await _cachePhotoClient.GetPhotoAlbumsByIdAsync(id);
                return Ok(photoAlbums);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
