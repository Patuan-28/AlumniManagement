using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamWeb.Models
{
    public class PhotoAlbumModel
    {
        [Key]
        public int AlbumID { get; set; }

        [Required(ErrorMessage = "Album Name is Required")]
        [StringLength(50)]
        [Display(Name = "Album Name")]
        public string AlbumName { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public string ThumbnailPhotoPath { get; set; }
        public string ThumbnailPhotoName { get; set; }
    }
}