namespace Common.DomainCommons;

public interface IHasModificationTime
{
    DateTime? ModificationTime { get; }
}