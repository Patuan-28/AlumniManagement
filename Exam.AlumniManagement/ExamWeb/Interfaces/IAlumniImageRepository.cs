using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using ExamWeb.AlumniImageService;
using ExamWeb.Models;

namespace ExamWeb.Interfaces
{
    public interface IAlumniImageRepository
    {
        IEnumerable<AlumniImageModel> GetAllImages(int alumniID);
        AlumniImageModel GetImageByID(int imageID, int alumniID);
        Task DeleteImageByID(int imageID, int alumniID);
        Task AddImage(IEnumerable<AlumniImageModel> alumniImages);
    }
}