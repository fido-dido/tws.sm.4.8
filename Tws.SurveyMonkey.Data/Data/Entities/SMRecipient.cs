namespace Tws.SurveyMonkey.Data.Entities
{
    public class SMRecipient : IEntityId, ISMObjectId
    {
        public SMRecipient(long id, string recipientEntityId, long smCollectorId, string email, string phoneNumber)
        {
            Id = id;
            ObjectId = recipientEntityId;
            SMCollectorId = smCollectorId;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        public SMRecipient(string recipientEntityId, long smCollectorId, string email, string phoneNumber) 
        {
            Id = default(int); 
            ObjectId = recipientEntityId;
            SMCollectorId = smCollectorId;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public long Id { get; set; }
        public string ObjectId { get; set; }
        public long SMCollectorId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
