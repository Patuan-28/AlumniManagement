using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamWeb.JobAttachmentService;
using ExamWeb.Models;

namespace ExamWeb.Interfaces
{
    public interface ICandidateRepository
    {
        IEnumerable<JobAttachmentDTO> GetCandidates(Guid jobId);
    }
}
