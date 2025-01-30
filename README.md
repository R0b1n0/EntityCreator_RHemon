## Package Editeur d'entité

## **EntityEditor tool :**

***Présentation:***
Le tool générateur d’entité permet de modifier et sauvegarder des instances de scriptableObject dérivant de SoEntity. 

***Contenu du package:***
- Class EntityCreatorWindow, affiche le tool 
- SoEntity, hérite de scriptable object
- SoArcher et SoKnight, class héritant de SoEntity, servent d'exemple. 

***Fonctionnement:***
L'utilisateur choisis le scriptable object qu'il souhaite éditer. 
L'éditeur custom affiche les champs privés et permet d'éditer les champs public et Serialized.
Le bouton save entity permet d’enregistrer une instance du scriptable object à l’emplacement souhaité avec les paramètres renseignés. L'utilisateur peut définir de nouveaux scripts héritant de SoEntity, le tool les prendra en compte. 

## **Grid Attribute :**

***Présentation:***
Grid Attribute permet d'afficher les éléments d'une liste sous forme de grille dans l'inspecteur.

***Contenu du package:***
- GridContent  : structure générique composée d’un vector2int, sa valeur représente la position relative de l’élément à l’angle supérieur gauche de la grille. Il contient également un field générique que l’user doit renseigner (l’objet passé doit être serializable).
- GridList : class générique contenant une list de GridContent.
- GridAttribute : PropertyAttribute. Le constructeur prend deux paramètres, le premier pour le nombre de cellules du tableau, le second pour la taille des cellules dans l'inspecteur (en pixel).

***Fonctionnement:***
L'utilisateur définit la taille de la grille (5par défaut) et le type serializable qu'il veut utiliser. 
Dans l'éditeur, la grille s'affiche et l'utilisateur peut sélectionner une case en cliquant dessus, si l'élément rangé dans cet emplacement à déjà une valeur, les propriétés serializable de cet élément saffiche sous la grille et l'user peut modifier ses champs, ssinon, un bouton "Add Element" s'affiche pour ajouter un élément à cet emplacement.
Si l'utilisateur réduit la taille de la grille après avoir ajouté des éléments, les éléments existants tombant hors des limites de la grille seront supprimés.
