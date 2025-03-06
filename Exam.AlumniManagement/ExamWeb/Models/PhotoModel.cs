using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExamWeb.Models
{
    public class PhotoModel
    {
        [Key]
        public int PhotoID { get; set; }

        [Required]
        [ForeignKey("PhotoAlbum")]
        [Display(Name = "Album ID")]
        public int AlbumID { get; set; }

        public string AlbumnName { get; set; }


        [Display(Name = "Photo Path")]
        public string PhotoPath { get; set; }


        [Display(Name = "Photo File Name")]
        public string PhotoFileName { get; set; }

        [Display(Name = "Is Album Thumbnail")]
        public bool IsPhotoAlbumThumbnail { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}