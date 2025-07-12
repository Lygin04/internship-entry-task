CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE moves (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    game_id UUID NOT NULL,
    row INTEGER NOT NULL,
    col INTEGER NOT NULL,
    player CHAR(1) NOT NULL,
    UNIQUE (game_id, row, col, player)
);
