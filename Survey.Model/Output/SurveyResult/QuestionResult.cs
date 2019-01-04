namespace Survey.Model.Output.SurveyResult
{
    public class QuestionResult : Question
    {
        public int NumberOfAnswered { get; set; }
        public int NumberOfYesOrTrueAnswered { get; set; }
        public int NumberOfNoOrFalseAnswered { get; set; }
    }
}