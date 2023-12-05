using System;
using System.Collections.Generic;

namespace OnlineExamPortal.Models
{
    public partial class Question
    {
        public Question()
        {
            Results = new HashSet<Result>();
        }

        public int QuestionId { get; set; }
        public string? Type { get; set; }
        public string? Question1 { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Option5 { get; set; }
        public string? CorrectOption { get; set; }
        public int? TopicId { get; set; }
        public string? DifficultyLevel { get; set; }

        public virtual Topic? Topic { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
