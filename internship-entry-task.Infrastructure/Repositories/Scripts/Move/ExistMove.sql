SELECT
    CASE
        WHEN EXISTS (
            SELECT 1
            FROM moves
            WHERE game_id = @GameId AND row = @Row AND col = @Col AND player = @Player
        ) THEN true
        ELSE false
        END;
