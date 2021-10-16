using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Input;
using Model.Output.SurveyResult;

namespace Domain.Interfaces
{
    public interface ISurveyService {

        Task<Model.Output.Survey> CreateSurvey(Model.Input.Survey survey);
        Task<Model.Output.Question> CreateSurveyQuestion(Model.Input.Question question);
        Task<IList<Model.Output.Question>> CreateSurveyQuestions(ICollection<Model.Input.Question> questions);
        Task<Model.Output.Survey> GetSurvey(int surveyId);
        Task<bool> UpdateQuestion(Model.Input.Question quesiton);
        Task<bool> SaveAnswer(Model.Input.Answer answer);
        Task<SurveyResult> GetSurveyResult(int surveyId);
    }
}