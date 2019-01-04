using System;
using System.Collections.Generic;

namespace Survey.Model.Output
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public IEnumerable<Question> Questions { get; set; }
    }
}
