using PipAndIvory.Domain.Entities.AggregateRoots;

namespace PipAndIvory.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Player> Players { get; }

    DbSet<Game> Games { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
