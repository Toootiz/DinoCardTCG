USE dinocard_db;

INSERT INTO habilidad (descripcion) VALUES 
(' '),
('Habilidad 2'),
('Habilidad 3'),
('Habilidad 4'),
('Habilidad 5'),
('Habilidad 6'),
('Habilidad 7'),
('Habilidad 8');

INSERT INTO carta (Nombre, Puntos_de_Vida, Puntos_de_ataque, Coste_en_elixir, Habilidad) VALUES
('T-Rex', 11, 6, 7, 2),
('Compy', 1, 1, 1, 1),
('Spinosaurus', 6, 4, 5, 1),
('Argentavis', 8, 4, 4, 1),
('Brontosaurus', 14, 4, 5, 1),
('Stegosaurus', 10, 3, 6, 3),
('Ankylosaurus', 3, 4, 4, 1),
('Allosaurus', 7, 5, 6, 4),
('Carnotaurus', 6, 4, 4, 1),
('Pteranodon', 6, 4, 3, 1),
('Triceratops', 12, 3, 3, 1),
('Velociraptor', 6, 3, 4, 1),
('Mosasaurus', 12, 4, 5, 1),
('Megalodon', 10, 5, 6, 5),
('Therizinosaurus', 8, 3, 4, 8),
('Giganotosaurus', 12, 8, 7, 1),
('Diplodocus', 10, 2, 4, 1),
('Parasaurolophus', 4, 2, 4, 1),
('Sabertooth', 6, 4, 3, 1),
('Woolly Mammoth', 12, 4, 5, 1),
('Direwolf', 6, 4, 3, 1),
('Baryonyx', 6, 4, 4, 1),
('Dodo', 3, 2, 1, 1),
('Kaprosuchus', 10, 5, 7, 1),
('Oviraptor', 2, 1, 3, 6),
('Giganoto', 14, 7, 7, 1),
('Gallimimus', 4, 2, 3, 7),
('Pulmonoscorpius', 8, 4, 4, 1),
('Archaeopteryx', 2, 1, 3, 1);

INSERT INTO deck (cantidad_cartas) VALUES 
(20);

INSERT INTO jugador (nombre, partidas_ganadas, partidas_perdidas) VALUES 
("Deck1", 2, 3);

INSERT INTO deck_jugador (id_deck, id_carta, id_jugador) VALUES
(1, 1, 1),
(1, 2, 1),
(1, 3, 1),
(1, 4, 1),
(1, 5, 1),
(1, 6, 1),
(1, 7, 1),
(1, 8, 1),
(1, 9, 1),
(1, 10, 1),
(1, 11, 1),
(1, 12, 1),
(1, 13, 1),
(1, 14, 1),
(1, 15, 1),
(1, 16, 1),
(1, 17, 1),
(1, 18, 1),
(1, 19, 1),
(1, 20, 1),
(2, 1, 2),
(2, 2, 2),
(2, 3, 2),
(2, 4, 2),
(2, 5, 2),
(2, 6, 2),
(2, 7, 2),
(2, 8, 2),
(2, 9, 2),
(2, 10, 2),
(2, 11, 2),
(2, 12, 2),
(2, 13, 2),
(2, 14, 2),
(2, 15, 2),
(2, 16, 2),
(2, 17, 2),
(2, 18, 2),
(2, 19, 2),
(2, 20, 2),
(3, 1, 3),
(3, 2, 3),
(3, 3, 3),
(3, 4, 3),
(3, 5, 3),
(3, 6, 3),
(3, 7, 3),
(3, 8, 3),
(3, 9, 3),
(3, 10, 3),
(3, 11, 3),
(3, 12, 3),
(3, 13, 3),
(3, 14, 3),
(3, 15, 3),
(3, 16, 3),
(3, 17, 3),
(3, 18, 3),
(3, 19, 3),
(3, 20, 3);

INSERT INTO credenciales (id_jugador, nombre, contraseña) VALUES 
(1, "Mike", "1234"),
(2, "Pepe", "1234"),
(3, "Rodrigo", "1234"),
(4, "Juan", "1234");

 






