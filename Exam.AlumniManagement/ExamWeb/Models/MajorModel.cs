using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamWeb.Models
{
    public class MajorModel
    {
        [Key]
        public int MajorID { get; set; }
        [Required(ErrorMessage = "Major Name is required")]
        [DisplayName("Name")]
        [StringLength(100)]
        public string MajorName { get; set; }
        [Required(ErrorMessage = "Faculty is required")]

        public System.Nullable<int> FacultyID { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Required(ErrorMessage = "ModifiedDate is required")]
        [DisplayName("Last Update")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.Date)]
        public System.DateTime ModifiedDate { get; set; }

        public IEnumerable<SelectListItem> Faculties { get; set; }
        [DisplayName("Faculty")]
        public string FacultyNames { get; set; }
    }
}