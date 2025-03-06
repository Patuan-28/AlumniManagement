using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamWeb.Models
{
    public class FacultyModel
    {
        [Key]
        public int FacultyID { get; set; }
        [Required(ErrorMessage = "Faculty name required")]
        [DisplayName("Name")]
        [StringLength(100)]
        public string FacultyName { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Required(ErrorMessage = "ModifiedDate is required")]
        [DisplayName("Last Update")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.Date)]
        public System.DateTime ModifiedDate { get; set; }
    }
}