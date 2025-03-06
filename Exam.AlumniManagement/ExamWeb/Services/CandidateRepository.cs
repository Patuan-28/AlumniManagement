using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamWeb.JobAttachmentService;
using ExamWeb.Models;
using ExamWeb.Interfaces;

namespace ExamWeb.Services
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly JobAttachmentServiceClient _jaServiceClient;

        public CandidateRepository()
        {
            _jaServiceClient = new JobAttachmentServiceClient();
        }

        public IEnumerable<JobAttachmentDTO> GetCandidates(Guid jobId)
        {
            var data = _jaServiceClient.GetCandidates(jobId);
            return data;
        }
    }
}