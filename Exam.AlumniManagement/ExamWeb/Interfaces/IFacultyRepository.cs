using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamWeb.FacultyService;
using ExamWeb.Models;

namespace ExamWeb.Interfaces
{
    public interface IFacultyRepository
    {
        IEnumerable<FacultyDTO> GetFaculties();
        FacultyDTO GetFacultyByID(int facultyID);
        void InsertFaculty(FacultyModel faculty);
        void UpdateFaculty(FacultyModel faculty);
        void DeleteFaculty(int facultyID);
    }
}
