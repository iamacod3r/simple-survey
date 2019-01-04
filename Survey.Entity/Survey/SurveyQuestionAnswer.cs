namespace Survey.Entity.Survey
{
    public class SurveyQuestionAnswer
    {
        public int SurveyId { get; set; }
        public int QuestionId { get; set;}
        public bool Answer { get; set; }
    }
}