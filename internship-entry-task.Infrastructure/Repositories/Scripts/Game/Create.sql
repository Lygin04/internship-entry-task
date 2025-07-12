INSERT INTO games (board, n, move_count, turn_player, status)
VALUES (@Board, @N, @MoveCount, @TurnPlayer, @Status)
RETURNING id;