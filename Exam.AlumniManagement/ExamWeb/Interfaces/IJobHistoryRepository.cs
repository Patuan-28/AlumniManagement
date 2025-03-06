using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ExamWeb.JobHistoryService;
using ExamWeb.Models;

namespace ExamWeb.Interfaces
{
    public interface IJobHistoryRepository
    {
        IEnumerable<JobHistoryDTO> GetJobHistories(int alumniID);
        JobHistoryDTO GetJobHistoryByID(int id);
        void InsertJobHistory(JobHistoryModel jobHistoryDTO);
        void UpdateJobHistory(JobHistoryModel jobHistoryDTO);
        void DeleteJobHistory(int id);
    }
}
