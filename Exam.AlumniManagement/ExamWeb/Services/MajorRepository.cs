using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ExamWeb.FacultyService;
using ExamWeb.Interfaces;
using ExamWeb.MajorService;
using ExamWeb.Models;

namespace ExamWeb.Services
{
    public class MajorRepository : IMajorRepository
    {
        private readonly MajorServiceClient _majorServiceClient;
        public MajorRepository()
        {
            _majorServiceClient = new MajorServiceClient();
        }

        public IEnumerable<MajorDTO> GetMajors()
        {
            var data = _majorServiceClient.GetMajors();
            return data;
        }

        public MajorDTO GetMajorByID(int majorID)
        {
            var data = _majorServiceClient.GetMajorByID(majorID);
            return data;
        }

        public IEnumerable<MajorDTO> GetMajorsByFacultyID(int facultyID)
        {
            var data = _majorServiceClient.GetMajorsByFacultyID(facultyID);
            return data;
        }
        public int GetFacultyIDByName(string facultyName)
        {
            var data = _majorServiceClient.GetFacultyIDByName(facultyName);
            return data;
        }

        public void InsertMajor(MajorModel major)
        {
            var result = Mapping.Mapper.Map<MajorDTO>(major);
            _majorServiceClient.InsertMajor(result);
        }

        public void UpdateMajor(MajorModel major)
        {
            var result = Mapping.Mapper.Map<MajorDTO>(major);
            _majorServiceClient.UpdateMajor(result);
        }

        public void DeleteMajor(int majorID)
        {
            _majorServiceClient.DeleteMajor(majorID);
        }

    }
}