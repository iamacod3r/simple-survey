using System;
using System.Collections.Generic;

namespace Entity.Survey
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}