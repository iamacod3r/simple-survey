using System.Collections.Generic;

namespace Survey.Model.Output
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public string Text { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}