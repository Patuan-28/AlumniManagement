using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamWeb.Models
{
    public class AlumniModel
    {
        [Key]
        public int AlumniID { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name must be between 1 - 50 characters")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [StringLength(50, ErrorMessage = "Middle name must be between 1 - 50 characters")]
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name must be between 1 - 50 characters")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Gender")]
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email addresss is invalid")]
        [DisplayName("Email")]
        [StringLength(25, ErrorMessage = "Email must be between 1-25 characters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile number is required")]
        [StringLength(15, ErrorMessage = "Mobile number must be between 1 - 15 characters")]
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(255, ErrorMessage = "Address must be between 1 - 255 characters")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "District is required")]
        public System.Nullable<int> DistrictID { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        [DisplayName("Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> DateOfBirth { get; set; }
        [Required(ErrorMessage = "Graduation year is required")]
        [DisplayName("Graduation Year")]
        [Range(1960, 2025, ErrorMessage = "Graduation year must be between 1960 - 2025")]
        public System.Nullable<int> GraduationYear { get; set; }
        [Required(ErrorMessage = "Degree is required")]
        [StringLength(100, ErrorMessage = "Degree must be between 1 - 255 characters")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "Major is required")]
        public System.Nullable<int> MajorID { get; set; }
        [DisplayName("LinkedIn Profile")]
        public string LinkedInProfile { get; set; }

        [DisplayName("Last Update")]
        public System.DateTime ModifiedDate { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        public IEnumerable<SelectListItem> Districts { get; set; }

        public IEnumerable<SelectListItem> Faculties { get; set; }

        public IEnumerable<SelectListItem> Majors { get; set; }

        public IEnumerable<SelectListItem> Hobbies { get; set; }

        [DisplayName("State Name")]
        public string StateNames { get; set; }
        [DisplayName("District Name")]
        public string DistrictNames { get; set; }
        [DisplayName("Faculty")]
        public string FacultyNames { get; set; }
        [DisplayName("Major")]
        public string MajorNames { get; set; }
        [DisplayName("Name")]
        public string FullNames { get; set; }
        [DisplayName("Address")]
        public string FullAddresses { get; set; }
        public string FacultyMajorNames { get; set; }
        [Required(ErrorMessage = "Faculty is required")]
        public int FacultyID { get; set; }
        [Required(ErrorMessage = "State is required")]
        public int StateID { get; set; }

        public List<int> SelectedHobbies { get; set; }

        public string HobbyDisplay { get; set; }

        public string PhotoName { get; set; }

        public string PhotoPath { get; set; }

        public System.Nullable<System.DateTime> ApplyDate { get; set; }
        public List<AttachmentDTO> Attachments { get; set; }
        public string ErrorDetails { get; set; }
    }

    public class AttachmentDTO
    {
        public string FileName { get; set; }
        public string FilePath
        {
            get; set;
        }
    }

}