using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWS.ScheduledTask.SurveyMonkey.Models.SurveyMessageDetails
{


    public class SurveyMessageDetails
    {
        public string status { get; set; }
        public bool is_scheduled { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public bool is_branding_enabled { get; set; }
        public DateTime date_created { get; set; }
        public string type { get; set; }
        public string id { get; set; }
       // public object recipient_status { get; set; }
        public object scheduled_date { get; set; }
        public string edit_message_link { get; set; }
        public bool embed_first_question { get; set; }
        public string href { get; set; }
    }


}
