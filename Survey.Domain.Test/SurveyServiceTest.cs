using Microsoft.VisualStudio.TestTools.UnitTesting;
using Survey.Domain.Implements;
using Survey.Domain.Interfaces;
using Survey.Repository.Implements;
using Survey.Repository.Interfaces;
using System;
using System.Linq;

namespace Survey.Domain.Test
{
    [TestClass]
    public class SurveyServiceTest
    {
        private ISurveyRepository surveyRepository;
        private ISurveyService surveyService;

        private readonly DateTime SurveyStartDateTime = DateTime.Now.AddDays(1);
        private readonly DateTime SurveyEndDateTime = DateTime.Now.AddDays(6);
        private const string SurveyTitle = "Test Title Survey";
        private const string SurveyDescription = "Test Desc Survey";

        private readonly Model.Input.Question Question1 = new Model.Input.Question
        {
            Text = "Do you live in California ?",
            YesAnswerText = "Of course!",
            NoAnswerText = "Nope",
        };

        private readonly Model.Input.Question Question2 = new Model.Input.Question
        {
            Text = "Do you speak English ?",
            YesAnswerText = "Yes",
            NoAnswerText = "No",
        };

        private readonly Model.Input.Answer QuestionAnswerYesOrTrue = new Model.Input.Answer {
             AnswerValue = true
        };

        private readonly Model.Input.Answer QuestionAnswerNoOrFalse = new Model.Input.Answer
        {
            AnswerValue = false
        };

        public SurveyServiceTest()
        {
            surveyRepository = new SurveyRepositoryInMemory();
            surveyService = new SurveyService(surveyRepository);
        }

        [TestMethod, Priority(1)]
        public void CreateSurvey()
        {

            Assert.IsNotNull(surveyService);

            var surveyInputModel = new Model.Input.Survey
            {
                Title = SurveyTitle,
                Description = SurveyDescription,
                StartDateTime = SurveyStartDateTime,
                EndDateTime = SurveyEndDateTime
            };

            var surveyOutputModel = surveyService.CreateSurvey(surveyInputModel).Result;

            Assert.IsNotNull(surveyOutputModel);
            Assert.IsTrue(surveyOutputModel.Id > 0);
            Assert.AreEqual(SurveyTitle, surveyOutputModel.Title);
            Assert.AreEqual(SurveyDescription, surveyOutputModel.Description);
            Assert.AreEqual(SurveyStartDateTime, surveyOutputModel.StartDateTime);
            Assert.AreEqual(SurveyEndDateTime, surveyOutputModel.EndDateTime);
        }

        [TestMethod, Priority(2)]
        public void CreateQuestion()
        {
            Assert.IsNotNull(surveyService);

            var surveys = surveyRepository.GetSurveys().Result;

            Assert.IsNotNull(surveys);
            Assert.IsTrue(surveys.Count > 0);

            var surveyId = surveys.First().Id;

            Question1.SurveyId = surveyId;
            Question2.SurveyId = surveyId;

            var questionOutputModel1 = surveyService.CreateSurveyQuestion(Question1).Result;
            var questionOutputModel2 = surveyService.CreateSurveyQuestion(Question2).Result;

            // Q1
            Assert.IsNotNull(questionOutputModel1);
            Assert.IsTrue(questionOutputModel1.QuestionId > 0);

            Assert.AreEqual(Question1.Text, questionOutputModel1.Text);
            Assert.AreEqual(surveyId, questionOutputModel1.SurveyId);

            Assert.IsNotNull(questionOutputModel1.Answers);

            Assert.AreEqual(Question1.YesAnswerText, questionOutputModel1.Answers.First(a=> a.Value).Text);
            Assert.AreEqual(Question1.NoAnswerText, questionOutputModel1.Answers.First(a => !a.Value).Text);

            // Q2
            Assert.IsNotNull(questionOutputModel2);
            Assert.IsTrue(questionOutputModel2.QuestionId > 0);

            Assert.AreEqual(Question2.Text, questionOutputModel2.Text);
            Assert.AreEqual(surveyId, questionOutputModel2.SurveyId);

            Assert.IsNotNull(questionOutputModel2.Answers);

            Assert.AreEqual(Question2.YesAnswerText, questionOutputModel2.Answers.First(a => a.Value).Text);
            Assert.AreEqual(Question2.NoAnswerText, questionOutputModel2.Answers.First(a => !a.Value).Text);

        }

        [TestMethod, Priority(3)]
        public void SaveSurveyQuestionAnswer() {

            var numberOfAnsweredQuestion = 1;
            var numberOfAnswered = 2;
            var totalNumberOfQuestion = 2;
            var numberofAnsweredNoOrFalse = 1;
            var numberofAnsweredYesOrTrue = 1;

            Assert.IsNotNull(surveyService);

            var surveys = surveyRepository.GetSurveys().Result;

            Assert.IsNotNull(surveys);
            Assert.IsTrue(surveys.Count > 0);

            var survey = surveys.First();

            Assert.IsNotNull(survey);
            Assert.AreEqual(SurveyTitle, survey.Title);
            Assert.AreEqual(SurveyDescription, survey.Description);

            Assert.IsNotNull(survey.Questions);
            Assert.IsTrue(survey.Questions.Count > 0);

            var ques = survey.Questions.First();

            Assert.IsNotNull(ques);
            Assert.AreEqual(Question1.Text, ques.Text);

            QuestionAnswerYesOrTrue.SurveyId = survey.Id;
            QuestionAnswerYesOrTrue.QuestionId = ques.Id;

            QuestionAnswerNoOrFalse.SurveyId = survey.Id;
            QuestionAnswerNoOrFalse.QuestionId = ques.Id;

            surveyService.SaveAnswer(QuestionAnswerYesOrTrue);
            surveyService.SaveAnswer(QuestionAnswerNoOrFalse);

            var surveyAnswers = surveyService.GetSurveyResult(survey.Id).Result;
            
            Assert.IsNotNull(surveyAnswers);
            Assert.AreEqual(totalNumberOfQuestion, surveyAnswers.TotalNumberOfQuestions);
            Assert.AreEqual(numberOfAnsweredQuestion, surveyAnswers.NumberOfAnsweredQuestion);

            var answeredQuestion = surveyAnswers.QuestionResults.First(q=> q.QuestionId == ques.Id);

            Assert.IsNotNull(answeredQuestion);

            Assert.AreEqual(numberOfAnswered, answeredQuestion.NumberOfAnswered);
            Assert.AreEqual(numberofAnsweredYesOrTrue, answeredQuestion.NumberOfYesOrTrueAnswered);
            Assert.AreEqual(numberofAnsweredNoOrFalse, answeredQuestion.NumberOfNoOrFalseAnswered);

        }
    }
}
