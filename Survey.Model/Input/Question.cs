using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Survey.Model.Input
{
    public class Question : IValidatableObject
    {
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public string Text { get; set; }
        public string YesAnswerText { get; set; }
        public string NoAnswerText { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SurveyId < 1)
            {
                yield return new ValidationResult("Survey Id is required.", new[] { nameof(SurveyId) });
            }

            if (string.IsNullOrWhiteSpace(Text))
            {
                yield return new ValidationResult("Text is required.", new[] { nameof(Text) });
            }

            if (string.IsNullOrWhiteSpace(YesAnswerText))
            {
                yield return new ValidationResult("YesAnswerText is required.", new[] { nameof(YesAnswerText) });
            }

            if (string.IsNullOrWhiteSpace(NoAnswerText))
            {
                yield return new ValidationResult("NoAnswerText is required.", new[] { nameof(NoAnswerText) });
            }
        }
    }
}