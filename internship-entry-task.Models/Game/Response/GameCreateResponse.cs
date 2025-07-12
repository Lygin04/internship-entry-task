using internship_entry_task.Models.Game.Enums;

namespace internship_entry_task.Models.Game.Response;

public class GameCreateResponse
{
    public Guid Id { get; set; }
    public int N {get; set;}
    public char TurnPlayer { get; set; }
}