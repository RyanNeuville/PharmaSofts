-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : mar. 11 nov. 2025 à 03:12
-- Version du serveur : 8.2.0
-- Version de PHP : 8.2.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Creation de la Base de données : `pharmasoft_db`
--

CREATE DATABASE IF NOT EXISTS `pharmasoft_db`;

-- --------------------------------------------------------

--
-- Structure de la table `fournisseurs`
--

DROP TABLE IF EXISTS `fournisseurs`;
CREATE TABLE IF NOT EXISTS `fournisseurs` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nom` varchar(25) NOT NULL,
  `email` varchar(25) NOT NULL,
  `telephone` varchar(20) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Structure de la table `medicaments`
--

DROP TABLE IF EXISTS `medicaments`;
CREATE TABLE IF NOT EXISTS `medicaments` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nom` varchar(100) NOT NULL,
  `prix` decimal(10,2) NOT NULL,
  `quantite` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `medicaments`
--

INSERT INTO `medicaments` (`id`, `nom`, `prix`, `quantite`) VALUES
(2, 'paracetamol', 500.00, 25),
(3, 'efferaglan', 1000.00, 48),
(4, 'metronidazole', 500.00, 30);

-- --------------------------------------------------------

--
-- Structure de la table `pharmaciens`
--

DROP TABLE IF EXISTS `pharmaciens`;
CREATE TABLE IF NOT EXISTS `pharmaciens` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nom` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `telephone` varchar(20) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `pharmaciens`
--

INSERT INTO `pharmaciens` (`id`, `nom`, `email`, `telephone`) VALUES
(5, 'mounir', 'p@gmail.com', '656152872');

-- --------------------------------------------------------

--
-- Structure de la table `utilisateurs`
--

DROP TABLE IF EXISTS `utilisateurs`;
CREATE TABLE IF NOT EXISTS `utilisateurs` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nom` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `mot_de_passe` varchar(255) DEFAULT NULL,
  `role` varchar(50) DEFAULT NULL,
  `reference_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `utilisateurs`
--

INSERT INTO `utilisateurs` (`id`, `nom`, `email`, `mot_de_passe`, `role`, `reference_id`) VALUES
(1, 'mounir', 'p@gmail.com', '123456', 'pharmacien', 5);

-- --------------------------------------------------------

--
-- Structure de la table `ventes`
--

DROP TABLE IF EXISTS `ventes`;
CREATE TABLE IF NOT EXISTS `ventes` (
  `id` int NOT NULL AUTO_INCREMENT,
  `produit` varchar(255) NOT NULL,
  `quantite` int NOT NULL,
  `prix_unitaire` decimal(10,2) NOT NULL,
  `sous_total` decimal(10,2) NOT NULL,
  `date_vente` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `ventes`
--

INSERT INTO `ventes` (`id`, `produit`, `quantite`, `prix_unitaire`, `sous_total`, `date_vente`) VALUES
(1, 'Paracétamol', 1, 500.00, 500.00, '2025-11-10 21:01:18'),
(2, 'Vitamine C', 1, 300.00, 300.00, '2025-11-10 21:04:34'),
(3, 'Amoxicilline', 1, 1200.00, 1200.00, '2025-11-11 02:48:44'),
(4, 'efferaglan', 2, 1000.00, 2000.00, '2025-11-11 03:02:42');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
