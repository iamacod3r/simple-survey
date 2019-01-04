using Survey.Repository.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Survey.Repository.Implements
{
    public class SurveyRepositoryInMemory : ISurveyRepository
    {
        private static int LastSurveyId { get; set; }
        private static int LastQuestionId { get; set; }
        private static ConcurrentDictionary<int, Lazy<Entity.Survey.Survey>> SurveyData { get; set; }
        private static ConcurrentDictionary<int, Lazy<List<Entity.Survey.SurveyQuestionAnswer>>> SurveyQuestionAnswerData { get; set; }

        public SurveyRepositoryInMemory()
        {
            if (SurveyData == null) {
                SurveyData = new ConcurrentDictionary<int, Lazy<Entity.Survey.Survey>>(); 
            }

            if (SurveyQuestionAnswerData == null) {
                SurveyQuestionAnswerData = new ConcurrentDictionary<int, Lazy<List<Entity.Survey.SurveyQuestionAnswer>>>();
            }
        }

        public async Task<Entity.Survey.Survey> CreateSurvey(Entity.Survey.Survey survey) {

            await Task.Run(() =>
            {
                survey.Id = ++LastSurveyId;
                SurveyData.TryAdd(survey.Id, new Lazy<Entity.Survey.Survey>(survey));
            });            

            return survey;
        }

        public async Task<Entity.Survey.Question> AddQuestionToSurvey(int surveyId, Entity.Survey.Question question) {

            var survey = await GetSurveyById(surveyId);
            if (survey != null) {
                question.Id = ++LastQuestionId;
                if (survey.Questions == null) {
                    survey.Questions = new List<Entity.Survey.Question>();
                }
                survey.Questions.Add(question);
            }

            return question;
        }

        public async Task<Entity.Survey.Survey> GetSurveyById(int id)
        {
            Lazy<Entity.Survey.Survey> survey = null;
            await Task.Run(() =>
            {
                SurveyData.TryGetValue(id, out survey);
            });

            if(survey != null)
            {
                return survey.Value;
            }

            return null;
        }

        public async Task<bool> UpdateQuestion(int surveyId, Entity.Survey.Question question) {

            var survey = await GetSurveyById(surveyId);

            if (survey.Questions != null && survey.Questions.Any()) {
                var ques = survey.Questions.FirstOrDefault(q => q.Id == question.Id);
                if (ques != null) {

                    ques.Text = question.Text;
                    ques.UpdatedDateTime = question.UpdatedDateTime;
                    ques.Answers = question.Answers;
                    return true;
                }
            
            }
            return false;
        }

        public async Task<bool> SaveSurveyQuestionAnswer(Entity.Survey.SurveyQuestionAnswer surveyQuestionAnswer) {

            await Task.Run(() =>
            {
                SurveyQuestionAnswerData.TryGetValue(surveyQuestionAnswer.SurveyId, out Lazy<List<Entity.Survey.SurveyQuestionAnswer>> savedAnswer);

                if (savedAnswer == null)
                {
                    var newAnswerCollection = new Lazy<List<Entity.Survey.SurveyQuestionAnswer>>();
                    newAnswerCollection.Value.Add(surveyQuestionAnswer);
                    SurveyQuestionAnswerData.TryAdd(surveyQuestionAnswer.SurveyId, newAnswerCollection);
                }
                else
                {
                    savedAnswer.Value.Add(surveyQuestionAnswer);
                }
            });

            return true;
        }

        public async Task<List<Entity.Survey.SurveyQuestionAnswer>> GetSurveyQuestionAnswers(int surveyId) {

            Lazy<List<Entity.Survey.SurveyQuestionAnswer>> savedAnswer = null;
            await Task.Run(()=> {
                SurveyQuestionAnswerData.TryGetValue(surveyId, out savedAnswer);
            });

            return savedAnswer?.Value;
        }

        public async Task<List<Entity.Survey.Survey>> GetSurveys()
        {
            List<Entity.Survey.Survey> result = null;
            await Task.Run(() =>
            {
                result = SurveyData.Select(s=> s.Value.Value).ToList();
            });

            return result;

        }
    }
}