UPDATE games
SET board       = @Board,
    move_count  = @MoveCount,
    turn_player = @TurnPlayer,
    status      = @Status
WHERE id = @Id;