using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Aspose.Cells;
using ExamWeb.Models;

namespace ExamWeb.Interfaces
{
    public interface IExcelService
    {
        Workbook AlumniExportToExcel();
        List<AlumniModel> ImportAlumniExcelToDatabase(HttpPostedFileBase file);
    }
}
