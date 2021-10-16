using System.Collections.Generic;

namespace Model.Output
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public string Text { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}