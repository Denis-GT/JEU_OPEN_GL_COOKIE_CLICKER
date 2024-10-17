using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenGLDotNet;

namespace BASE_OPEN_GL
{
	partial class Program
	{
    static FG.TCALLBACKglutDisplayProc F_Affichage;
    static FG.TCALLBACKglutIdleProc F_Animation_Scene;
    static FG.TCALLBACKglutReshapeProc F_Changement_Taille_Fenetre;
    static FG.TCALLBACKglutKeyboardProc F_Gestion_Clavier;
    static FG.TCALLBACKglutMouseProc F_Gestion_Bouton_Souris;
    static FG.TCALLBACKglutMotionProc F_Gestion_Souris_Clique;
    static FG.TCALLBACKglutPassiveMotionProc F_Gestion_Souris_Libre;
    static FG.TCALLBACKglutKeyboardUpProc F_Gestion_Clavier_Relache;
    static FG.TCALLBACKglutSpecialProc F_Gestion_Touches_Speciales;
    static FG.TCALLBACKglutMouseWheelProc F_Gestion_Molette;
		//---------------------------

    static void Main()
		{
      F_Affichage = delegate () { On_Afficher_Scene(); };
      F_Animation_Scene = delegate () { Animation_Scene(); };
      F_Changement_Taille_Fenetre = delegate (int P_Largeur, int P_Hauteur) { On_Changement_Taille_Fenetre(P_Largeur, P_Hauteur); };
      F_Gestion_Clavier = delegate (byte P_Touche, int P_X, int P_Y) { Gestion_Clavier(P_Touche, P_X, P_Y); };
      F_Gestion_Bouton_Souris = delegate (int P_Bouton, int P_Etat, int P_X, int P_Y) { Gestion_Bouton_Souris(P_Bouton, P_Etat, P_X, P_Y); };
      F_Gestion_Souris_Clique = delegate (int P_X, int P_Y) { Gestion_Souris_Clique(P_X, P_Y); };
      F_Gestion_Souris_Libre = delegate (int P_X, int P_Y) { Gestion_Souris_Libre(P_X, P_Y); };
      F_Gestion_Clavier_Relache = delegate (byte P_Touche, int P_X, int P_Y) { Gestion_Clavier_Relache(P_Touche, P_X, P_Y); };
      F_Gestion_Touches_Speciales = delegate (int P_Touche, int P_X, int P_Y) { Gestion_Touches_Speciales(P_Touche, P_X, P_Y); };
      F_Gestion_Molette = delegate (int P_Molette, int P_Sens, int P_X, int P_Y) { Gestion_Molette(P_Molette, P_Sens, P_X, P_Y); };

      Initialisation_3D();
      FG.ReshapeFunc(F_Changement_Taille_Fenetre);
      FG.DisplayFunc(F_Affichage);
      FG.KeyboardFunc(F_Gestion_Clavier);
      FG.KeyboardUpFunc(F_Gestion_Clavier_Relache);
      FG.SpecialFunc(F_Gestion_Touches_Speciales);
      FG.IdleFunc(F_Animation_Scene);
      FG.MouseFunc(F_Gestion_Bouton_Souris);
      FG.MouseWheelFunc(F_Gestion_Molette);
      FG.PassiveMotionFunc(F_Gestion_Souris_Libre);
      FG.MotionFunc(F_Gestion_Souris_Clique);

      Initialisation_Animation();
			FG.MainLoop();
		}
	}
}
 