using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OnlineExamPortal.Models;


namespace OnlineExamPortal.Controllers
{
    public class DataUpload2Controller : Controller
    {
        private readonly ONLINEEXAMPORTALContext _dbContext;

        private readonly ILogger<DataUpload2Controller> _logger;
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;


        public DataUpload2Controller(ILogger<DataUpload2Controller> logger, IWebHostEnvironment _environment, IConfiguration _configuration, ONLINEEXAMPORTALContext dbContext)
        {
            _logger = logger;
            Environment = _environment;
            Configuration = _configuration;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            if (_dbContext != null)
            {
                int highestExamId = _dbContext.Exams.Max(e => e.ExamId);

                // Pass the highest ExamID to the view
                ViewBag.HighestExamId = highestExamId;

                int userCount = _dbContext.Users.Count(u => u.UserRole == "User");

                // Pass the user count to the view
                ViewBag.UserCount = userCount;

                int totalQuestionsCount = _dbContext.Questions.Count();

                // Pass the total question count to the view
                ViewBag.TotalQuestionsCount = totalQuestionsCount;
            }
            return View();
        }

        public IActionResult Logout()
        {

            return RedirectToAction("Login", "Exam");
        }

        [HttpPost]
        public IActionResult Index(IFormFile postedFile)
        {
            if (postedFile != null)
            {
                //Create a Folder.
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Save the uploaded Excel file.
                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                //Read the connection string for the Excel file.
                string conString = this.Configuration.GetConnectionString("ExcelConString");
                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                //Insert the Data read from the Excel file to Database Table.
                conString = this.Configuration.GetConnectionString("DBConnection");
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.Questions";

                        //[OPTIONAL]: Map the Excel columns with that of the database table.
                        sqlBulkCopy.ColumnMappings.Add("Type", "Type");
                        sqlBulkCopy.ColumnMappings.Add("Question", "Question");
                        sqlBulkCopy.ColumnMappings.Add("Option1", "Option1");
                        sqlBulkCopy.ColumnMappings.Add("Option2", "Option2");
                        sqlBulkCopy.ColumnMappings.Add("Option3", "Option3");
                        sqlBulkCopy.ColumnMappings.Add("Option4", "Option4");
                        sqlBulkCopy.ColumnMappings.Add("Option5", "Option5");
                        sqlBulkCopy.ColumnMappings.Add("CorrectOption", "CorrectOption");
                        sqlBulkCopy.ColumnMappings.Add("TopicID", "TopicID");
                        sqlBulkCopy.ColumnMappings.Add("DifficultyLevel", "DifficultyLevel");
                        
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }

            return RedirectToAction("index", "AddSingleQuestion");
        }
    }
}
 
