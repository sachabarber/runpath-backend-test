using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Core.Interfaces
{
    ///<inheritdoc/>
    public class CachePhotoClient : ICachePhotoClient
    {
        private IAlbumClient _albumClient;
        private IPhotoClient _photoClient;
        private IDateTimeProvider _dateTimeProvider;
        private DateTimeOffset _lastCacheTime;
        private List<PhotoAlbum> _photoAlbums = null;
        private readonly AsyncLock _mutex = new AsyncLock();

        public CachePhotoClient(
            IAlbumClient albumClient,
            IPhotoClient photoClient,
            IDateTimeProvider dateTimeProvider)
        {
            _albumClient = albumClient;
            _photoClient = photoClient;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<List<PhotoAlbum>> GetPhotoAlbumsAsync()
        {
            using (await _mutex.LockAsync())
            {
                await EnsureCacheAsync();
                return _photoAlbums;
            }
        }

        public async Task<List<PhotoAlbum>> GetPhotoAlbumsByIdAsync(int id)
        {
            using (await _mutex.LockAsync())
            {
                await EnsureCacheAsync();
                return _photoAlbums.Where(x => x.Album.UserId == id).ToList();
            }
        }

        private async Task EnsureCacheAsync()
        {
            //1st time of fetching, so just fill cache
            if (_photoAlbums == null)
            {
                await FillCacheAsync();
                return;
            }

            //should cache be reset and fetched again. We fetch every 10 mins
            if (_dateTimeProvider.UtcNow - _lastCacheTime > TimeSpan.FromMinutes(10))
            {
                _photoAlbums = null;
                await FillCacheAsync();
            }
        }


        private async Task FillCacheAsync()
        {

            var albums = await _albumClient.GetAlbumsAsync();
            var photos = await _photoClient.GetPhotosAsync();
            var photosDictionary = photos
                .GroupBy(x => x.AlbumId)
                .ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());

            List<PhotoAlbum> photoAlbums = new List<PhotoAlbum>();
            foreach (var album in albums)
            {
                if (photosDictionary.ContainsKey(album.Id))
                {
                    photoAlbums.Add(new PhotoAlbum()
                    {
                        Album = album,
                        Photos = photosDictionary[album.Id]
                    });
                }
                else
                {
                    photoAlbums.Add(new PhotoAlbum()
                    {
                        Album = album,
                        Photos = new List<Photo>()
                    });
                }
            }

            _photoAlbums = photoAlbums;
            _lastCacheTime = _dateTimeProvider.UtcNow;
        }

    }
}
