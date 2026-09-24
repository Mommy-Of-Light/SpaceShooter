DROP DATABASE IF EXISTS SpaceShooter;
CREATE DATABASE SpaceShooter;

USE SpaceShooter;

CREATE TABLE scores
(
    id INT AUTO_INCREMENT PRIMARY KEY,
    pseudo VARCHAR(50) NOT NULL,
    score INT NOT NULL,
    wave INT NOT NULL,
    difficulty VARCHAR(10) NOT NULL,
    created_at DATETIME NOT NULL
);