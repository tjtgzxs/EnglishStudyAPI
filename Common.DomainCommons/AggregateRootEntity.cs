namespace Common.DomainCommons;

public record AggregateRootEntity:BaseEntity, IAggregateRoot,ISoftDelete,IHasCreationTime,IHasModificationTime,IHasDeletionTime
{
    public bool IsDeleted { get; private set; }
    public DateTime CreationTime { get; private set; }=DateTime.Now;
    public DateTime? ModificationTime { get; private set; }
    public DateTime? DeletionTime { get; private set; }
    public void SoftDelete()
    {
        IsDeleted=true;
        DeletionTime=DateTime.Now;
    }

    public void NotifyModified()
    {
        ModificationTime=DateTime.Now;
    }


}