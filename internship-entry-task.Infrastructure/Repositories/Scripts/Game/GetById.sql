SELECT id           AS Id,
       board        AS Board,
       n            AS N,
       move_count   AS MoveCount,
       turn_player  AS TurnPlayer,
       status       AS Status
FROM games
WHERE id = @id;