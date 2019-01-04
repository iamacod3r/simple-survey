using System.Collections.Generic;
using System.Threading.Tasks;
using Survey.Model.Input;
using Survey.Model.Output.SurveyResult;

namespace Survey.Domain.Interfaces
{
    public interface ISurveyService
    {
        Task<Model.Output.Survey> CreateSurvey(Model.Input.Survey survey);

        Task<Model.Output.Question> CreateSurveyQuestion(Question question);

        Task<List<Model.Output.Question>> CreateSurveyQuestions(ICollection<Question> questions);

        Task<Model.Output.Survey> GetSurvey(int surveyId);

        Task<bool> UpdateQuestion(Question question);

        Task<bool> SaveAnswer(Answer answer);

        Task<SurveyResult> GetSurveyResult(int surveyId);
    }
}