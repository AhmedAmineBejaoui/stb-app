USE master;
IF DB_ID('stb') IS NULL
BEGIN
    CREATE DATABASE stb;
END
GO

USE stb;
GO

DROP TABLE IF EXISTS notifications;
DROP TABLE IF EXISTS demandes;
DROP TABLE IF EXISTS users;
GO

CREATE TABLE users (
    id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(100) NOT NULL,
    userfirstname NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL,
    mobile VARCHAR(30) NOT NULL,
    password NVARCHAR(200) NOT NULL,
    role NVARCHAR(50) DEFAULT 'User'
);
GO

CREATE TABLE demandes (
    id INT PRIMARY KEY IDENTITY(1,1),
    request_type NVARCHAR(100),
    request_sub_type NVARCHAR(100),
    description NVARCHAR(MAX),
    creation_date DATETIME DEFAULT GETDATE(),
    status NVARCHAR(50) NOT NULL DEFAULT 'En attente',
    user_id INT FOREIGN KEY REFERENCES users(id)
);
GO

CREATE TABLE notifications (
    id INT PRIMARY KEY IDENTITY(1,1),
    message NVARCHAR(500) NOT NULL,
    creation_date DATETIME DEFAULT GETDATE(),
    is_read BIT DEFAULT 0,
    user_id INT FOREIGN KEY REFERENCES users(id),
    demande_id INT FOREIGN KEY REFERENCES demandes(id)
);
GO

INSERT INTO users (username, userfirstname, email, mobile, password, role)
VALUES 
(N'admin', N'Admin', N'admin@stb.local', N'0000000000', N'admin123', N'Admin'),
(N'malek', N'Malek', N'malek@stb.local', N'1111111111', N'azerty123', N'User');
GO

INSERT INTO demandes (request_type, request_sub_type, description, status, user_id)
VALUES 
(N'Support technique', N'Connexion réseau', N'Problème de connexion à internet', N'En attente', 2),
(N'Accès', N'VPN', N'Demande d''accès au VPN', N'En attente', 2),
(N'Matériel', N'Ordinateur', N'Demande de remplacement de PC', N'Approuvée', 2);
GO

INSERT INTO notifications (message, user_id, demande_id)
VALUES 
(N'Nouvelle demande créée', 2, 1),
(N'Votre demande a été approuvée', 2, 3);
GO

SELECT * FROM users;
SELECT * FROM demandes;
SELECT * FROM notifications;

