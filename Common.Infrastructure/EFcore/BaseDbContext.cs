using MediatR;
using Microsoft.EntityFrameworkCore;
using Zack.DomainCommons.Models;

namespace Common.Infrastructure.EFcore;

public class BaseDbContext: DbContext
{
    private IMediator? _mediator;

    public BaseDbContext(DbContextOptions options,IMediator? mediator):base(options)
    {
        _mediator = mediator;
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new NotImplementedException("Don not call SaveChanges, please call SaveChangesAsync instead.");
    }

    public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,CancellationToken cancellationToken)
    {
        if (_mediator != null)
        {
            await _mediator.DispatchDomainEventsAsync(this);
        }

        var softDeletedEntities = ChangeTracker.Entries<ISoftDelete>()
            .Where(e => e.State == EntityState.Modified && e.Entity.IsDeleted)
            .Select(e => e.Entity).ToList();
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        return result;
    }
}