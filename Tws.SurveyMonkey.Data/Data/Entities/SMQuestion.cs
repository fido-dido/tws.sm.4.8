namespace Tws.SurveyMonkey.Data.Entities
{
    public class SMQuestion : IEntityId, ISMObjectId
    {
        public SMQuestion(long id, string questionEntityId, long smPageId, string questionType, string question, int order)
        {
            Id = id;
            ObjectId = questionEntityId;
            SMPageId = smPageId;
            QuestionType = questionType;
            Question = question;
            Order = order;
        }

        public SMQuestion(string questionEntityId, long smPageId, string questionType, string question, int order)
        {

            Id = default(int);
            ObjectId = questionEntityId;
            SMPageId = smPageId;
            QuestionType = questionType;
            Question = question;
            Order = order;
        }

        public long Id { get; set; }
        public string ObjectId { get; set; }
        public long SMPageId { get; set; }
        public string QuestionType { get; set; }
        public string Question { get; set; }
        public int Order { get; set; }
    }
}
