using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ExamWeb.PhotoAlbumService;
using ExamWeb.Models;

namespace ExamWeb.Interfaces
{
    public interface IPhotoAlbumRepository
    {
        
        IEnumerable<PhotoAlbumDTO> GetPhotoAlbums();

        
        PhotoAlbumDTO GetPhotoAlbumById(int photoAlbumId);

        
        void UpsertPhotoAlbum(PhotoAlbumModel photoAlbum);

        
        void DeletePhotoAlbum(int photoAlbumId);
    }
}
