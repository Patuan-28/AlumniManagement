using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamWeb.Models
{
    public class EventModel
    {
        [Key]
        public int EventID { get; set; }

        [Required(ErrorMessage ="Title is Required")]
        [StringLength(100)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Description is Required")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public string EventImagePath { get; set; }

        public string EventImageName { get; set; }

        [Required (ErrorMessage ="Start Date is Required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Is Closed")]
        public bool IsClosed { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}