using System.Collections.Generic;

namespace Model.Output.SurveyResult
{
    public class SurveyResult : Survey
    {
        public int TotalNumberOfQuestions { get; set; }
        public int NumberOfAnsweredQuestion { get; set; }
        public List<QuestionResult> QuestionResults { get; set; }
    }
}