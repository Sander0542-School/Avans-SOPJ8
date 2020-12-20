using System;
using System.Collections.Generic;

namespace Bumbo.Web.Models.Schedule
{
    public class EventViewModel
    {
        public int Id { get; set; }

        public String Title { get; set; }

        public String Start { get; set; }

        public String End { get; set; }

        public bool AllDay { get; set; }
        
        public Dictionary<string, object> ExtendedProps { get; set; }
    }
}
