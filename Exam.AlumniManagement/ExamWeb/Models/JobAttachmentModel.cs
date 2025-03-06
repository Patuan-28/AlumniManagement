using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamWeb.JobPostingService;

namespace ExamWeb.Models
{
    public class JobAttachmentModel
    {
        public int AttachmentID { get; set; }

        public Guid JobID { get; set; }

        public int AlumniID { get; set; }

        public string AlumniName { get; set; }

        public byte AttachmentTypeID { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FullName { get; set; }

        public DateTime ApplyDate { get; set; }
    }
}