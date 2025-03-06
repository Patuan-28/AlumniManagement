using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamWeb.MajorService;
using ExamWeb.Models;

namespace ExamWeb.Interfaces
{
    public interface IMajorRepository
    {
        IEnumerable<MajorDTO> GetMajors();
        IEnumerable<MajorDTO> GetMajorsByFacultyID(int facultyID);
        int GetFacultyIDByName(string facultyName);
        MajorDTO GetMajorByID(int majorID);
        void InsertMajor(MajorModel major);
        void UpdateMajor(MajorModel major);
        void DeleteMajor(int majorID);
    }
}
