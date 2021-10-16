using Repository.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Repository.Implements
{
    public class SurveyRepositoryInMemory : ISurveyRepository
    {
        private static int _lastSurveyId { get; set; }
        private static int _lastQuestionId { get; set; }
        private static ConcurrentDictionary<int, Lazy<Entity.Survey.Survey>> _surveyData { get; set; }
        private static ConcurrentDictionary<int, Lazy<List<Entity.Survey.SurveyQuestionAnswer>>> _surveyQuestionAnswerData { get; set; }

        #region DI
        private readonly ILogger<SurveyRepositoryInMemory> _logger;
        #endregion

        public SurveyRepositoryInMemory(ILogger<SurveyRepositoryInMemory> logger)
        {
            _logger = logger;
            
            if (_surveyData == null) {
                _surveyData = new ConcurrentDictionary<int, Lazy<Entity.Survey.Survey>>(); 
            }

            if (_surveyQuestionAnswerData == null) {
                _surveyQuestionAnswerData = new ConcurrentDictionary<int, Lazy<List<Entity.Survey.SurveyQuestionAnswer>>>();
            }
        }

        public async Task<Entity.Survey.Survey> CreateSurvey(Entity.Survey.Survey survey) {

            await Task.Run(() =>
            {
                survey.Id = ++_lastSurveyId;
                _surveyData.TryAdd(survey.Id, new Lazy<Entity.Survey.Survey>(survey));
            });            

            return survey;
        }

        public async Task<Entity.Survey.Question> AddQuestionToSurvey(int surveyId, Entity.Survey.Question question) {

            var survey = await GetSurveyById(surveyId);
            if (survey != null) {
                question.Id = ++_lastQuestionId;
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
                _surveyData.TryGetValue(id, out survey);
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
                _surveyQuestionAnswerData.TryGetValue(surveyQuestionAnswer.SurveyId, out Lazy<List<Entity.Survey.SurveyQuestionAnswer>> savedAnswer);

                if (savedAnswer == null)
                {
                    var newAnswerCollection = new Lazy<List<Entity.Survey.SurveyQuestionAnswer>>();
                    newAnswerCollection.Value.Add(surveyQuestionAnswer);
                    _surveyQuestionAnswerData.TryAdd(surveyQuestionAnswer.SurveyId, newAnswerCollection);
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
                _surveyQuestionAnswerData.TryGetValue(surveyId, out savedAnswer);
            });

            return savedAnswer?.Value;
        }

        public async Task<List<Entity.Survey.Survey>> GetSurveys()
        {
            List<Entity.Survey.Survey> result = null;
            await Task.Run(() =>
            {
                result = _surveyData.Select(s=> s.Value.Value).ToList();
            });

            return result;

        }
    }
}