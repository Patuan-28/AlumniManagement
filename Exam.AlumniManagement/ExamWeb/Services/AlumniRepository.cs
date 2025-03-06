using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamWeb.AlumniService;
using ExamWeb.Interfaces;
using ExamWeb.MajorService;
using ExamWeb.Models;

namespace ExamWeb.Services
{
    public class AlumniRepository : IAlumniRepository
    {
        private readonly AlumniServiceClient _alumniServiceClient;
        public AlumniRepository()
        {
            _alumniServiceClient = new AlumniServiceClient();
        }

        public IEnumerable<AlumniDTO> GetAlumnis()
        {
            var data = _alumniServiceClient.GetAlumnis();
            return data;
        }

        public IEnumerable<StateDTO> GetStates()
        {
            var data = _alumniServiceClient.GetStates();
            return data;
        }

        public IEnumerable<HobbyDTO> GetHobbys()
        {
            var data = _alumniServiceClient.GetHobbys();
            return data;
        }

        public IEnumerable<DistrictDTO> GetDistrictsByStateID(int stateID)
        {
            var data = _alumniServiceClient.GetDistrictsByStateID(stateID);
            return data;
        }

        public AlumniDTO GetAlumniByID(int alumniId)
        {
            var data = _alumniServiceClient.GetAlumniByID(alumniId);
            return data;
        }

        public void UpsertAlumni(AlumniModel alumni)
        {
            var result = Mapping.Mapper.Map<AlumniDTO>(alumni);
            _alumniServiceClient.UpsertAlumni(result);
        }

        public void InsertAlumni(AlumniModel alumni)
        {
            var result = Mapping.Mapper.Map<AlumniDTO>(alumni);
            _alumniServiceClient.InsertAlumni(result);
        }

        public void UpdateAlumni(AlumniModel alumni)
        {
            var result = Mapping.Mapper.Map<AlumniDTO>(alumni);
            _alumniServiceClient.UpdateAlumni(result);
        }

        public void DeleteAlumni(int alumniId)
        {
            _alumniServiceClient.DeleteAlumni(alumniId);
        }

        public void UpsertMultipleAlumni(List<AlumniModel> alumniList)
        {
            var result = Mapping.Mapper.Map<List<AlumniDTO>>(alumniList);
            _alumniServiceClient.UpsertMultipleAlumni(result.ToArray());
        }
   
    }
}