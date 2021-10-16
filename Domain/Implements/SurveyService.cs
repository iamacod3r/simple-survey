using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Model.Output.SurveyResult;
using Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Domain.Implements 
{
    public class SurveyService : ISurveyService
    {

        #region DI
        private readonly ILogger<SurveyService> _logger;
        private readonly ISurveyRepository _repository;
        #endregion 

        public SurveyService(ILogger<SurveyService> logger, ISurveyRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Model.Output.Survey> CreateSurvey(Model.Input.Survey survey)
        {
            var newSurveyEntity = new Entity.Survey.Survey 
            {
                Title = survey.Title,
                Description = survey.Description,
                StartDateTime = survey.StartDateTime,
                EndDateTime = survey.EndDateTime,
                CreatedDateTime = DateTime.Now
            };

            newSurveyEntity = await _repository.CreateSurvey(newSurveyEntity);

            return await GetSurvey(newSurveyEntity.Id);
        }

        public async Task<Model.Output.Question> CreateSurveyQuestion(Model.Input.Question question)
        {
            var survey = await _repository.GetSurveyById(question.SurveyId);
            if (survey != null)
            {
                var newQuestion = new Entity.Survey.Question
                {
                    Text = question.Text,
                    CreatedDateTime = DateTime.Now,
                    UpdatedDateTime = DateTime.Now,
                    Answers = new List<Entity.Survey.Answer>
                        {
                            new Entity.Survey.Answer
                            {
                                 Text = question.YesAnswerText,
                                 Value = true
                            },
                            new Entity.Survey.Answer
                            {
                                 Text = question.NoAnswerText,
                                 Value = false
                            }
                        }
                };
                newQuestion = await _repository.AddQuestionToSurvey(question.SurveyId, newQuestion);
                var result = new Model.Output.Question
                {
                    QuestionId = newQuestion.Id,
                    SurveyId = question.SurveyId,
                    Text = newQuestion.Text,
                    Answers = newQuestion.Answers.Select(a => new Model.Output.Answer { Text = a.Text, Value = a.Value })
                };

                return result;
            }

            return null;
        }

        public async Task<IList<Model.Output.Question>> CreateSurveyQuestions(ICollection<Model.Input.Question> questions)
        {
            var result = new List<Model.Output.Question>();
            await Task.Run(() =>
            Parallel.ForEach(questions, async q =>
            {
                var tempResult = await CreateSurveyQuestion(q);
                if (tempResult != null)
                {
                    result.Add(tempResult);
                }
            }));

            return result;
        }

        public async Task<Model.Output.Survey> GetSurvey(int surveyId)
        {
            var surveyEntity = await _repository.GetSurveyById(surveyId);
            if (surveyEntity == null)
            {
                return null;
            }

            var surveyModel = new Model.Output.Survey
            {
                Id = surveyEntity.Id,
                Title = surveyEntity.Title,
                Description = surveyEntity.Description,
                StartDateTime = surveyEntity.StartDateTime,
                EndDateTime = surveyEntity.EndDateTime
            };
            if (surveyEntity.Questions != null)
            {
                surveyModel.Questions = surveyEntity.Questions.Select(q => new Model.Output.Question
                {
                    SurveyId = surveyEntity.Id,
                    QuestionId = q.Id,
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new Model.Output.Answer
                    {
                        Text = a.Text,
                        Value = a.Value
                    })

                });
            }

            return surveyModel;
        }

        public async Task<bool> UpdateQuestion(Model.Input.Question question)
        {
            var survey = await _repository.GetSurveyById(question.SurveyId);
            if (survey.Questions != null && survey.Questions.Any())
            {
                var ques = survey.Questions.FirstOrDefault(q => q.Id == question.QuestionId);
                if (ques != null)
                {

                    ques.Text = question.Text;
                    ques.UpdatedDateTime = DateTime.Now;
                    ques.Answers = new List<Entity.Survey.Answer>
                    {
                        new Entity.Survey.Answer
                        {
                             Text = question.YesAnswerText,
                             Value = true
                        },
                        new Entity.Survey.Answer
                        {
                            Text = question.NoAnswerText,
                            Value = false
                        }
                    };

                    return await _repository.UpdateQuestion(question.SurveyId, ques);
                }
            }
            return false;
        }

        public async Task<bool> SaveAnswer(Model.Input.Answer answer)
        {

            var checkSurvey = await GetSurvey(answer.SurveyId);
            if (checkSurvey == null)
            {
                return false;
            }

            if (!checkSurvey.Questions.Any(q => q.QuestionId == answer.QuestionId))
            {
                return false;
            }

            var answerEntity = new Entity.Survey.SurveyQuestionAnswer
            {
                SurveyId = answer.SurveyId,
                QuestionId = answer.QuestionId,
                Answer = answer.AnswerValue.Value
            };

            var result = await _repository.SaveSurveyQuestionAnswer(answerEntity);

            return result;
        }

        public async Task<SurveyResult> GetSurveyResult(int surveyId)
        {
            var survey = await GetSurvey(surveyId);

            if (survey == null)
            {
                return null;
            }

            var savedAnswers = await _repository.GetSurveyQuestionAnswers(surveyId);

            var result = new SurveyResult
            {
                Id = survey.Id,
                Title = survey.Title,
                Description = survey.Description,
                StartDateTime = survey.StartDateTime,
                EndDateTime = survey.EndDateTime,
                TotalNumberOfQuestions = survey.Questions.Count(),
                NumberOfAnsweredQuestion = savedAnswers != null ? savedAnswers.GroupBy(a=> a.QuestionId).Count() : 0,
                QuestionResults = new List<QuestionResult>()
            };

            foreach (var q in survey.Questions)
            {
                var quesSummary = new QuestionResult
                {
                    Text = q.Text,
                    QuestionId = q.QuestionId,
                    SurveyId = q.SurveyId,
                    Answers = q.Answers
                };

                if (savedAnswers != null)
                {
                    var currentQuesAnswers = savedAnswers.Where(a => a.SurveyId == surveyId && a.QuestionId == q.QuestionId);
                    if (currentQuesAnswers != null && currentQuesAnswers.Any())
                    {
                        quesSummary.NumberOfAnswered = currentQuesAnswers.Count();
                        quesSummary.NumberOfYesOrTrueAnswered = currentQuesAnswers.Count(a => a.Answer);
                        quesSummary.NumberOfNoOrFalseAnswered = currentQuesAnswers.Count(a => !a.Answer);
                    }
                }

                result.QuestionResults.Add(quesSummary);
            }

            return result;
        }
    }
}