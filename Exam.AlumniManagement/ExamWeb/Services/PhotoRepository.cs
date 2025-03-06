using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamWeb.PhotoService;
using ExamWeb.Models;
using ExamWeb.Interfaces;
using ExamWeb.PhotoAlbumService;

namespace ExamWeb.Services
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoServiceClient _photoServiceClient;
        private readonly PhotoAlbumServiceClient _photoAlbumServiceClient;

        public PhotoRepository()
        {
            _photoServiceClient = new PhotoServiceClient();
            _photoAlbumServiceClient = new PhotoAlbumServiceClient();
        }

        public IEnumerable<PhotoModel> GetAllPhotos()
        {
            var photos = _photoServiceClient.GetAllPhotos();
            var mapped = Mapping.Mapper.Map<IEnumerable<PhotoModel>>(photos);
            return mapped;
        }

        public IEnumerable<PhotoModel> GetPhotos(int AlbumID)
        {
            var album = _photoAlbumServiceClient.GetPhotoAlbumById(AlbumID);
            if(album != null)
            {
                var photos = _photoServiceClient.GetPhotos(AlbumID);
                var mapped = Mapping.Mapper.Map<IEnumerable<PhotoModel>>(photos);
                return mapped;
            }else
            {
                return Enumerable.Empty<PhotoModel>();
            }
        }

        public PhotoModel GetPhotoByID(int AlbumID, int id)
        {
            var album = _photoAlbumServiceClient.GetPhotoAlbumById(AlbumID);
            if (album != null)
            {
                var photo = _photoServiceClient.GetPhotoByID(id, AlbumID);
                var mapped = Mapping.Mapper.Map<PhotoModel>(photo);
                return mapped;
            }else
            {
                return null;
            }
        }

        public void InsertPhoto(PhotoModel photo, int AlbumID)
        {
            var result = Mapping.Mapper.Map<PhotoDTO>(photo);
            _photoServiceClient.InsertPhoto(result, AlbumID);
        }

        public void DeletePhoto(int id)
        {
            _photoServiceClient.DeletePhoto(id);
        }

        public void SetThumbnail(int id, int AlbumID)
        {
            _photoServiceClient.SetThumbnail(id, AlbumID);
        }
    }
}