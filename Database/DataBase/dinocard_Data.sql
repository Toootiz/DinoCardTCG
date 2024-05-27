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
("Mesc", 2, 3),
("Jose", 2, 1),
("Gatuno7000", 2, 1);

INSERT INTO deck (cantidad_cartas) VALUES
(20),
(20),
(20),
(20);

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
(2, 1, 1),
(2, 2, 1),
(2, 3, 1),
(2, 4, 1),
(2, 5, 1),
(2, 6, 1),
(2, 7, 1),
(2, 8, 1),
(2, 9, 1),
(2, 10, 1),
(2, 11, 1),
(2, 12, 1),
(2, 13, 1),
(2, 14, 1),
(2, 15, 1),
(2, 16, 1),
(2, 17, 1),
(2, 18, 1),
(2, 19, 1),
(2, 20, 1),
(3, 1, 1),
(3, 2, 1),
(3, 3, 1),
(3, 4, 1),
(3, 5, 1),
(3, 6, 1),
(3, 7, 1),
(3, 8, 1),
(3, 9, 1),
(3, 10, 1),
(3, 11, 1),
(3, 12, 1),
(3, 13, 1),
(3, 14, 1),
(3, 15, 1),
(3, 16, 1),
(3, 17, 1),
(3, 18, 1),
(3, 19, 1),
(3, 20, 1),
(4, 1, 2),
(4, 2, 2),
(4, 3, 2),
(4, 4, 2),
(4, 5, 2),
(4, 6, 2),
(4, 7, 2),
(4, 8, 2),
(4, 9, 2),
(4, 10, 2),
(4, 11, 2),
(4, 12, 2),
(4, 13, 2),
(4, 14, 2),
(4, 15, 2),
(4, 16, 2),
(4, 17, 2),
(4, 18, 2),
(4, 19, 2),
(4, 20, 2);

INSERT INTO credenciales (id_jugador, contraseña) VALUES 
(1, "1234"),
(2, "1234"),
(3, "1234");


CREATE VIEW carta_habilidad_detalle AS
SELECT 
    c.Nombre,
    c.Puntos_de_Vida,
    c.Puntos_de_ataque,
    c.Coste_en_elixir,
    h.descripcion,
    h.id_habilidad,
    hd.venenodmg,
    hd.quemadodmg,
    hd.sangradodmg,
    hd.mordidadmg,
    hd.colatazodmg,
    hd.boostvida,
    hd.boostataquedmg,
    hd.boostcosto,
    hd.duracion
FROM 
    carta c
JOIN 
    habilidad h ON c.Habilidad = h.id_habilidad
JOIN 
    habilidadData hd ON h.id_habilidad = hd.id_habilidad;
    
SELECT * FROM carta_habilidad_detalle;


 






