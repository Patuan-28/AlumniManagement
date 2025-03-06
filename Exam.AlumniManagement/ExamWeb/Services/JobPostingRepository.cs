using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamWeb.JobPostingService;
using ExamWeb.Models;
using ExamWeb.Interfaces;
using ExamWeb.JobAttachmentService;

namespace ExamWeb.Services
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly JobPostingServiceClient _jpServiceClient;
        public JobPostingRepository()
        {
            _jpServiceClient = new JobPostingServiceClient();
        }

        public void DeleteJobPosting(Guid jobPostingID)
        {
            _jpServiceClient.DeleteJobPosting(jobPostingID);
        }

        public AttachmentTypeDTO GetAttachmentTypeByID(byte attachmentTypeID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AttachmentTypeDTO> GetAttachmentTypes()
        {
            var data = _jpServiceClient.GetAttachmentTypes();
            return data;
        }

        public EmploymentTypeDTO GetEmploymentTypeByID(byte employmentTypeID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmploymentTypeDTO> GetEmploymentTypes()
        {
            var data = _jpServiceClient.GetEmploymentTypes();
            return data;
        }

        public JobPostingService.JobAttachmentDTO GetJobAttachmentByID(int attachmentID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobPostingService.JobAttachmentDTO> GetJobAttachments()
        {
            throw new NotImplementedException();
        }

        public JobPostingDTO GetJobPostingByID(Guid jobPostingID)
        {
            var data = _jpServiceClient.GetJobPostingByID(jobPostingID);
            return data;
        }

        public IEnumerable<JobPostingDTO> GetJobPostings()
        {
            var data = _jpServiceClient.GetJobPostings();
            return data;
        }

        public SkillsDTO GetSkillByID(int skillID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SkillsDTO> GetSkills()
        {
            var data = _jpServiceClient.GetSkills();
            return data;
        }

        public void UpsertJobPosting(JobPostingModel model)
        {
            var result = Mapping.Mapper.Map<JobPostingDTO>(model);
            _jpServiceClient.UpsertJobPosting(result);
        }

        public void InsertJobPosting(JobPostingModel jobPosting)
        {
            var result = Mapping.Mapper.Map<JobPostingDTO>(jobPosting);
            _jpServiceClient.InsertJobPosting(result);
        }

        public void UpdateJobPosting(JobPostingModel jobPosting)
        {
            var result = Mapping.Mapper.Map<JobPostingDTO>(jobPosting);
            _jpServiceClient.UpdateJobPosting(result);
        }

        public void ApplyToJob(JobAttachmentModel applyJob)
        {
            var result = Mapping.Mapper.Map<JobPostingService.JobAttachmentDTO>(applyJob);
            _jpServiceClient.ApplyToJob(result);
        }

        public void InsertJobCandidate(JobCandidateModel jobCandidate)
        {
            var data = Mapping.Mapper.Map<JobCandidateDTO>(jobCandidate);
            _jpServiceClient.InsertJobCandidate(data);
        }

        public IEnumerable<JobCandidateModel> GetAllCandidateByJobID(Guid jobID)
        {
            var data = _jpServiceClient.GetAllCandidateByJobID(jobID);
            var result = Mapping.Mapper.Map<List<JobCandidateModel>>(data);
            return result;
        }
    }
}