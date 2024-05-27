DROP SCHEMA IF EXISTS dinocard_db;
CREATE SCHEMA dinocard_db;
USE dinocard_db;


-- Table forf `habilidad`

CREATE TABLE habilidad (
    id_habilidad INT NOT NULL AUTO_INCREMENT,
    descripcion TEXT,
    PRIMARY KEY (id_habilidad)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE habilidadData (
    id_habilidad INT,
    venenodmg INT,
    quemadodmg INT,
    sangradodmg INT,
    mordidadmg INT,
    colatazodmg INT,
    boostvida INT,
    boostataquedmg INT,
    boostcosto INT,
    duracion INT,
        
    FOREIGN KEY (id_habilidad) REFERENCES habilidad(id_habilidad)
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
  nombre_deck VARCHAR(255),
  descripcion_deck VARCHAR(255),
  fecha_de_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
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
	fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	fecha_modificacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    PRIMARY KEY (id_jugador)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- Table for `credenciales`

CREATE TABLE credenciales(
	id_credencial INT NOT NULL AUTO_INCREMENT,
    id_jugador INT,
    contraseña VARCHAR(255),
    
    PRIMARY KEY (id_credencial),
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






