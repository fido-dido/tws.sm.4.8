using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tws.SurveyMonkey.Data.Entities;

namespace Tws.SurveyMonkey.Data.Data
{
    public class SMMessage : IEntityId, ISMObjectId
    {
        public SMMessage(long id, string messageEntityId, long smCollectorId, string status, bool isScheduled, bool  embedFirstQuestion, string subject, string htmlBody, string type, DateTime scheduledDate)
        {
            Id = id;
            ObjectId = messageEntityId;
            SMCollectorId = smCollectorId; 
            Status = status; 
            IsScheduled = isScheduled;
            EmbedFirstQuestion = embedFirstQuestion; 
            Subject = subject;
            HtmlBody = htmlBody; 
            Type = type;
            ScheduledDate = scheduledDate;
        }
        public SMMessage(string messageEntityId, long smCollectorId, string status, bool isScheduled, bool embedFirstQuestion, string subject, string htmlBody, string type, DateTime scheduledDate)
        {
            Id = default;
            ObjectId = messageEntityId;
            SMCollectorId = smCollectorId;
            Status = status;
            IsScheduled = isScheduled;
            EmbedFirstQuestion = embedFirstQuestion;
            Subject = subject;
            HtmlBody = htmlBody;
            Type = type;
            ScheduledDate = scheduledDate;
        }



        public long Id { get; set; }
        public long SMCollectorId { get; set; }
        public string ObjectId { get; set; }
        public string Status { get; set; }
        public bool IsScheduled { get; set; }
        public bool EmbedFirstQuestion { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string Type { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
