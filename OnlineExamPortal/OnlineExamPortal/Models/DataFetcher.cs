using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Data;

namespace OnlineExamPortal.Models
{
    public class DataFetcher
    {
        SqlConnection connection = new SqlConnection(@"Data Source = LIVING_ROOM\SQLEXPRESS; Initial Catalog = OnlineExamPortal; Integrated Security = true; TrustServerCertificate = true");
        public List<Result>? SinglePieList(int UId, int TId)
        {
            string Query = "SELECT * FROM RESULT r JOIN EXAM e ON r.ExamId = e.ExamId" +
                $" WHERE e.UserId = {UId} AND e.TopicId = {TId}" +
                $" AND E.ExamId = (SELECT MAX(ExamId) From Exam WHERE UserId = {UId})";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Result> results = new List<Result>();
            foreach (DataRow dr in dt.Rows)
            {
                Result model = new Result();
                model.ResultId = Convert.ToInt32(dr[0]);
                model.ExamId = Convert.ToInt32(dr[1]);
                model.QuestionId = Convert.ToInt32(dr[2]);
                model.SelectedOption = Convert.ToString(dr[3]);
                model.CorrectOption = Convert.ToString(dr[4]);
                model.Result1 = Convert.ToInt32(dr[5]);
                results.Add(model);
            }
            if (results.Count() == 0)
            {
                return null;
            }
            else
            {
                return results;
            }
        }
        public List<Result>? UidTidList(int UId, int TId)
        {
            string Query = $"SELECT * FROM RESULT r JOIN EXAM e ON r.ExamId = e.ExamId WHERE e.UserId = {UId} AND e.TopicId = {TId}";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Result> results = new List<Result>();
            foreach (DataRow dr in dt.Rows)
            {
                Result model = new Result();
                model.ResultId = Convert.ToInt32(dr[0]);
                model.ExamId = Convert.ToInt32(dr[1]);
                model.QuestionId = Convert.ToInt32(dr[2]);
                model.SelectedOption = Convert.ToString(dr[3]);
                model.CorrectOption = Convert.ToString(dr[4]);
                model.Result1 = Convert.ToInt32(dr[5]);
                results.Add(model);
            }
            if (results.Count() == 0)
            {
                return null;
            }
            else
            {
                return results;
            }
        }

        public IEnumerable<Analysis> GetQuestionsByUserAndTopic(int UserId, int TopicId, string Type)
        {
            string questionsQuery;
            connection.Open();
            if (Type == "all")
            {
                questionsQuery = "SELECT Q.QuestionId AS QId, Q.Question, Q.CorrectOption, R.SelectedOption, " +
                "Q.Option1, Q.Option2, Q.Option3, Q.Option4, Q.Option5, R.Result " +
                                       "FROM Questions Q " +
                                       "JOIN Result R ON Q.QuestionId = R.QuestionId " +
                                       "JOIN Exam E ON R.ExamId = E.ExamId " +
                                       $"WHERE E.UserId = {UserId} AND Q.TopicId = {TopicId}";
            }
            else
            {
                questionsQuery = "SELECT Q.QuestionId AS QId, Q.Question, Q.CorrectOption, R.SelectedOption, " +
                    "Q.Option1, Q.Option2, Q.Option3, Q.Option4, Q.Option5, R.Result " +
                                           "FROM Questions Q " +
                                           "JOIN Result R ON Q.QuestionId = R.QuestionId " +
                                           "JOIN Exam E ON R.ExamId = E.ExamId " +
                                           $"WHERE E.UserId = {UserId} AND Q.TopicId = {TopicId}" +
                                           $"AND E.ExamId = (SELECT MAX(ExamId) From Exam WHERE UserId = {UserId})";
            }
            SqlCommand questionsCommand = new SqlCommand(questionsQuery, connection);
            SqlDataReader reader = questionsCommand.ExecuteReader();

            List<Analysis> analysiss = new List<Analysis>();

            while (reader.Read())
            {
                Analysis analysis = new Analysis
                {
                    QuestionId = Convert.ToInt32(reader["QId"]),
                    QuestionText = reader["Question"].ToString(),
                    CorrectOption = reader["CorrectOption"].ToString(),
                    SelectedOption = reader["SelectedOption"].ToString(),
                    Option1 = reader["Option1"].ToString(),
                    Option2 = reader["Option2"].ToString(),
                    Option3 = reader["Option3"].ToString(),
                    Option4 = reader["Option4"].ToString(),
                    Option5 = reader["Option5"].ToString(),
                    Result = Convert.ToInt32(reader["Result"])
                };
                analysiss.Add(analysis);
            }
            connection.Close();
            return analysiss;
        }

        public List<Topic> GetTopicsForUser(int UserId)
        {
            string Query = $"SELECT DISTINCT T.TopicId, T.TopicName FROM Exam E JOIN Topic T ON E.TopicId = T.TopicId WHERE E.UserId = {UserId}";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            List<Topic> topics = new List<Topic>();
            foreach (DataRow dr in dt.Rows)
            {
                Topic topic = new Topic();
                topic.TopicId = Convert.ToInt32(dr[0]);
                topic.TopicName = Convert.ToString(dr[1]);
                topics.Add(topic);
            }
            return topics;
        }
    }
}