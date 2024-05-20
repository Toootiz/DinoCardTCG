DROP SCHEMA IF EXISTS dinocard_db;
CREATE SCHEMA dinocard_db;
USE dinocard_db;


-- Table forf `habilidad`

CREATE TABLE habilidad (
    id_habilidad INT NOT NULL AUTO_INCREMENT,
    descripcion TEXT,
    PRIMARY KEY (id_habilidad)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Table for `carta`

CREATE TABLE carta (
  id_carta INT NOT NULL AUTO_INCREMENT,
  nombre VARCHAR(255),
  puntos_de_vida INT,
  puntos_de_ataque INT,
  coste_en_elixir INT,
  habilidad INT,
  
  PRIMARY KEY (id_carta),
  FOREIGN KEY (habilidad) REFERENCES habilidad(id_habilidad)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Table for `deck`

CREATE TABLE deck (
	id_deck INT NOT NULL AUTO_INCREMENT,
    cantidad_cartas INT,
    
    PRIMARY KEY (id_deck)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Table for `deck de jugador`

CREATE TABLE deck_jugador (

  id_deck INT,
  id_carta INT,
  id_jugador INT,
  fecha_de_creacion INT,
  fecha_modificacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  
  FOREIGN KEY (id_carta) REFERENCES carta(id_carta),
  FOREIGN KEY (id_deck) REFERENCES deck(id_deck)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Table for `jugador`

CREATE TABLE jugador(

	id_jugador INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(255),
	partidas_ganadas INT,
    partidas_perdidas INT,
	fecha_creacion INT,
	fecha_modificacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    PRIMARY KEY (id_jugador)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Table for `credenciales`

CREATE TABLE credenciales(
	id_credencial VARCHAR(255),
    id_jugador INT,
    nombre VARCHAR(255),
    contraseña VARCHAR(255),
    
    FOREIGN KEY (id_jugador) REFERENCES jugador(id_jugador)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Table for `partida`

CREATE TABLE partida(
    id_partida INT,
    id_jugador INT,
    id_ganador INT,
    id_perdedor INT,
    cantidad_turnos INT,
    
    FOREIGN KEY (id_jugador) REFERENCES jugador(id_jugador)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;






