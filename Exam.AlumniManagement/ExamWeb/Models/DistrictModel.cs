using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamWeb.Models
{
    public class DistrictModel
    {
        public int DistrictID { get; set; }

        public string DistrictName { get; set; }

        public int StateID { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        public string StateNames { get; set; }
    }
}