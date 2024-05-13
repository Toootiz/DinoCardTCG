CREATE DATABASE  IF NOT EXISTS `dinocard_db`;
USE `dinocard_db`;

CREATE TABLE `DinoCard` (
  `id` INT AUTO_INCREMENT PRIMARY KEY,
  `Nombre` VARCHAR(255),
  `Puntos_de_Vida` INT,
  `Puntos_de_ataque` INT,
  `Coste_en_elixir` INT,
  `Habilidad` TEXT
);

INSERT INTO `DinoCard` (`Nombre`, `Puntos_de_Vida`, `Puntos_de_ataque`, `Coste_en_elixir`, `Habilidad`) VALUES
('T-Rex', 11, 6, 7, '1'),
('Compy', 1, 1, 1, '0'),
('Spinosaurus', 6, 4, 5, '0'),
('Argentavis', 8, 4, 4, '0'),
('Brontosaurus', 14, 4, 5, '0'),
('Stegosaurus', 10, 3, 6, '2'),
('Ankylosaurus', 3, 4, 4, '0'),
('Allosaurus', 7, 5, 6, '3'),
('Carnotaurus', 6, 4, 4, '0'),
('Pteranodon', 6, 4, 3, '0'),
('Triceratops', 12, 3, 3, '0'),
('Velociraptor', 6, 3, 4, '0'),
('Mosasaurus', 12, 4, 5, '0'),
('Megalodon', 10, 5, 6, '4'),
('Therizinosaurus', 8, 3, 4, '7'),
('Giganotosaurus', 12, 8, 7, '0'),
('Diplodocus', 10, 2, 4, '0'),
('Parasaurolophus', 4, 2, 4, '0'),
('Sabertooth', 6, 4, 3, '0'),
('Woolly Mammoth', 12, 4, 5, '0'),
('Direwolf', 6, 4, 3, '0'),
('Baryonyx', 6, 4, 4, '0'),
('Dodo', 3, 2, 1, '0'),
('Kaprosuchus', 10, 5, 7, '0'),
('Oviraptor', 2, 1, 3, '5'),
('Giganoto', 14, 7, 7, '0'),
('Gallimimus', 4, 2, 3, '6'),
('Pulmonoscorpius', 8, 4, 4, '0'),
('Archaeopteryx', 2, 1, 3, '0');
