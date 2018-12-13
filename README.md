# ChatRoom

# DataBase

	CREATE DATABASE `chatroom` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */;
	CREATE TABLE `Message` (
		  `id` int(11) NOT NULL AUTO_INCREMENT,
		  `userid` int(11) DEFAULT NULL,
		  `text` longtext,
		  PRIMARY KEY (`id`),
		  KEY `fk_Message_1_idx` (`userid`),
		  CONSTRAINT `fk_Message_1` FOREIGN KEY (`userid`) REFERENCES `User` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
		) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

	CREATE TABLE `User` (
		  `id` int(11) NOT NULL,
		  `name` varchar(45) NOT NULL,
		  PRIMARY KEY (`id`),
		  UNIQUE KEY `name_UNIQUE` (`name`)
		) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

