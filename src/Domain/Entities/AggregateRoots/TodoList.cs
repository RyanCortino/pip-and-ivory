namespace PipAndIvory.Domain.Entities.AggregateRoots;

public class TodoList : BaseAuditableEntity<int>
{
    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.Grey;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}
