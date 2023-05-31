namespace Tws.SurveyMonkey.Data.Entities
{
    public class SMChoice : IEntityId, ISMObjectId
    {
        public SMChoice(long id, string choiceEntityId, long smQuestionId, string name, int order)
        {
            Id = id;
            ObjectId = choiceEntityId;
            SMQuestionId = smQuestionId;
            Name = name;
            Order = order;
        }

        public SMChoice(string choiceEntityId, long smQuestionId, string name, int order)
        {
            Id = default(int);
            ObjectId = choiceEntityId;
            SMQuestionId = smQuestionId;
            Name = name;
            Order = order;
        }

        public long Id { get; set; }
        public string ObjectId { get; set; }
        public long SMQuestionId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}
