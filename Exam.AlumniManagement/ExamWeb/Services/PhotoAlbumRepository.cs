using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamWeb.PhotoAlbumService;
using ExamWeb.Models;
using ExamWeb.Interfaces;

namespace ExamWeb.Services
{
    public class PhotoAlbumRepository : IPhotoAlbumRepository
    {
        private readonly PhotoAlbumServiceClient _photoAlbumServiceClient;

        public PhotoAlbumRepository()
        {
            _photoAlbumServiceClient = new PhotoAlbumServiceClient();
        }

        public IEnumerable<PhotoAlbumDTO> GetPhotoAlbums()
        {
            var data = _photoAlbumServiceClient.GetPhotoAlbums();
            return data;
        }

        public PhotoAlbumDTO GetPhotoAlbumById(int photoAlbumId)
        {
            var data = _photoAlbumServiceClient.GetPhotoAlbumById(photoAlbumId);
            return data;
        }

        public void UpsertPhotoAlbum(PhotoAlbumModel photoAlbum)
        {
            var result = Mapping.Mapper.Map<PhotoAlbumDTO>(photoAlbum);
            _photoAlbumServiceClient.UpsertPhotoAlbum(result);
        }

        public void DeletePhotoAlbum(int photoAlbumId)
        {
            _photoAlbumServiceClient.DeletePhotoAlbum(photoAlbumId);
        }
    }
}