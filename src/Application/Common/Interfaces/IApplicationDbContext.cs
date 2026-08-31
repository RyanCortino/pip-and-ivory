using PipAndIvory.Domain.Entities;

namespace PipAndIvory.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Player> Players { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
