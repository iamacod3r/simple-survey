using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Input
{
    public class Survey : IValidatableObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Title)) {
                yield return new ValidationResult("Title is required.", new[] { nameof(Title) });
            }
            if (string.IsNullOrWhiteSpace(Description))
            {
                yield return new ValidationResult("Description is required.", new[] { nameof(Description) });
            }
            if (StartDateTime < DateTime.Now)
            {
                yield return new ValidationResult("Start Date and Time must be after the present date and time.", new[] { nameof(StartDateTime) });
            }
            if (EndDateTime < StartDateTime)
            {
                yield return new ValidationResult("End Date and Time must be a value greater than Start Date and Time.", new[] { nameof(EndDateTime) });
            }
        }
    }
}