namespace Common.DomainCommons;

public interface IHasDeletionTime
{
    DateTime? DeletionTime { get; }
}