using System;
using System.Collections.Generic;

namespace OnlineExamPortal.Models
{
    public partial class Topic
    {
        public Topic()
        {
            Exams = new HashSet<Exam>();
            Questions = new HashSet<Question>();
        }

        public int TopicId { get; set; }
        public string? TopicName { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
