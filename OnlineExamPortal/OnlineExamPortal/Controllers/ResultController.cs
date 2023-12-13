using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using OnlineExamPortal.Models;


namespace OnlineExamPortal.Controllers
{
    public class ResultController : Controller
    {
        [HttpGet]
        public IActionResult Index(int UserId, int TopicId)
        {
            DataFetcher d = new DataFetcher();
            return View(d.UidTidList(UserId, TopicId));
        }

        [HttpGet]
        public IActionResult PieChart(int UserId, int TopicId)
        {
            DataFetcher d = new DataFetcher();
            List<Result> rl = d.UidTidList(UserId, TopicId);
            int cas = rl.Count(r => r.Result1 == 1);
            int ias = rl.Count(r => r.Result1 == 0);
            var cl = new List<string>() { "Correct Answers", "IncorrectAnswers" };
            var cd = new List<int>() { cas, ias };
            var cc = new List<string> { "green", "red" };

            ViewBag.ChartLabels = Newtonsoft.Json.JsonConvert.SerializeObject(cl);
            ViewBag.ChartDatas = Newtonsoft.Json.JsonConvert.SerializeObject(cd);
            ViewBag.ChartColors = Newtonsoft.Json.JsonConvert.SerializeObject(cc);

            return View();
        }

        [HttpGet]
        public IActionResult ShowQuestions(int UserId, int TopicId)
        {
            DataFetcher d = new DataFetcher();
            var questions = d.GetQuestionsByUserAndTopic(UserId,TopicId);
            return Json(questions);
        }

        [HttpGet]
        public IActionResult ByTopics(int UserId)
        {
            DataFetcher d = new DataFetcher();
            var topics = d.GetTopicsForUser(UserId);
            var chartDataByTopic = new Dictionary<int, Graphical>();

            ViewBag.UserId = UserId;
            foreach (var topic in topics)
            {
                List<Result> rl = d.UidTidList(UserId, topic.TopicId);
                if (rl != null)
                {
                    //ViewData[topic] = topic.TopicID.ToString();
                    ViewData[topic.TopicId.ToString()] = topic.TopicName;
                    int cas = rl.Count(r => r.Result1 == 1);
                    int ias = rl.Count(r => r.Result1 == 0);
                    var chartLabels = new List<string>() { "Correct Answers", "IncorrectAnswers" }; // Implement this method to get labels for the topic
                    var chartData = new List<int>() { cas, ias };
                    var chartColors = new List<string> { "green", "red" };

                    chartDataByTopic[topic.TopicId] = new Graphical
                    {
                        Labels = Newtonsoft.Json.JsonConvert.SerializeObject(chartLabels),
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(chartData),
                        Colors = Newtonsoft.Json.JsonConvert.SerializeObject(chartColors)
                    };
                }
            }

            ViewBag.ChartDataByTopic = chartDataByTopic;

            return View();
        }

    }
}
