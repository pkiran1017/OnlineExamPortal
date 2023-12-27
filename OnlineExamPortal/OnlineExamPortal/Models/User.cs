﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace OnlineExamPortal.Models
{
    public partial class User
    {
        public User()
        {
            Exams = new HashSet<Exam>();
        }
       

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? UserRole { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
    }
}
