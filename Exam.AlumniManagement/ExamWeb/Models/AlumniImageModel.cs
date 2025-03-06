using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web;
using ExamWeb.AlumniImageService;

namespace ExamWeb.Models
{
    public class AlumniImageModel
    {
        [Key]
        public int ImageID { get; set; }
        [Required]
        public int AlumniID { get; set; }

        [Required]
        [DisplayName("Image Path")]
        public string ImagePath { get; set; }

        [Required]
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [Required]
        [DisplayName("Upload Date")]
        public System.DateTime UploadDate { get; set; }

        public EntityRef<Alumni> Alumnis { get; set; }
    }
}