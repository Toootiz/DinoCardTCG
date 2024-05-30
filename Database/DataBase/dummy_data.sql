USE dinocard_db;

-- Insertar datos en la tabla habilidad
INSERT INTO habilidad (id_habilidad, descripcion) VALUES 
(1,'Ataque rápido'),
(2, 'Defensa fuerte'),
(3, 'Curación'),
(1, 'Defensa leve'),
(2, 'Huye del ataque'),
(3, 'Hace magia'),
(1, 'Vuela rápido'),
(2, 'Escupe fuego'),
(3, 'Nada como tiburón'),
(2, 'Defensa hasta morir');

-- Insertar datos en la tabla Jugador
INSERT INTO Jugador (id_jugador, nombre, partidas_ganadas, partidas_perdidas, fecha_creacion) VALUES 
(1, 'Alice', 10, 5, UNIX_TIMESTAMP('2024-01-01 10:00:00')),
(2, 'Bob', 8, 7, UNIX_TIMESTAMP('2024-01-05 15:30:00')),
(3, 'Charlie', 12, 3, UNIX_TIMESTAMP('2024-01-10 20:45:00')),
(1, 'Tom', 11, 2, UNIX_TIMESTAMP('2024-01-01 10:00:00')),
(2, 'Mike', 1, 9, UNIX_TIMESTAMP('2024-01-05 15:30:00')),
(3, 'Andy', 0, 1, UNIX_TIMESTAMP('2024-01-10 20:45:00')),
(1, 'Peter', 12, 5, UNIX_TIMESTAMP('2024-01-01 10:00:00')),
(2, 'Fer', 5, 8, UNIX_TIMESTAMP('2024-01-05 15:30:00')),
(3, 'Julio', 12, 3, UNIX_TIMESTAMP('2024-01-10 20:45:00'));

-- Insertar datos en la tabla Carta
INSERT INTO Carta (id_carta, nombre, tipo, puntos_de_vida, puntos_de_ataque, coste_en_elixir, habilidad) VALUES 
(1, 'Dragón de Fuego', 'Ataque', 100, 50, 5, 1), -- habilidad 1
(2, 'Guerrero de Hielo', 'Defensa', 80, 40, 4, 2), -- habilidad 2
(3, 'Hechicera', 'Hechizo', 53, 17, 1, 2), -- habilidad 3
(1, 'Dragón de agua', 'Defensa', 70, 20, 2, 1),
(2, 'Ave', 'Ataque', 60, 20, 3, 3),
(3, 'Hechicero', 'Hechizo', 60, 20, 3, 3);

-- Insertar datos en la tabla Mano
INSERT INTO Mano (id_mano, id_carta, id_jugador, fecha_de_creacion) VALUES 
(1, 1, 1, UNIX_TIMESTAMP('2024-01-01 10:00:00')),
(2, 2, 2, UNIX_TIMESTAMP('2024-01-01 10:00:00')),
(3, 3, 3, UNIX_TIMESTAMP('2024-01-05 15:30:00')),
(1, 1, 4, UNIX_TIMESTAMP('2024-01-06 10:00:32')),
(2, 2, 5, UNIX_TIMESTAMP('2024-01-01 10:00:54')),
(3, 3, 6, UNIX_TIMESTAMP('2024-01-09 15:30:52')),
(1, 1, 7, UNIX_TIMESTAMP('2024-01-11 10:00:15')),
(2, 2, 8, UNIX_TIMESTAMP('2024-01-12 10:25:00')),
(3, 3, 9, UNIX_TIMESTAMP('2024-01-05 15:59:00'));

-- Insertar datos en la tabla Deck
INSERT INTO Deck (id_deck, cantidad_cartas, id_jugador, id_carta) VALUES 
(1, 30, 1, 1),
(2, 40, 2, 2),
(3, 50, 3, 3),
(1, 30, 1, 1),
(2, 40, 2, 2),
(3, 50, 3, 3),
(1, 30, 1, 1),
(2, 40, 2, 2),
(3, 50, 3, 3);

-- Insertar datos en la tabla Partida
INSERT INTO Partida (id_jugador1, id_jugador2, id_ganador, id_perdedor, cantidad_turnos) VALUES 
(1, 2, 1, 2, 15),
(2, 3, 2, 3, 10),
(1, 3, 3, 1, 20),
(1, 2, 1, 2, 15),
(2, 3, 2, 3, 10),
(1, 3, 3, 1, 20), 
(1, 2, 1, 2, 15),
(2, 3, 2, 3, 10),
(1, 3, 3, 1, 20);

-- Insertar datos en la tabla Credenciales
INSERT INTO credenciales (id_credencial, id_jugador, nombre, contraseña) VALUES 
('cred1', 1, 'Alice', 'password123'),
('cred2', 2, 'Bob', 'password456'),
('cred3', 3, 'Charlie', 'password789'), 
('cred4', 4, 'Tom', 'password987'),
('cred5', 5, 'Mike', 'password654'),
('cred6', 6, 'Andy', 'password321'),
('cred7', 7, 'Peter', 'password555'),
('cred8', 8, 'Fer', 'password666'),
('cred9', 9, 'Julio', 'password777');


SELECT * FROM vista_detalles_cartas;
SELECT * FROM vista_estadisticas_jugadores;
SELECT * FROM vista_decks_cartas;
SELECT * FROM vista_resultados_partidas;
SELECT * FROM vista_credenciales_jugadores;