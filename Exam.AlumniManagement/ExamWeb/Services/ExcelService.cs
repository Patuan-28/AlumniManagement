using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Aspose.Cells;
using ExamWeb.AlumniService;
using ExamWeb.FacultyService;
using ExamWeb.Interfaces;
using ExamWeb.MajorService;
using ExamWeb.Models;

namespace ExamWeb.Services
{
    public class ExcelService : IExcelService
    {
        private AlumniServiceClient _alumniService;
        private MajorServiceClient _majorService;
        private FacultyServiceClient _facultyService;

        public ExcelService()
        {
            _alumniService = new AlumniServiceClient();
            _majorService = new MajorServiceClient();
            _facultyService = new FacultyServiceClient();
        }

        public Workbook AlumniExportToExcel()
        {
            var data = _alumniService.GetAlumnis()
                .Select(a => new
                {
                    AlumniID = a.AlumniID,
                    FirstName = a.FirstName,
                    MiddleName = a.MiddleName,
                    LastName = a.LastName,
                    Gender = a.Gender,
                    Email = a.Email,
                    MobileNumber = a.MobileNumber,
                    Address = a.Address,
                    FullAddresses = a.StateNames + " - " + a.DistrictNames,
                    FullFacultyMajorName = a.FacultyNames + " - " + a.MajorNames,
                    DateOfBirth = a.DateOfBirth,
                    GraduationYear = a.GraduationYear,
                    Degree = a.Degree,
                    LinkedInProfile = a.LinkedInProfile,
                });

            var states = _alumniService.GetStates().ToList();
            var districts = _alumniService.GetDistricts().ToList();
            var faculties = _facultyService.GetFaculties().ToList();
            var majors = _majorService.GetMajors().ToList();
            var fullAddress = data.Select(fa => fa.FullAddresses).ToList();
            var fullFacultyMajorName = data.Select(fa => fa.FullFacultyMajorName).ToList();

            Workbook workBook = new Workbook();
            Worksheet workSheet = workBook.Worksheets[0];
            int[] maxColumnWidths = new int[14];
            workSheet.Name = "Alumnis";

            workSheet.Cells[0, 0].PutValue("AlumniID");
            workSheet.Cells[0, 1].PutValue("First Name");
            workSheet.Cells[0, 2].PutValue("Middle Name");
            workSheet.Cells[0, 3].PutValue("Last Name");
            workSheet.Cells[0, 4].PutValue("Gender");
            workSheet.Cells[0, 5].PutValue("Email");
            workSheet.Cells[0, 6].PutValue("Mobile Number");
            workSheet.Cells[0, 7].PutValue("Address");
            workSheet.Cells[0, 8].PutValue("State - District");
            workSheet.Cells[0, 9].PutValue("Faculty - Major");
            workSheet.Cells[0, 10].PutValue("Date Of Birth");
            workSheet.Cells[0, 11].PutValue("Graduation Year");
            workSheet.Cells[0, 12].PutValue("Degree");
            workSheet.Cells[0, 13].PutValue("LinkedIn Profile");

            maxColumnWidths[1] = "First Name".Length;
            maxColumnWidths[2] = "Middle Name".Length;
            maxColumnWidths[3] = "Last Name".Length;
            maxColumnWidths[4] = "Gender".Length;
            maxColumnWidths[5] = "Email".Length;
            maxColumnWidths[6] = "Mobile Number".Length;
            maxColumnWidths[7] = "Address".Length;
            maxColumnWidths[8] = "State - District".Length;
            maxColumnWidths[9] = "Faculty - Major".Length;
            maxColumnWidths[10] = "Date Of Birth".Length;
            maxColumnWidths[11] = "Graduation Year".Length;
            maxColumnWidths[12] = "Degree".Length;
            maxColumnWidths[13] = "LinkedIn Profile".Length;

            workSheet.Cells.HideColumn(0);

            Style unlockedStyle = workSheet.Workbook.CreateStyle();
            unlockedStyle.HorizontalAlignment = TextAlignmentType.Center;
            unlockedStyle.IsLocked = false;
            workSheet.Cells.ApplyStyle(unlockedStyle, new StyleFlag() { Locked = true });

            Style style = workSheet.Workbook.CreateStyle();
            StyleFlag styleFlag = new StyleFlag();
            style.IsLocked = true;
            styleFlag.Locked = true;
            workSheet.Cells.Columns[0].ApplyStyle(style, styleFlag);

            workSheet.Protect(ProtectionType.All);

            for (int i = 0; i < data.Count(); i++)
            {
                workSheet.Cells[i + 1, 0].PutValue(data.ToList()[i].AlumniID);
                workSheet.Cells[i + 1, 1].PutValue(data.ToList()[i].FirstName);
                workSheet.Cells[i + 1, 2].PutValue(data.ToList()[i].MiddleName);
                workSheet.Cells[i + 1, 3].PutValue(data.ToList()[i].LastName);
                workSheet.Cells[i + 1, 4].PutValue(data.ToList()[i].Gender);
                workSheet.Cells[i + 1, 5].PutValue(data.ToList()[i].Email);
                workSheet.Cells[i + 1, 6].PutValue(data.ToList()[i].MobileNumber);
                workSheet.Cells[i + 1, 7].PutValue(data.ToList()[i].Address);
                workSheet.Cells[i + 1, 8].PutValue(data.ToList()[i].FullAddresses);
                workSheet.Cells[i + 1, 9].PutValue(data.ToList()[i].FullFacultyMajorName);
                workSheet.Cells[i + 1, 10].PutValue(data.ToList()[i].DateOfBirth);
                if ((data.ToList()[i]).DateOfBirth.HasValue)
                {
                    workSheet.Cells[i + 1, 10].PutValue((data.ToList()[i]).DateOfBirth.Value);
                    workSheet.Cells[i + 1, 10].SetStyle(GetDateStyle(workBook));
                }
                workSheet.Cells[i + 1, 11].PutValue(data.ToList()[i].GraduationYear);
                workSheet.Cells[i + 1, 12].PutValue(data.ToList()[i].Degree);
                workSheet.Cells[i + 1, 13].PutValue(data.ToList()[i].LinkedInProfile);

                maxColumnWidths[1] = Math.Max(maxColumnWidths[1], data.ToList()[i].FirstName.Length);
                maxColumnWidths[2] = Math.Max(maxColumnWidths[2], (data.ToList()[i].MiddleName ?? "").Length);
                maxColumnWidths[3] = Math.Max(maxColumnWidths[3], data.ToList()[i].LastName.Length);
                maxColumnWidths[4] = Math.Max(maxColumnWidths[4], (data.ToList()[i].Gender ?? "").Length);
                maxColumnWidths[5] = Math.Max(maxColumnWidths[5], data.ToList()[i].Email.Length);
                maxColumnWidths[6] = Math.Max(maxColumnWidths[6], data.ToList()[i].MobileNumber.Length);
                maxColumnWidths[7] = Math.Max(maxColumnWidths[7], data.ToList()[i].Address.Length);
                maxColumnWidths[8] = Math.Max(maxColumnWidths[8], data.ToList()[i].FullAddresses.Length);
                maxColumnWidths[9] = Math.Max(maxColumnWidths[9], data.ToList()[i].FullFacultyMajorName.Length);
                maxColumnWidths[10] = Math.Max(maxColumnWidths[10], data.ToList()[i].DateOfBirth.ToString().Length);
                maxColumnWidths[11] = Math.Max(maxColumnWidths[11], data.ToList()[i].GraduationYear.ToString().Length);
                maxColumnWidths[12] = Math.Max(maxColumnWidths[12], (data.ToList()[i].Degree ?? "").Length);
                maxColumnWidths[13] = Math.Max(maxColumnWidths[13], (data.ToList()[i].LinkedInProfile ?? "").Length);
            }
            for (int col = 1; col < maxColumnWidths.Length; col++)
            {
                workSheet.Cells.SetColumnWidth(col, maxColumnWidths[col] + 5);
            }
            Worksheet masterSheet = workBook.Worksheets.Add("Master");
            foreach (var i in states)
            {
                int row = 0;
                foreach (var j in districts)
                {
                    if (j.StateID == i.StateID)
                    {
                        masterSheet.Cells[row, 0].PutValue(i.StateName + " - " + j.DistrictName);
                        row += 1;
                    }
                    else
                    {
                        row += 1;
                    }
                }
            }
            foreach (var i in faculties)
            {
                int row = 0;
                foreach (var j in majors)
                {
                    if (i.FacultyID == j.FacultyID)
                    {
                        masterSheet.Cells[row, 1].PutValue(i.FacultyName + " - " + j.MajorName);
                        row += 1;
                    }
                    else
                    {
                        row += 1;
                    }
                }
            }
            masterSheet.Cells[0, 2].PutValue("D3");
            masterSheet.Cells[1, 2].PutValue("S1");
            masterSheet.Cells[2, 2].PutValue("S2");
            masterSheet.Cells[3, 2].PutValue("S3");

            masterSheet.Cells[0, 3].PutValue("Male");
            masterSheet.Cells[1, 3].PutValue("Female");

            masterSheet.VisibilityType = VisibilityType.VeryHidden;

            int fullAdressColumnIndex = 8;
            CellArea fullAdressArea = CellArea.CreateCellArea(1, fullAdressColumnIndex, 1000, fullAdressColumnIndex);
            Validation fullAdressValidation = workSheet.Validations[workSheet.Validations.Add(fullAdressArea)];
            fullAdressValidation.Type = ValidationType.List;
            fullAdressValidation.Formula1 = "=Master!$A$1:$A$" + districts.Count;

            int fullFacultyMajorNameColumnIndex = 9;
            CellArea fullFacultyMajorNameArea = CellArea.CreateCellArea(1, fullFacultyMajorNameColumnIndex, 1000, fullFacultyMajorNameColumnIndex);
            Validation fullFacultyMajorNameValidation = workSheet.Validations[workSheet.Validations.Add(fullFacultyMajorNameArea)];
            fullFacultyMajorNameValidation.Type = ValidationType.List;
            fullFacultyMajorNameValidation.Formula1 = "=Master!$B$1:$B$" + majors.Count;

            int firstNameColumnIndex = 1;
            CellArea firstNameArea = CellArea.CreateCellArea(1, firstNameColumnIndex, 1000, firstNameColumnIndex);
            Validation firstNameValidation = workSheet.Validations[workSheet.Validations.Add(firstNameArea)];
            firstNameValidation.Type = ValidationType.TextLength;
            firstNameValidation.Operator = OperatorType.Between;
            firstNameValidation.Formula1 = "1";
            firstNameValidation.Formula2 = "50";
            firstNameValidation.ShowError = true;
            firstNameValidation.AlertStyle = ValidationAlertType.Stop;
            firstNameValidation.ErrorTitle = "Invalid input";
            firstNameValidation.ErrorMessage = "First name must be between 1 and 50 characters";

            int middleNameColumnIndex = 2;
            CellArea middleNameArea = CellArea.CreateCellArea(1, middleNameColumnIndex, 1000, middleNameColumnIndex);
            Validation middleNameValidation = workSheet.Validations[workSheet.Validations.Add(middleNameArea)];
            middleNameValidation.Type = ValidationType.TextLength;
            middleNameValidation.Operator = OperatorType.Between;
            middleNameValidation.Formula1 = "1";
            middleNameValidation.Formula2 = "50";
            middleNameValidation.ShowError = true;
            middleNameValidation.AlertStyle = ValidationAlertType.Stop;
            middleNameValidation.ErrorTitle = "Invalid input";
            middleNameValidation.ErrorMessage = "Middle name must be between 1 and 50 characters";

            int lastNameColumnIndex = 3;
            CellArea lastNameArea = CellArea.CreateCellArea(1, lastNameColumnIndex, 1000, lastNameColumnIndex);
            Validation lastNameValidation = workSheet.Validations[workSheet.Validations.Add(lastNameArea)];
            lastNameValidation.Type = ValidationType.TextLength;
            lastNameValidation.Operator = OperatorType.Between;
            lastNameValidation.Formula1 = "1";
            lastNameValidation.Formula2 = "50";
            lastNameValidation.ShowError = true;
            lastNameValidation.AlertStyle = ValidationAlertType.Stop;
            lastNameValidation.ErrorTitle = "Invalid input";
            lastNameValidation.ErrorMessage = "Last name must be between 1 and 50 characters";

            int genderColumnIndex = 4;
            CellArea genderArea = CellArea.CreateCellArea(1, genderColumnIndex, 1000, genderColumnIndex);
            Validation genderValidation = workSheet.Validations[workSheet.Validations.Add(genderArea)];
            genderValidation.Type = ValidationType.List;
            genderValidation.Formula1 = "=Master!$D$1:$D$2";

            int emailColumnIndex = 5;
            CellArea emailArea = CellArea.CreateCellArea(1, emailColumnIndex, 1000, emailColumnIndex);
            Validation emailValidation = workSheet.Validations[workSheet.Validations.Add(emailArea)];
            emailValidation.Type = ValidationType.TextLength;
            emailValidation.Operator = OperatorType.Between;
            emailValidation.Formula1 = "1";
            emailValidation.Formula2 = "50";
            emailValidation.ShowError = true;
            emailValidation.AlertStyle = ValidationAlertType.Stop;
            emailValidation.ErrorTitle = "Invalid input";
            emailValidation.ErrorMessage = "Email must be between 1 and 50 characters";

            int mobileNumberColumnIndex = 6;
            CellArea mobileNumberArea = CellArea.CreateCellArea(1, mobileNumberColumnIndex, 1000, mobileNumberColumnIndex);
            // Validasi panjang karakter (1-15)
            Validation mobileNumberValidation = workSheet.Validations[workSheet.Validations.Add(mobileNumberArea)];
            mobileNumberValidation.Type = ValidationType.TextLength;
            mobileNumberValidation.Operator = OperatorType.Between;
            mobileNumberValidation.Formula1 = "1";
            mobileNumberValidation.Formula2 = "15";
            mobileNumberValidation.ShowError = true;
            mobileNumberValidation.AlertStyle = ValidationAlertType.Stop;
            mobileNumberValidation.ErrorTitle = "Invalid input";
            mobileNumberValidation.ErrorMessage = "Mobile number must be between 1 and 15 characters.";

            // Validasi hanya angka (whole number)
            Validation numberOnlyValidation = workSheet.Validations[workSheet.Validations.Add(mobileNumberArea)];
            numberOnlyValidation.Type = ValidationType.WholeNumber;
            numberOnlyValidation.Operator = OperatorType.GreaterOrEqual;
            numberOnlyValidation.Formula1 = "0"; // Minimal angka 0
            numberOnlyValidation.ShowError = true;
            numberOnlyValidation.AlertStyle = ValidationAlertType.Stop;
            numberOnlyValidation.ErrorTitle = "Invalid input";
            numberOnlyValidation.ErrorMessage = "Mobile number must contain only numbers.";

            int addressColumnIndex = 7;
            CellArea addressArea = CellArea.CreateCellArea(1, addressColumnIndex, 1000, addressColumnIndex);
            Validation addressValidation = workSheet.Validations[workSheet.Validations.Add(addressArea)];
            addressValidation.Type = ValidationType.TextLength;
            addressValidation.Operator = OperatorType.Between;
            addressValidation.Formula1 = "1";
            addressValidation.Formula2 = "255";
            addressValidation.ShowError = true;
            addressValidation.AlertStyle = ValidationAlertType.Stop;
            addressValidation.ErrorTitle = "Invalid input";
            addressValidation.ErrorMessage = "Address must be between 1 and 255 characters";

            //------------------------------------------------------------------------------------------------------------------
            int dateOfBirthColumnIndex = 10;
            CellArea dateOfBirthArea = CellArea.CreateCellArea(1, dateOfBirthColumnIndex, 1000, dateOfBirthColumnIndex);

            // Validasi hanya menerima input tanggal (date format)
            Validation dateOfBirthValidation = workSheet.Validations[workSheet.Validations.Add(dateOfBirthArea)];
            dateOfBirthValidation.Type = ValidationType.Date;
            dateOfBirthValidation.Operator = OperatorType.Between;

            // Atur batasan tanggal (misalnya, lahir antara tahun 1900 hingga 2024)
            dateOfBirthValidation.Formula1 = "1900-01-01";
            dateOfBirthValidation.Formula2 = "2099-12-31";

            dateOfBirthValidation.ShowError = true;
            dateOfBirthValidation.AlertStyle = ValidationAlertType.Stop;
            dateOfBirthValidation.ErrorTitle = "Invalid Date Format";
            dateOfBirthValidation.ErrorMessage = "Please enter a valid date in the format YYYY-MM-DD (e.g., 1995-06-15).";

            // Menampilkan input message saat user memilih sel
            dateOfBirthValidation.ShowInput = true;
            dateOfBirthValidation.InputTitle = "Enter Date of Birth";
            dateOfBirthValidation.InputMessage = "Enter a valid date (YYYY-MM-DD). Example: 1995-06-15";
            //------------------------------------------------------------------------------------------------------------------

            int graduationYearColumnIndex = 11;
            CellArea graduationYearArea = CellArea.CreateCellArea(1, graduationYearColumnIndex, 1000, graduationYearColumnIndex);
            Validation graduationYearValidation = workSheet.Validations[workSheet.Validations.Add(graduationYearArea)];
            graduationYearValidation.Type = ValidationType.WholeNumber;
            graduationYearValidation.Operator = OperatorType.Between;
            graduationYearValidation.Formula1 = "1960";
            graduationYearValidation.Formula2 = "2025";
            graduationYearValidation.ShowError = true;
            graduationYearValidation.AlertStyle = ValidationAlertType.Stop;
            graduationYearValidation.ErrorTitle = "Invalid input";
            graduationYearValidation.ErrorMessage = "Graduation year must be between 1960 and 2025";

            int degreeColumnIndex = 12;
            CellArea degreeArea = CellArea.CreateCellArea(1, degreeColumnIndex, 1000, degreeColumnIndex);
            Validation degreeValidation = workSheet.Validations[workSheet.Validations.Add(degreeArea)];
            degreeValidation.Type = ValidationType.List;
            degreeValidation.Formula1 = "=Master!$C$1:$C$4";

            int linkedInProfileColumnIndex = 13;
            CellArea linkedInProfileArea = CellArea.CreateCellArea(1, linkedInProfileColumnIndex, 1000, linkedInProfileColumnIndex);
            Validation linkedInProfileValidation = workSheet.Validations[workSheet.Validations.Add(linkedInProfileArea)];
            linkedInProfileValidation.Type = ValidationType.TextLength;
            linkedInProfileValidation.Operator = OperatorType.Between;
            linkedInProfileValidation.Formula1 = "1";
            linkedInProfileValidation.Formula2 = "255";
            linkedInProfileValidation.ShowError = true;
            linkedInProfileValidation.AlertStyle = ValidationAlertType.Stop;
            linkedInProfileValidation.ErrorTitle = "Invalid input";
            linkedInProfileValidation.ErrorMessage = "LinkedIn profile must be between 1 and 255 characters";

            return workBook;
        }

        public List<AlumniModel> ImportAlumniExcelToDatabase(HttpPostedFileBase file)
        {
            Workbook workbook = new Workbook(file.InputStream);
            Worksheet worksheet = workbook.Worksheets[0];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                int AlumniID = ConvertAlumniID(worksheet, i);
                string FirstName = worksheet.Cells[i, 1].StringValue;
                string MiddleName = (worksheet.Cells[i, 2].StringValue ?? "");
                string LastName = worksheet.Cells[i, 3].StringValue;
                string Gender = worksheet.Cells[i, 4].StringValue;
                string Email = worksheet.Cells[i, 5].StringValue;
                string MobileNumber = worksheet.Cells[i, 6].StringValue;
                string Address = worksheet.Cells[i, 7].StringValue;
                string FullAddresses = worksheet.Cells[i, 8].StringValue;
                string FullFacultyMajorName = worksheet.Cells[i, 9].StringValue;
                DateTime DateOfBirth = worksheet.Cells[i, 10].DateTimeValue;
                int GraduationYear = worksheet.Cells[i, 11].IntValue;
                string Degree = worksheet.Cells[i, 12].StringValue;
                string LinkedInProfile = worksheet.Cells[i, 13].StringValue;

                var addressParts = FullAddresses.Split(new[] { " - " }, StringSplitOptions.None);
                string stateName = addressParts[0];
                string districtName = addressParts[1];

                var facultyMajorParts = FullFacultyMajorName.Split(new[] { " - " }, StringSplitOptions.None);
                string facultyName = facultyMajorParts[0];
                string majorName = facultyMajorParts[1];

                var stateID = _alumniService.GetStateIDByName(stateName);
                var districtID = _alumniService.GetDistrictIDByName(districtName);
                var facultyID = _majorService.GetFacultyIDByName(facultyName);
                var majorID = _majorService.GetMajorIDByName(majorName);

                var alumni = new AlumniDTO
                {
                    AlumniID = AlumniID,
                    StateID = stateID,
                    DistrictID = districtID,
                    FacultyID = facultyID,
                    MajorID = majorID,
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    Gender = Gender,
                    Email = Email,
                    MobileNumber = MobileNumber,
                    Address = Address,
                    DateOfBirth = DateOfBirth,
                    Degree = Degree,
                    GraduationYear = GraduationYear,
                    LinkedInProfile = LinkedInProfile
                };
                //var listAlumnis = new List<AlumniDTO>();
                _alumniService.ImportFromExcel(alumni);
            }
            return listAlumnis;
        }

        private Style GetDateStyle(Workbook workbook)
        {
            Style style = workbook.CreateStyle();
            style.IsLocked = false;
            style.Custom = "mm-dd-yyyy";
            return style;
        }

        private int ConvertAlumniID(Worksheet worksheet, int i)
        {
            if (worksheet.Cells[i, 0].Value != null)
            {
                return worksheet.Cells[i, 0].IntValue;
            }
            return 0;
        }
    }
}