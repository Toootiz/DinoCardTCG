USE dinocard_db;

INSERT INTO habilidad (descripcion) VALUES 
(''),
('Da un mordisco que causa sangrado y daño extra'),
('Pega una mordida fuerte'),
('Pega fuertemente con su cola'),
('Lanza una llamarada de fuego'),
('Hace un rugido el cual aumenta el ataque de un dinosaurio'),
('Muerde y causa sangrado'),
('Disminuye el soto de una carta'),
('Aumente la vida de una carta');

INSERT INTO habilidadData (id_habilidad, venenodmg, quemadodmg, sangradodmg, mordidadmg, colatazodmg, boostvida, boostataquedmg, boostcosto, duracion) VALUES
(5, 0, 3, 0, 0, 0, 0, 0, 0, 3),
(1, 0, 0, 0, 0, 0, 0, 0, 0, 0),
(2, 0, 0, 1, 3, 0, 0, 0, 0, 2),
(3, 0, 0, 0, 3, 0, 0, 0, 0, 1),
(4, 0, 0, 0, 0, 3, 0, 0, 0, 0),
(6, 0, 0, 0, 0, 0, 0, 2, 0, 2),
(7, 0, 0, 2, 0, 0, 0, 0, 0, 3),
(8, 0, 0, 0, 0, 0, 0, 0, 2, 5),
(9, 0, 0, 0, 0, 0, 4, 0, 0, 0);


INSERT INTO carta (Nombre, Puntos_de_Vida, Puntos_de_ataque, Coste_en_elixir, Habilidad) VALUES
('T-Rex', 11, 6, 7, 2),
('Compy', 1, 1, 1, 1),
('Spinosaurus', 6, 4, 5, 3),
('Argentavis', 8, 4, 4, 1),
('Brontosaurus', 14, 4, 5, 1),
('Stegosaurus', 10, 3, 6, 4),
('Ankylosaurus', 3, 4, 4, 1),
('Allosaurus', 7, 5, 6, 6),
('Carnotaurus', 6, 4, 4, 1),
('Pteranodon', 6, 4, 3, 1),
('Triceratops', 12, 3, 3, 1),
('Velociraptor', 6, 3, 4, 1),
('Mosasaurus', 12, 4, 5, 1),
('Megalodon', 10, 5, 6, 7),
('Therizinosaurus', 8, 3, 4, 8),
('Giganotosaurus', 12, 8, 7, 1),
('Diplodocus', 10, 2, 4, 1),
('Parasaurolophus', 4, 2, 4, 1),
('Sabertooth', 6, 4, 3, 9),
('Woolly Mammoth', 12, 4, 5, 1),
('Direwolf', 6, 4, 3, 1),
('Baryonyx', 6, 4, 4, 1),
('Dodo', 3, 2, 1, 1),
('Kaprosuchus', 10, 5, 7, 1),
('Oviraptor', 2, 1, 3, 1),
('Giganoto', 14, 7, 7, 1),
('Gallimimus', 4, 2, 3, 7),
('Pulmonoscorpius', 8, 4, 4, 1),
('Archaeopteryx', 2, 1, 3, 1),
('Wyvern', 5, 6, 7, 5);


INSERT INTO jugador (nombre, partidas_ganadas, partidas_perdidas) VALUES 
("Mesc", 42, 14),
("Jose", 31, 57),
("Gatuno7000", 147, 56);


INSERT INTO deck (id_jugador, nombre_deck, descripcion_deck, id_carta1, id_carta2, id_carta3, id_carta4, id_carta5, id_carta6, id_carta7, id_carta8, id_carta9, id_carta10) VALUES
(1,"Deck Chispa", "Para jugar de chill", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
(2, "Deck Uno", "Para jugar mucho", 1, 2, 4, 6, 8, 10, 12, 14, 16, 18),
(3,"Deck Dos", "Para jugar rapido", 2, 3, 4, 5, 8, 10, 15, 18, 20, 21),
(1,"Deck Tres", "Para jugar feliz", 10, 11, 12, 13, 14, 15, 16, 17, 18, 19);


select * from deck;