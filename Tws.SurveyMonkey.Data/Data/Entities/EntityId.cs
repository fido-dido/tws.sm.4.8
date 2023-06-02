namespace Tws.SurveyMonkey.Data.Entities
{
    public interface IEntityId
    {
        long Id { get; }
    }

    public interface IEntityIdLong
    {
        long Id { get; }
    }

    public class EntityId : IEntityId
    { 
        public EntityId(long id)
        {
            Id = id;
        }

        public EntityId(IEntityId entity)
        {
            Id = entity.Id;
        }

        public long Id { get; }
    }
}
