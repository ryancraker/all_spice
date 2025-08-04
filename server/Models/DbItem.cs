namespace all_spice.Models;

public abstract class DbItem<T>
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
