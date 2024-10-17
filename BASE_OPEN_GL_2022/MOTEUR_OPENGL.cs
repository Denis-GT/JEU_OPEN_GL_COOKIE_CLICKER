
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using OpenGLDotNet;


namespace BASE_OPEN_GL
{
  partial class Program
  {

    // NE PAS TOUCHER CE FICHIER


    static float[] Materiel_Speculaire = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    static float[] Materiel_Brillant = new float[] { 50.0f };
    static float[] Position_Lumiere = new float[] { -1.0f, 1.0f, 1.0f, 0.0f };

    static void Initialisation_3D()
    {
      int[] argc = new int[1] { 0 };
      string[] argv = new string[1] { "BaseOpenGl_2022" };
      FG.Init(argc, argv);

      FG.InitDisplayMode(FG.GLUT_DOUBLE | FG.GLUT_RGB | FG.GLUT_DEPTH);
      FG.InitWindowSize(800, 600);
      FG.CreateWindow("BASE OPENGL 2022 - JLV - SIO");

      GL.Init(true);
      IL.Init();
      ILU.Init();
      ILUT.Init();
      ILUT.Renderer(ILUT.ILUT_OPENGL);

      GL.Enable(GL.GL_LIGHTING);
      GL.Enable(GL.GL_LIGHT0);
      GL.Enable(GL.GL_COLOR_MATERIAL);
      GL.ShadeModel(GL.GL_SMOOTH);     
      GL.ClearDepth(1.0);                                        
      GL.Enable(GL.GL_DEPTH_TEST);                              
      GL.DepthFunc(GL.GL_LESS);                                   

      //GL.Materialfv(GL.GL_FRONT, GL.GL_SPECULAR, Materiel_Speculaire);
      //GL.Materialfv(GL.GL_FRONT, GL.GL_SHININESS, Materiel_Brillant);
      //GL.Lightfv(GL.GL_LIGHT0, GL.GL_POSITION, Position_Lumiere);

      WGL.SwapIntervalEXT(1);
    }
    //------------------------------------------------

    static void On_Changement_Taille_Fenetre(int P_Largeur, int P_Hauteur)
    {
      GL.MatrixMode(GL.GL_PROJECTION);
      GL.LoadIdentity();
      GL.Viewport(0, 0, P_Largeur, P_Hauteur);
      float L_Rapport_Largeur_Hauteur = (float)P_Largeur / (float)P_Hauteur;
      GLU.Perspective(60.0, L_Rapport_Largeur_Hauteur, 1.5, 100.0);    
    }
    //------------------------------------------------

    static void On_Afficher_Scene()
    {
      //.... DEBUT DE NE PAS TOUCHER
      GL.Clear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);   // Effacer les buffer d'affichage, de profondeur et de masque
      GL.MatrixMode(GL.GL_MODELVIEW);  // choisir la matrice de vue
      GL.LoadIdentity(); // initialiser la matrice vue en matrice identité Le repere est donc en 0,0,0
      GLU.LookAt(0.0, 0.0, 20.0, // La caméra est à  0,0,20 (x y z)
                  0.0, 0.0, 0.0,   // regarde 0,0,0 (le centre)
                  0.0, 1.0, 0.0);  // vecteur orientation  (vers le haut)

      Afficher_Ma_Scene();
      FG.SwapBuffers();
    }
    //------------------------------------------------

    static void OPENGL_Affiche_Chaine(float P_X, float P_Y, string P_Chaine, 
      uint P_Police = FG.GLUT_BITMAP_TIMES_ROMAN_24)
    {
      GL.Disable(GL.GL_LIGHTING);
      GL.RasterPos2f(P_X, P_Y);
      int Nombre_Caracteres = P_Chaine.Length;
      for (int Index_Caractere = 0; Index_Caractere < Nombre_Caracteres; Index_Caractere++) {
        FG.BitmapCharacter(P_Police, P_Chaine[Index_Caractere]);
      }
      GL.Enable(GL.GL_LIGHTING);
    }
    //----------------------------------------------------

    static void OPENGL_Active_Reflexion()
    {    
      GL.Materialfv(GL.GL_FRONT, GL.GL_SPECULAR, Materiel_Speculaire);
      GL.Materialfv(GL.GL_FRONT, GL.GL_SHININESS, Materiel_Brillant);
      GL.Lightfv(GL.GL_LIGHT0, GL.GL_POSITION, Position_Lumiere);
    }

    //------------------------------------

    static uint OPENGL_Charge_Texture(string P_Nom, bool P_Miroir_X = false, bool P_Miroir_Y = false)
    {
      uint Numero_Image = IL.GenImage();
      IL.BindImage(Numero_Image);
      bool OK = IL.LoadImage(P_Nom);

      if (OK) {
        uint[] Code_Texture = new uint[1];
        GL.GenTextures(1, Code_Texture);
        if (P_Miroir_X) ILU.Mirror();
        if (P_Miroir_Y) ILU.FlipImage();
        int Format_Image = IL.GetInteger(IL.IL_IMAGE_FORMAT);
        GL.Enable(GL.GL_TEXTURE_2D);
        GL.BindTexture(GL.GL_TEXTURE_2D, Code_Texture[0]);
        GL.TexImage2D(GL.GL_TEXTURE_2D, 0, (int)Format_Image, IL.GetInteger(IL.IL_IMAGE_WIDTH), IL.GetInteger(IL.IL_IMAGE_HEIGHT), 0,
                                       (uint)Format_Image, (uint)IL.GetInteger(IL.IL_IMAGE_TYPE), IL.GetData());
        GL.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, (int)GL.GL_CLAMP_TO_EDGE);
        GL.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, (int)GL.GL_CLAMP_TO_EDGE);
        GL.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_R, (int)GL.GL_CLAMP_TO_EDGE);
        GL.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
        GL.TexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
        GL.TexEnvf(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, GL.GL_REPLACE);
        IL.DeleteImage(Numero_Image);
        GL.Disable(GL.GL_TEXTURE_2D);
        return Code_Texture[0];
      }
      return 0;
    }
  }
}
