using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamWeb.FacultyService;
using ExamWeb.Interfaces;
using ExamWeb.Models;

namespace ExamWeb.Services
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly FacultyServiceClient _facultyServiceClient;
        public FacultyRepository()
        {
            _facultyServiceClient = new FacultyServiceClient();
        }

        public IEnumerable<FacultyDTO> GetFaculties()
        {
            try
            {
                //if faculty service client is not active 
            }catch(Exception ex)
            {
                throw ex;
            }
            var data = _facultyServiceClient.GetFaculties();
            return data;
        }

        public FacultyDTO GetFacultyByID(int facultyID)
        {
            var data = _facultyServiceClient.GetFacultyByID(facultyID);
            return data;
        }

        public void InsertFaculty(FacultyModel faculty)
        {
            var result = Mapping.Mapper.Map<FacultyDTO>(faculty);
            _facultyServiceClient.InsertFaculty(result);
        }

        public void UpdateFaculty(FacultyModel faculty)
        {
            var result = Mapping.Mapper.Map<FacultyDTO>(faculty);
            _facultyServiceClient.UpdateFaculty(result);
        }

        public void DeleteFaculty(int facultyID)
        {
            _facultyServiceClient.DeleteFaculty(facultyID);
        }
    }
}