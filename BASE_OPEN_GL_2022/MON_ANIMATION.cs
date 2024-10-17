using System;
using System.IO;
using Microsoft.SqlServer.Server;

using OpenGLDotNet;


/*
 * 
 * 
 * Exemple de base d'une application OPENGL. Cet exemple est destiné au SIO 1B CCI de nimes. 
 * C'est une base pour développer un ensemble de projets - session 2022
 */
namespace BASE_OPEN_GL
{
  partial class Program
  {

    // vous pouvez mettre vos variables globales ici
    // vous pouvez mettre vos fonctions (méthodes statiques) ici

    static double Y = 0;
    static double X = 0;
    static double Angle_Axe_X = 1;
    static double Vitesse_Rotation = 0.1;
    static double Taille_Sphere = 2;
    static int Qualite_Decor = 30;
    static bool Souris_Sur_Sphere;
    static double Score = 0;
    static double Palier = 0;
    static bool Espace = false;
    static double Ancien_Score = 1;
    static string Description_Qualite_Decor = "moyen";

    //==========================================================
    // Cette fonction est invoquée qu'une seule fois avant que le moteur OpenGl travaille.
    // elle est utile pour initialiser des éléments globaux à l'application
    static void Initialisation_Animation()
    {
      OPENGL_Active_Reflexion();
      FG.FullScreen();
    }

    //==========================================================
    // Cette fonction est invoquée par le Moteur de manière périodique pour Afficher une Frame
    static void Afficher_Ma_Scene()
    {
      OPENGL_Affiche_Chaine(-1, 7, "Score : " + Score, 5);
      OPENGL_Affiche_Chaine(12, 8, "Qualité du décor : " + Description_Qualite_Decor + " : " + Qualite_Decor, 8);
      OPENGL_Affiche_Chaine(-20, 8, "Commandes : ", 5);
      OPENGL_Affiche_Chaine(-20, 7, "Clic gauche sur la sphère pour augmenter le score", 5);
      OPENGL_Affiche_Chaine(-20, 6, "Molette souris : Haut : Augmenter qualité du décor", 5);
      OPENGL_Affiche_Chaine(-20, 5, "Molette souris : Bas : Diminuer qualité du décor", 5);
      OPENGL_Affiche_Chaine(-20, 4, "F1 : Mode plein écrant", 5);
      OPENGL_Affiche_Chaine(-20, 3, "F2 : Quitter le mode plien écrant", 5);
      OPENGL_Affiche_Chaine(-20, 2, "Espace : réinitialiser la partie", 5);

      if (Qualite_Decor < 6 || Qualite_Decor == 6) {
        Description_Qualite_Decor = "Minimum";
        Qualite_Decor = 6;
      }
      if (Qualite_Decor < 10 && Qualite_Decor > 6) {
        Description_Qualite_Decor = "Faible";
      }
      if (Qualite_Decor < 30 && Qualite_Decor > 9) {
        Description_Qualite_Decor = "Moyen";
      }
      if (Qualite_Decor < 50 && Qualite_Decor > 29) {
        Description_Qualite_Decor = "Élevé";
      }
      if (Qualite_Decor < 80 && Qualite_Decor > 49) {
        Description_Qualite_Decor = "Très Élevé";
      }
      if (Qualite_Decor < 100 && Qualite_Decor > 79) {
        Description_Qualite_Decor = "Ultra";
      }
      if (Qualite_Decor > 100 || Qualite_Decor == 100) {
        Description_Qualite_Decor = "Maximum";
        Qualite_Decor = 100;
      }

      GL.Translated(X, Y, 0);
      FG.SolidSphere(Taille_Sphere, Qualite_Decor, 13);

      if (Score > Ancien_Score) {
        Score--;
        Ancien_Score = Score + 1;
        GL.Color3b(0, 100, 0);
      }

      if (Score > 5) {
        GL.Translated(X, Y, 0);
        FG.SolidTorus(0.75, 3, 100, Qualite_Decor);
        GL.Color3b(0, 0, 100);
      }

      if (Score > 10) {
        GL.Color3b(0, 50, 100);
        GL.Rotated(Angle_Axe_X, 0, 1, 0);
        Angle_Axe_X = Angle_Axe_X + Vitesse_Rotation;
        Angle_Axe_X = Angle_Axe_X % 360.0;
      }

      if (Score > 20) {
        GL.Color3b(0, 100, 100);
        GL.Translated(X, Y, 0);
        FG.SolidTorus(0.75, 5, 100, Qualite_Decor);
      }

      if (Score > 25) {
        GL.Color3b(0, 100, 50);
      }

      if (Score > 50) {
        GL.Color3b(50, 0, 100);
      }

      if (Score > 100) {
        GL.Color3b(100, 100, 0);
      }

      if (Score > Palier + 5) {
        Palier = Score;
        Vitesse_Rotation = Vitesse_Rotation + 0.5;
      }

      if (Espace == true) {
        Score = 0;
        Espace = false;
        GL.Color3b(100, 100, 100);
      }
    }



    //=========================================================
    // cette fonction est invoquée en boucle par openGl.
    // Peut être utilisée pour modifier des variables globales utilisée dans "Afficher_Ma_Scene"
    static void Animation_Scene()
    {

      //fortement recommandé
      FG.PostRedisplay(); // Pour demander de réafficher une Frame afin de tenir compte des modifications
    }

    //======================================================================
    // cette fonction est invoquée par OpenGl lorsqu'on appuie sur une touche spéciale (flèches, Fx, ...)
    // P_Touche contient le code de la touche, P_X et P_Y contiennent les coordonnées de la souris quand on appuie sur une touche
    static void Gestion_Touches_Speciales(int P_Touche, int P_X, int P_Y)
    {
      if (P_Touche == FG.GLUT_KEY_F1) FG.FullScreen();
      if (P_Touche == FG.GLUT_KEY_F2) FG.LeaveFullScreen();


      //fortement recommandé
      FG.PostRedisplay(); // Pour demander de réafficher une Frame afin de tenir compte des modifications
    }

    //======================================================================
    // cette fonction est invoquée par OpenGl lorsqu'on appuie sur une touche normale (A,Z,E, ...)
    // P_Touche contient le code de la touche, P_X et P_Y contiennent les coordonnées de la souris quand on appuie sur une touche
    static void Gestion_Clavier(byte P_Touche, int P_X, int P_Y)
    {
      // 27 est le code de la touche "Echap"
      if (P_Touche == 27) FG.LeaveMainLoop();
      if (P_Touche == 32) Espace = true;

      //fortement recommandé
      FG.PostRedisplay(); // Pour demander de réafficher une Frame afin de tenir compte des modifications
    }

    //======================================================================
    // cette fonction est invoquée par OpenGl lorsqu'on relache une touche normale (A,Z,E, ...)
    // P_Touche contient le code de la touche, P_X et P_Y contiennent les coordonnées de la souris quand on relache sur une touche
    static void Gestion_Clavier_Relache(byte P_Touche, int P_X, int P_Y)
    {


      // FG.PostRedisplay(); // Pour demander de réafficher une Frame afin de tenir compte des modifications
    }

    //==================================================================================
    // cette fonction est invoquée par OpenGl lorsqu'on appuie sur un bouton de la souris
    // P_Bouton contient le code du bouton (gauche ou droite), P_Etat son etat, les coordonnées de la souris quand on appuie sur un bouton sont dans P_X et P_Y

    static void Gestion_Bouton_Souris(int P_Bouton, int P_Etat, int P_X, int P_Y)
    {
      if (P_Bouton == 0 && Souris_Sur_Sphere == true) Score++;


      //fortement recommandé
      FG.PostRedisplay(); // Pour demander de réafficher une Frame afin de tenir compte des modifications
    }

    //====================================================================
    // cette fonction est invoquée par OpenGl lorsqu'on tourne la molette de la souris
    // P_Molette contient le code de la molette, P_Sens son sens de rotation, les coordonnées de la souris quand on tourne la molette sont dans P_X et P_Y

    static void Gestion_Molette(int P_Molette, int P_Sens, int P_X, int P_Y)
    {
      if (P_Sens == 1) Qualite_Decor--;
      if (P_Sens == -1) Qualite_Decor++;

      //  FG.PostRedisplay(); // Pour demander de réafficher une Frame afin de tenir compte des modifications
    }

    //====================================================================
    // cette fonction est invoquée par OpenGl lorsqu'on bouge la souris sans appuyer sur un bouton
    // les coordonnées de la souris ont dans P_X et P_Y
    static void Gestion_Souris_Libre(int P_X, int P_Y)
    {
      if ((P_X < 1055 && P_X > 865) && (P_Y < 630 && P_Y > 445)) {
        Souris_Sur_Sphere = true;
      } else {
        Souris_Sur_Sphere = false;
      }
      // FG.PostRedisplay(); // Pour demander de réafficher une Frame afin de tenir compte des modifications
    }


    //====================================================================
    // cette fonction est invoquée par OpenGl lorsqu'on bouge la souris tout en appuyant sur un bouton
    // les coordonnées de la souris ont dans P_X et P_Y
    static void Gestion_Souris_Clique(int P_X, int P_Y)
    {

      // FG.PostRedisplay(); // Pour demander de réafficher une Frame afin de tenir compte des modifications
    }
  }
}
