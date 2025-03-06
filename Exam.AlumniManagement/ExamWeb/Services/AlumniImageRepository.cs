using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ExamWeb.AlumniImageService;
using ExamWeb.AlumniService;
using ExamWeb.Interfaces;
using ExamWeb.Models;

namespace ExamWeb.Services
{
    public class AlumniImageRepository : IAlumniImageRepository
    {
        private AlumniImageServiceClient _alumniImageServiceClient;
        private AlumniServiceClient _alumniServiceClient;
        public AlumniImageRepository()
        {
            _alumniImageServiceClient = new AlumniImageServiceClient();
            _alumniServiceClient = new AlumniServiceClient();
        }

        public IEnumerable<AlumniImageModel> GetAllImages(int alumniID)
        {
            var alumni = _alumniServiceClient.GetAlumniByID(alumniID);
            if (alumni != null)
            {
                var alumniImages = _alumniImageServiceClient.GetAllImages(alumniID);
                var mapped = Mapping.Mapper.Map<IEnumerable<AlumniImageModel>>(alumniImages);
                return mapped;
            }
            else
            {
                return Enumerable.Empty<AlumniImageModel>();
            }
        }

        public AlumniImageModel GetImageByID(int imageID, int alumniID)
        {
            var alumni = _alumniServiceClient.GetAlumniByID(alumniID);
            if (alumni != null)
            {
                var alumniImage = _alumniImageServiceClient.GetImageByID(imageID, alumniID);
                var mapped = Mapping.Mapper.Map<AlumniImageModel>(alumniImage);
                return mapped;
            }
            else
            {
                return null;
            }
        }
        public async Task AddImage(IEnumerable<AlumniImageModel> alumniImages)
        {
            List<AlumniImageDTO> images = Mapping.Mapper.Map<List<AlumniImageDTO>>(alumniImages);
            AlumniImageDTO[] imagesArray = images.ToArray();
            await _alumniImageServiceClient.AddImageAsync(imagesArray);
        }

        public async Task DeleteImageByID(int imageID, int alumniID)
        {
            var alumni = _alumniServiceClient.GetAlumniByID(alumniID);
            if (alumni != null)
            {
                var existingImage = _alumniImageServiceClient.GetImageByID(imageID, alumniID);
                await _alumniImageServiceClient.DeleteImageByIDAsync(existingImage.ImageID, alumniID);
            }
            else
            {
                throw new Exception("Data not found");
            }
        }
    }
}