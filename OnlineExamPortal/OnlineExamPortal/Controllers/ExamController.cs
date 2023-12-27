using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using OnlineExamPortal.Models;
using RestSharp;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


namespace OnlineExamPortal.Controllers
{
    public class ExamController : Controller
    {   
        SqlConnection conn1=new SqlConnection(@"Data Source=LIVING_ROOM\SQLEXPRESS;Initial Catalog=OnlineExamPortal;Integrated Security=True");

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(Registration r)
        {
            string query = "INSERT INTO Users (UserName, Password, Email, Mobile, UserRole) VALUES ('" + r.UserName + "', '" + r.Password + "', '" + r.Email + "', '" + r.Mobile + "', '" + r.UserRole + "')";
            conn1.Open();
            SqlCommand command =new SqlCommand(query,conn1);

            
                command.Parameters.AddWithValue("@UserName", r.UserName);
                command.Parameters.AddWithValue("@Password", r.Password);
                command.Parameters.AddWithValue("@Email", r.Email);
                command.Parameters.AddWithValue("@Mobile", r.Mobile);
                command.Parameters.AddWithValue("@UserRole", r.UserRole);

                command.ExecuteNonQuery();
            conn1.Close();

            return RedirectToAction("Login", "Exam");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
            conn1.Open();
            SqlCommand command = new SqlCommand(query, conn1);

            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Password", Password);

            int count = (int)command.ExecuteScalar();

            conn1.Close();

            if (count == 1)
            {
                string roleQuery = "SELECT UserRole FROM Users WHERE Email = @Email AND Password = @Password";
                conn1.Open();
                SqlCommand roleCommand = new SqlCommand(roleQuery, conn1);
                roleCommand.Parameters.AddWithValue("@Email", Email);
                roleCommand.Parameters.AddWithValue("@Password", Password);

                string userRole = roleCommand.ExecuteScalar()?.ToString();

                conn1.Close();

                if (userRole == "Admin")
                {
                   
                    string roleQuery1 = "SELECT userid FROM Users WHERE Email = '"+Email+"' AND Password = '"+Password+"'";
                    SqlCommand roleCommand1 = new SqlCommand(roleQuery1, conn1);
                    conn1.Open();
                    object result = roleCommand1.ExecuteScalar();
                    int userid;

                    if (result != null && int.TryParse(result.ToString(), out userid))
                    {
                        // User is an admin
                        HttpContext.Session.SetInt32("UserId", userid);
                        return RedirectToAction("Index","AddSingleQuestion");
                    }
                }
                else
                {
                    // User is not an admin but has valid credentials
                    string roleQuery1 = "SELECT userid FROM Users WHERE Email = '" + Email + "' AND Password = '" + Password + "'";
                    SqlCommand roleCommand1 = new SqlCommand(roleQuery1, conn1);
                    conn1.Open();
                    object result = roleCommand1.ExecuteScalar();
                    int userid_user;

                    if (result != null && int.TryParse(result.ToString(), out userid_user))
                    {
                        
                        HttpContext.Session.SetInt32("UserId", userid_user);

                        return RedirectToAction("start","quiz");
                    }
                }
            }
            ViewData["ErrorMessage"] = "User details not valid";
            return View();
        }

       
        public IActionResult AdminPage()
        {

            /*
             Always add this 2 lines in your controller so you can fetch current logged in user's userid.
            This is to simplify implementation of login so all the data must get added to the current logged in user 

            in view page
            @ViewBag.UserId!
            above line will fetch the userid for the current logged in user
             */

            var userid = HttpContext.Session.GetInt32("UserId");
           
            ViewBag.UserId = userid;
            return View();
        }
        public IActionResult UserPage()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
           
            ViewBag.UserId = userid;
            return View();
        }
        public IActionResult ForgetPass()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPass(string Email)
        {
            Registration r = new Registration();

            conn1.Open();

            string query = "SELECT Password FROM Users WHERE Email = @Email";

            using (SqlCommand cmd = new SqlCommand(query, conn1))
            {
                cmd.Parameters.AddWithValue("@Email", Email);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        r.Password = reader["Password"].ToString();
                    }
                }
            }
            return View("ForgetPassView", r);
        }
        
    }
}
