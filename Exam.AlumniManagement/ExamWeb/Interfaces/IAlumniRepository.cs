using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ExamWeb.AlumniService;
using ExamWeb.Models;

namespace ExamWeb.Interfaces
{
    public interface IAlumniRepository
    {
        IEnumerable<AlumniDTO> GetAlumnis();
        IEnumerable<DistrictDTO> GetDistrictsByStateID(int stateID);
        IEnumerable<StateDTO> GetStates();
        IEnumerable<HobbyDTO> GetHobbys();
        AlumniDTO GetAlumniByID(int alumniId);
        void UpsertAlumni(AlumniModel alumni);
        void InsertAlumni(AlumniModel alumni);
        void UpdateAlumni(AlumniModel alumni);
        void DeleteAlumni(int alumniId);
    }
}
