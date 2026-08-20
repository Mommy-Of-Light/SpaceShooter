# Cahier des charges

## 1. Présentation

**Nom du projet :**
Space Shooter

**Description du projet :**
Un jeu dans lequel le joueur contrôle un petit vaisseau spatial et doit combattre des hordes d’ennemis venus dans le but de le détruire.

**Objectif du projet :**
Le projet s’inspire du célèbre jeu *Space Invaders* de 1978. L’objectif est de recréer le principe du jeu tout en ajoutant des fonctionnalités supplémentaires améliorant l’expérience de jeu, comme des sauvegardes, des bonus, différentes difficultés et un système de classement.

## 2. Besoins

**Que doit permettre le projet ?**

* Amener de la joie, de la nostalgie et de l’adrénaline.
* Amener un esprit de compétition sans haine.
* Permettre au joueur de jouer seul.
* Permettre au joueur d’améliorer son score.
* Permettre au joueur de recommencer des parties.
* Proposer différentes difficultés afin de varier l’expérience de jeu.

## 3. Fonctionnalités

### Fonctionnalité 1

**Nom :** Jeu principal

**Description :**
Le joueur peut déplacer son vaisseau vers la droite et la gauche et tirer sur les ennemis. Des vagues d’ennemis apparaissent progressivement. Le joueur gagne des points en détruisant les ennemis. La difficulté et la progression évoluent au cours de la partie.

### Fonctionnalité 2

**Nom :** Menu

**Description :**
Le menu permet de commencer une partie, sélectionner la difficulté, accéder aux sauvegardes, consulter l’archive des parties terminées et consulter le classement.

### Fonctionnalité 3

**Nom :** Sauvegarde et archive

**Description :**
Le joueur peut sauvegarder une partie commencée afin de pouvoir la reprendre ultérieurement. Les parties terminées peuvent être conservées dans une archive.

### Fonctionnalité 4

**Nom :** Classement

**Description :**
Le jeu conserve les meilleurs scores du joueur afin de lui permettre de comparer ses performances et d'essayer d'améliorer son classement.

### Fonctionnalité 5

**Nom :** Difficultés et bonus

**Description :**
Le jeu propose différentes difficultés. Des bonus peuvent également apparaître pendant une partie afin de modifier temporairement ou durablement les possibilités du joueur.

## 4. Utilisateurs

**Qui va utiliser le projet ?**

Des amateurs de jeux vidéo.

**Que peuvent-ils faire ?**

* Jouer.
* S'affronter dans des compétitions.
* S'amuser.
* Améliorer leur score.
* Choisir une difficulté.
* Sauvegarder une partie.
* Consulter leurs anciennes parties.
* Consulter le classement.

## 5. Interface

**À quoi doit ressembler l'interface ?**

L'interface doit s'inspirer du style de *Space Invaders*, avec un menu d'accueil, une interface de jeu, un classement et une archive des parties.

**Écrans nécessaires :**

* Menu.
* Jeu.
* Classement.
* Parties sauvegardées.
* Archives.
* Écran de fin de partie.

## 6. Contraintes

**Contraintes techniques :**

* Le jeu doit être développé pour ordinateur.
* Le jeu doit être réalisé en C# avec MonoGame.
* Les données nécessitant une sauvegarde doivent pouvoir être enregistrées.
* Le jeu doit rester jouable et compréhensible pour l'utilisateur.
* Le projet doit être réalisé dans le temps prévu pour le projet de fin d'études.

**Contraintes de temps :**

Le projet doit être terminé pour la date de remise définie dans le calendrier du projet.

**Autres contraintes :**

Les fonctionnalités supplémentaires pourront être adaptées ou retirées en fonction du temps disponible.

## 7. Technologies

**Langage(s) :**

* C#
* SQL
* XML

**Logiciel(s) / outil(s) :**

* Visual Studio Community
* MonoGame
* MariaDB
* Visual Studio Code
* GitHub
* Google Drive

**Autres :**

Les technologies pourront être adaptées si une contrainte technique apparaît durant le développement.

## 8. Tests

**Comment vérifier que le projet fonctionne correctement ?**

* Vérifier que le joueur peut se déplacer.
* Vérifier que le joueur peut tirer.
* Vérifier que les ennemis apparaissent correctement.
* Vérifier les collisions entre les tirs et les ennemis.
* Vérifier le fonctionnement du score.
* Vérifier la progression des vagues.
* Vérifier les différentes difficultés.
* Vérifier le fonctionnement des bonus.
* Vérifier la sauvegarde d'une partie.
* Vérifier le chargement d'une partie sauvegardée.
* Vérifier l'archivage des parties terminées.
* Vérifier l'affichage du classement.
* Vérifier le fonctionnement des différents écrans du jeu.

## 9. Résultat attendu

**À quoi doit ressembler le projet terminé ?**

Le projet terminé doit être un jeu desktop fonctionnel inspiré de *Space Invaders*.

Le joueur doit pouvoir contrôler son vaisseau, tirer sur les ennemis, combattre différentes vagues et obtenir un score.

Le jeu doit également proposer les fonctionnalités prévues dans le cahier des charges, notamment les différentes difficultés, les sauvegardes, les bonus, l'archive des parties et le classement.

L'application doit être suffisamment stable pour être présentée et utilisée dans le cadre de l'examen.

## 10. Remarques

Certaines fonctionnalités peuvent être adaptées pendant le développement en fonction des contraintes techniques et du temps disponible.

Les fonctionnalités principales du jeu sont prioritaires. Les fonctionnalités supplémentaires pourront être réalisées si le temps restant le permet.
