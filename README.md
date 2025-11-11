# 💊 Application de Gestion de Pharmacie (PharmaSofts)

Un système complet pour la gestion informatisée des opérations d'une pharmacie, incluant le suivi des médicaments, la gestion des stocks, la tenue des ventes et l'administration des utilisateurs.

## ✨ Fonctionnalités Clés

Ce projet offre une interface utilisateur pour différents rôles et permet les actions suivantes :

* **Interface de Connexion :** Accès sécurisé basé sur le **Nom d'utilisateur** et le **Mot de passe**.
* **Tableau de Bord Administrateur :** Vue d'ensemble des **statistiques** (total médicaments, ruptures, péremptions proches) et accès aux modules de gestion.
* **Gestion des Utilisateurs :** **Ajout, modification, suppression** des comptes (Admin / Pharmacien).
* **Gestion des Médicaments :** **Ajout, modification, suppression** et consultation des détails (Nom, Catégorie, Prix, Quantité, Date de péremption).
* **Gestion du Stock :** Consultation du **stock actuel** et affichage des **alertes automatiques** (rupture de stock, médicaments expirés).
* **Historique des Ventes (Admin) :** Consultation de toutes les ventes, avec possibilité de **recherche**.
* **Tableau de Bord Pharmacien :** Vue des **ventes du jour** et accès rapide à la **saisie de nouvelles ventes**.
* **Enregistrement des Ventes :** Interface dédiée à la **saisie rapide des transactions**, calcul du montant total.
* **Historique Personnel des Ventes (Pharmacien) :** Consultation des ventes enregistrées par l'utilisateur.

## 🛠️ Stack Technique

Le projet est développé en utilisant les technologies suivantes pour garantir une application de bureau robuste avec une base de données centralisée :

| Composant | Technologie | Rôle |
| :--- | :--- | :--- |
| **Interface Utilisateur / Logique Applicative** | **VB.NET (Visual Basic .NET)** | Développement de l'application de bureau (Windows Forms) |
| **Serveur Web / Base de Données** | **XAMPP** | Environnement de développement intégrant le serveur web (Apache) et la gestion de la base de données. |
| **Base de Données** | **MySQL** | Système de gestion de base de données relationnelle pour stocker toutes les informations (médicaments, utilisateurs, ventes, etc.). |
