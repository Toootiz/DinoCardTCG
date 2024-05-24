-- Crear el esquema si no existe
DROP SCHEMA IF EXISTS dinocard_db;
CREATE SCHEMA dinocard_db;
USE dinocard_db;

-- Crear la tabla habilidad
CREATE TABLE habilidad (
    id_habilidad INT NOT NULL AUTO_INCREMENT,
    descripcion TEXT,
    PRIMARY KEY (id_habilidad)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Crear la tabla Jugador
CREATE TABLE Jugador (
    id_jugador INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    partidas_ganadas INT NOT NULL,
    partidas_perdidas INT NOT NULL,
    fecha_creacion INT,
    fecha_modificacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Crear la tabla Carta
CREATE TABLE Carta (
    id_carta INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    tipo ENUM('Ataque', 'Defensa', 'Hechizo') NOT NULL,
    puntos_de_vida INT,
    puntos_de_ataque INT,
    coste_en_elixir INT,
    habilidad INT,
    FOREIGN KEY (habilidad) REFERENCES habilidad(id_habilidad)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Crear la tabla Mano (deck del jugador)
CREATE TABLE Mano (
    id_mano INT AUTO_INCREMENT PRIMARY KEY,
    id_carta INT,
    id_jugador INT,
    fecha_de_creacion INT,
    
    fecha_modificacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (id_carta) REFERENCES Carta(id_carta),
    FOREIGN KEY (id_jugador) REFERENCES Jugador(id_jugador)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Crear la tabla Deck (Mazo)
CREATE TABLE Deck (
    id_deck INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    cantidad_cartas INT,
    id_jugador INT,
    id_carta INT NOT NULL,
    FOREIGN KEY (id_jugador) REFERENCES Jugador(id_jugador),
    FOREIGN KEY (id_carta) REFERENCES Carta(id_carta)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Crear la tabla Partida
CREATE TABLE Partida (
    id_partida INT AUTO_INCREMENT PRIMARY KEY,
    id_jugador1 INT NOT NULL,
    id_jugador2 INT NOT NULL,
    id_ganador INT NOT NULL,
    id_perdedor INT NOT NULL,
    cantidad_turnos INT NOT NULL,
    
    FOREIGN KEY (id_jugador1) REFERENCES Jugador(id_jugador),
    FOREIGN KEY (id_jugador2) REFERENCES Jugador(id_jugador)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Crear la tabla Credenciales
CREATE TABLE credenciales (
    id_credencial VARCHAR(255),
    id_jugador INT,
    nombre VARCHAR(255),
    contraseña VARCHAR(255),
    FOREIGN KEY (id_jugador) REFERENCES Jugador(id_jugador)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- Insertar datos en la tabla habilidad
INSERT INTO habilidad (id_habilidad, descripcion) VALUES 
(1,'Ataque rápido'), -- id_habilidad = 1
(2, 'Defensa fuerte'), -- id_habilidad = 2
(3, 'Curación');      -- id_habilidad = 3

-- Insertar datos en la tabla Jugador
INSERT INTO Jugador (id_jugador, nombre, partidas_ganadas, partidas_perdidas, fecha_creacion) VALUES 
(1, 'Alice', 10, 5, UNIX_TIMESTAMP('2024-01-01 10:00:00')),
(2, 'Bob', 8, 7, UNIX_TIMESTAMP('2024-01-05 15:30:00')),
(3, 'Charlie', 12, 3, UNIX_TIMESTAMP('2024-01-10 20:45:00'));

-- Insertar datos en la tabla Carta
INSERT INTO Carta (id_carta, nombre, tipo, puntos_de_vida, puntos_de_ataque, coste_en_elixir, habilidad) VALUES 
(1, 'Dragón de Fuego', 'Ataque', 100, 50, 5, 1), -- habilidad 1
(2, 'Guerrero de Hielo', 'Defensa', 80, 40, 4, 2), -- habilidad 2
(3, 'Hechicera', 'Hechizo', 60, 20, 3, 3); -- habilidad 3

-- Insertar datos en la tabla Mano
INSERT INTO Mano (id_mano, id_carta, id_jugador, fecha_de_creacion) VALUES 
(1, 1, 1, UNIX_TIMESTAMP('2024-01-01 10:00:00')),
(2, 2, 1, UNIX_TIMESTAMP('2024-01-01 10:00:00')),
(3, 3, 2, UNIX_TIMESTAMP('2024-01-05 15:30:00'));

-- Insertar datos en la tabla Deck
INSERT INTO Deck (id_deck, cantidad_cartas, id_jugador, id_carta) VALUES 
(1, 30, 1, 1),
(2, 40, 2, 2),
(3, 50, 3, 3);

-- Insertar datos en la tabla Partida
INSERT INTO Partida (id_jugador1, id_jugador2, id_ganador, id_perdedor, cantidad_turnos) VALUES 
(1, 2, 1, 2, 15),
(2, 3, 2, 3, 10),
(1, 3, 3, 1, 20);

-- Insertar datos en la tabla Credenciales
INSERT INTO credenciales (id_credencial, id_jugador, nombre, contraseña) VALUES 
('cred1', 1, 'Alice', 'password123'),
('cred2', 2, 'Bob', 'password456'),
('cred3', 3, 'Charlie', 'password789');


SELECT * FROM Jugador;
SELECT * FROM Carta;
SELECT * FROM Mano;
SELECT * FROM Deck;
SELECT * FROM Partida;
SELECT * FROM credenciales;

-- tottal de jugadores
SELECT COUNT(*) AS total_jugadores FROM Jugador;
-- jugador con más partidas ganadas
SELECT nombre, partidas_ganadas FROM Jugador ORDER BY partidas_ganadas DESC LIMIT 1;
-- total de cartas por tipo
SELECT tipo, COUNT(*) AS total_cartas FROM Carta GROUP BY tipo;
-- partidas ganadas de un jugador
SELECT *  FROM Partida WHERE id_ganador = 1;
-- obtener el mazo (hand) de un jugador
SELECT Deck.id_deck, Deck.cantidad_cartas, Carta.nombre, Carta.tipo, Carta.puntos_de_vida, Carta.puntos_de_ataque, Carta.coste_en_elixir
FROM Deck
JOIN Carta ON Deck.id_carta = Carta.id_carta
WHERE Deck.id_jugador = 1;
-- Obtener una lista de todos los jugadores junto con sus cartas
SELECT Jugador.nombre AS jugador_nombre, Carta.nombre AS carta_nombre
FROM Jugador
JOIN Mano ON Jugador.id_jugador = Mano.id_jugador
JOIN Carta ON Mano.id_carta = Carta.id_carta;





