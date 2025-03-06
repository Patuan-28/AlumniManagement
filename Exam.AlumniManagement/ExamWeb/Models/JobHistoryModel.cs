using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamWeb.Models
{
    public class JobHistoryModel
    {
        [Key]
        public int JobHistoryID { get; set; }
        public System.Nullable<int> AlumniID { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [DisplayName("Job Title")]
        [StringLength(100)]
        public string JobTitle { get; set; }
        [Required(ErrorMessage = "Company is required")]
        [StringLength(100)]
        public string Company { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        [DisplayName("Start Date")]

        public System.Nullable<System.DateTime> StartDate { get; set; }
        [DisplayName("End Date")]

        public System.Nullable<System.DateTime> EndDate { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [DisplayName("Last Update")]

        public System.DateTime ModifiedDate { get; set; }

        public IEnumerable<SelectListItem> Alumni { get; set; }
    }
}