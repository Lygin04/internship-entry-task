namespace internship_entry_task.Domain.Entities;

public class DbMove
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public char Player { get; set; }
}