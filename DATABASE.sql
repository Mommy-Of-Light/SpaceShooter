DROP DATABASE IF EXISTS SpaceShooter;
CREATE DATABASE SpaceShooter;

CREATE USER IF NOT EXISTS 'spaceshooter_app'@'localhost'
IDENTIFIED BY 'no-security';

GRANT SELECT, INSERT, UPDATE, DELETE
ON SpaceShooter.*
TO 'spaceshooter_app'@'localhost';

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