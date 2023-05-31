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

    //public class EntityIdLong : IEntityIdLong
    //{
    //    public EntityIdLong(long id)
    //    {
    //        Id = id;
    //    }

    //    public EntityIdLong(IEntityIdLong entity)
    //    {
    //        Id = entity.Id;
    //    }

    //    public long Id { get; }
    //}

    //public sealed class StringEntityComparer : IEqualityComparer<IEntityId>
    //{
    //    private readonly StringComparer _comparer = StringComparer.InvariantCultureIgnoreCase;
    //    public bool Equals(IEntityId x, IEntityId y)
    //    {
    //        return _comparer.Equals(x.EntityId, y.EntityId);
    //    }

    //public int GetHashCode(IEntityId obj)
    //{
    //    return obj.EntityId.ToLower().GetHashCode();
    //}
    //}
}
