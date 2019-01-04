using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Survey.Model.Input
{
    public class Answer : IValidatableObject
    {
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        public bool? AnswerValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SurveyId < 1)
            {
                yield return new ValidationResult("Survey Id is required.", new[] { nameof(SurveyId) });
            }

            if (QuestionId < 1)
            {
                yield return new ValidationResult("Survey Id is required.", new[] { nameof(QuestionId) });
            }

            if (AnswerValue == null)
            {
                yield return new ValidationResult("Answer Value is required.", new[] { nameof(Answer) });
            }
        }
    }
}