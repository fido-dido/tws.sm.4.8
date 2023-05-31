namespace Tws.SurveyMonkey.Data.Entities
{
    public class SMCollector : IEntityId, ISMObjectId
    {
        public SMCollector(long id, string collectorEntityId, long smSurveyId, string name, string type, string email)
        {
            Id = id;
            ObjectId = collectorEntityId;
            SMSurveyId = smSurveyId;
            Name = name;
            Type = type;
            Email = email;
        }
        public SMCollector(string collectorEntityId, long smSurveyId, string name, string type, string email)
        {
            Id = default(int);
            ObjectId = collectorEntityId;
            SMSurveyId = smSurveyId;
            Name = name;
            Type = type;
            Email = email;
        }

        public long Id { get; set; }
        public string ObjectId { get; set; }
        public long SMSurveyId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
    }
}
