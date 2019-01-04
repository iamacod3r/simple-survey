using System.Collections.Generic;
using System.Threading.Tasks;

namespace Survey.Repository.Interfaces
{
    public interface ISurveyRepository
    {
        Task<Entity.Survey.Survey> CreateSurvey(Entity.Survey.Survey survey);

        Task<Entity.Survey.Question> AddQuestionToSurvey(int surveyId, Entity.Survey.Question question);

        Task<Entity.Survey.Survey> GetSurveyById(int id);

        Task<bool> UpdateQuestion(int surveyId, Entity.Survey.Question question);

        Task<bool> SaveSurveyQuestionAnswer(Entity.Survey.SurveyQuestionAnswer surveyQuestionAnswer);

        Task<List<Entity.Survey.SurveyQuestionAnswer>> GetSurveyQuestionAnswers(int surveyId);

        Task<List<Entity.Survey.Survey>> GetSurveys();
    }
}