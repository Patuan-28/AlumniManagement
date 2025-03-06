using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ExamWeb.Models
{
    public class JobPostingModel
    {
        [Key]
        public Guid JobID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(50)]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Company is required")]
        [StringLength(50)]
        [DisplayName("Company")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Job Description is required")]
        [StringLength(500)]
        [DisplayName("Job Description")]
        public string JobDescription { get; set; }

        [Required(ErrorMessage = "Employment Type is required")]
        [DisplayName("Employment Type")]
        public byte EmploymentTypeID { get; set; }

        [Required(ErrorMessage = "Minimum Experience is required")]
        [DisplayName("Minimum Experience")]
        public byte MinimumExperience { get; set; }

        [Required(ErrorMessage = "Modified Date is required")]
        [DisplayName("Last Update")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }

        [Required(ErrorMessage = "Is Active is required")]
        [DisplayName("Status")]
        public bool IsActive { get; set; }

        [DisplayName("Remarks")]
        public bool? IsClosed { get; set; }

        [Required(ErrorMessage = "Skills Required")]
        public List<int> SelectedSkills { get; set; } = new List<int>();

        [Required(ErrorMessage = "Attachmemnts Required")]
        public List<int> SelectedAttachmentTypes { get; set; } = new List<int>();
        public List<int> SelectedJobAttachments { get; set; } = new List<int>();
        public IEnumerable<SelectListItem> EmploymentTypes { get; set; }
        public string SkillDisplay { get; set; }
        public string AttachmentTypeDisplay { get; set; }
        public string JobAttachmentDisplay { get; set; }
        public string EmploymentTypeNames { get; set; }

        [DisplayName("Candidates")]
        public int CandidateCount { get; set; }

        // Computed property for displaying IsActive status
        public string IsActiveDisplay => IsActive ? "Active" : "Inactive";
        public string IsClosedDisplay => IsActive ? "Closed" : "Not Yet";

        public bool IsClosedValue
        {
            get { return IsClosed ?? false; }
            set { IsClosed = value; }
        }

        public int Candidates { get; set; }
        public int TotalCandidates { get; set; }
    }
}
