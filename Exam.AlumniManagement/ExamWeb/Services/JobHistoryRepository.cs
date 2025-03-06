using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamWeb.AlumniService;
using ExamWeb.Interfaces;
using ExamWeb.JobHistoryService;
using ExamWeb.Models;

namespace ExamWeb.Services
{
    public class JobHistoryRepository : IJobHistoryRepository
    {
        private readonly JobHistoryServiceClient _jhServiceClient;
        public JobHistoryRepository()
        {
            _jhServiceClient = new JobHistoryServiceClient();
        }

        public IEnumerable<JobHistoryDTO> GetJobHistories(int alumniID)
        {
            var data = _jhServiceClient.GetJobHistories(alumniID);
            return data;
        }

        public JobHistoryDTO GetJobHistoryByID(int id)
        {
            var data = _jhServiceClient.GetJobHistoryByID(id);
            return data;
        }

        public void InsertJobHistory(JobHistoryModel jobHistoryDTO)
        {
            var result = Mapping.Mapper.Map<JobHistoryDTO>(jobHistoryDTO);
            _jhServiceClient.InsertJobHistory(result);
        }

        public void UpdateJobHistory(JobHistoryModel jobHistoryDTO)
        {
            var result = Mapping.Mapper.Map<JobHistoryDTO>(jobHistoryDTO);
            _jhServiceClient.UpdateJobHistory(result);
        }

        public void DeleteJobHistory(int id)
        {
            _jhServiceClient.DeleteJobHistory(id);
        }
    }
}