using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWS.ScheduledTask.SurveyMonkey.Models.SurveyMessage
{

    public class SurveyMessage
    {
        public Datum[] data { get; set; }
        public int per_page { get; set; }
        public int page { get; set; }
        public int total { get; set; }
        public Links links { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }

    public class Datum
    {
        public string status { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string href { get; set; }
    }

}
