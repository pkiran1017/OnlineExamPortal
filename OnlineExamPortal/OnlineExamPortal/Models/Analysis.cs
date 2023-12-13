namespace OnlineExamPortal.Models
{
    public class Analysis
    {
        public int QuestionId { get; set; }
        public string Type { get; set; }
        public string QuestionText { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string CorrectOption { get; set; }
        public int TopicId { get; set; }
        public string DifficultyLevel { get; set; }
        public string? SelectedOption { get; internal set; }
        public int Result { get; internal set; }
    }
}

