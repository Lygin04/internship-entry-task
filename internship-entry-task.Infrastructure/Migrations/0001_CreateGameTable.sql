CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE games (
                      id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                      board text NOT NULL,
                      n INTEGER NOT NULL,
                      move_count INTEGER NOT NULL,
                      turn_player CHAR(1) NOT NULL,
                      status INTEGER NOT NULL
);
