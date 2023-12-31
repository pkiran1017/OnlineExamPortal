﻿using Microsoft.AspNetCore.Mvc;
using OnlineExamPortal1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace OnlineExamPortal1.Controllers
{


    public class quizController : Controller
    {
        projectContext db = new projectContext();

        SqlConnection conn1 = new SqlConnection(@"Data Source=LIVING_ROOM\SQLEXPRESS;Initial Catalog=OnlineExamPort;Integrated Security=true");
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult start()
        {
            int lastexamid= 0;
           
            string query = "Insert into exam(examstartdatetime,examduration) values(GETDATE(),'" + 30 + "')";
            string query1 = "select Top 1 * from exam order by examid desc";
            SqlCommand cmd1 = new SqlCommand(query1, conn1);
            SqlCommand cmd=new SqlCommand(query,conn1);
            conn1.Open();
            cmd.ExecuteNonQuery();
            SqlDataReader reader =cmd1.ExecuteReader();
            if(reader.Read())
           {
                lastexamid = Convert.ToInt32(reader["examid"]);

           }
            conn1.Close();
            List<Topic> topicList = db.Topics.ToList();
            ViewBag.TopicList = new SelectList(topicList, "TopicId", "TopicName");
            TempData["td"] = lastexamid;
            return View();
        }
        
        public IActionResult QuizPage(string TopicList,string difficulty)
        {

            // db.Questions.Where(d => d.TopicId==Convert.ToInt32(TopicList) && d.DifficultyLevel==difficulty).ToList();
            TempData["topic"] = TopicList;
            //return View(db.Questions.ToList());
            return View(db.Questions.Where(d => d.TopicId == Convert.ToInt32(TopicList) && d.DifficultyLevel == difficulty).Take(10).ToList());
        }

        [HttpPost]
        public IActionResult QuizPage(IFormCollection frm)
        {
            int userid = 5; 
            string[] questionkey=frm.Keys.ToArray();
            int topicid = Convert.ToInt32(TempData["topic"]);
            int count = 0;
            foreach(string key in questionkey)
            {
                
                string correctop="";
                int result = 0;
                if (count >= questionkey.Length-1)
                    break;
                string questionid = key;
                string Selectedoption = frm[questionid];
                string data = Convert.ToString(TempData["td"]);
                int d=Convert.ToInt32(data);
                
                string query2 = "select * from questions where questionid=" + questionid;
                SqlCommand cmd2 = new SqlCommand(query2, conn1);
                conn1.Open();
                cmd2.ExecuteNonQuery();
                SqlDataReader reader = cmd2.ExecuteReader();
                if (reader.Read())
                {
                   correctop = Convert.ToString(reader["correctoption"]);

                }
                if(correctop==Selectedoption)
                {
                    result = 1;
                }
                reader.Close();
                string query = "Insert into result(examid,questionid,selectedoption,correctoption,result) values('" + d + "','" + questionid + "','" + Selectedoption + "','"+correctop+"','"+result+"')";
                SqlCommand cmd = new SqlCommand(query, conn1);
                cmd.ExecuteNonQuery();
                string query1 = "update  exam set examenddatetime=GETDATE(),Noofattempts='"+1+"',userid='"+userid+"',topicid='"+topicid+"' where examid="+d;
                SqlCommand cmd1 = new SqlCommand(query1, conn1);
                cmd1.ExecuteNonQuery();
                conn1.Close() ;
                count++;


            }
           

            return Content("result");

        }
    }
}
