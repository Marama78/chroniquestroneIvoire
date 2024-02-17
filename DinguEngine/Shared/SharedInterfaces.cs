using Microsoft.Xna.Framework;

/* élève de l'école de jeux vidéos, gamecodeur.fr, depuis 2017,
 * ceci est un extrait d'un code perso, dans le pur esprit viking
 * Faîtes en bon usage. Pas besoin de crédits. Un viking, çà vit de
 * conquêtes et de victoire, brouaaaaaahhhhhh!!!!
 * ---------------------------------------------------------------
 * tu peux le recopier, l'utiliser à des fins perso et commerciales,
 * ou bien le modifier à ta guise.
 * EBB Dan Marama
*/

namespace DinguEngine.Shared
{
    internal interface IInteractiveRect
    {
        public bool isloadedFromXML { get; set; }
        public Rectangle position { get; set; }
        public Rectangle frame { get; set; }
        public Rectangle frameDefault { get; set; }
        public float alpha { get; set; }
        public Color color { get; set; }
        public int textureID { get; set; }
        public int frameSize { get; set; }
        public Vector2 origin { get; set; }

    }





    internal  interface IDrawRect
    {
        public int2 frameref { get; set; }  
        public int outputTileW { get; set; }
        public int outputTileH { get; set; }
        public int gridID { get; set; } //-- index dans la grille --
        public int horizontalFrames { get; set; } //-- bouton selection horizontal --
        public int verticalFrames { get; set; } //-- bouton selection vertical --
        public bool isloadedFromXML { get; set; }
        public Rectangle position { get; set; }
        public Rectangle DEFAULT_POSITION { get; set; }
        public Rectangle frame { get; set; }
        public Rectangle DEFAULT_FRAME { get; set; }
        public float alpha { get; set; }
        public Color color { get; set; }
        public int textureID { get; set; }
        public int frameSize { get; set; }

        public string isanimated { get; set; }
        public string isloop { get; set; }
        public string ispingpong { get; set; }
        public int totalframes { get; set; }
        public float speedAnim { get; set; }
        public float chronoAnim { get; set; }

        public float rotation { get; set; }

        public string currentState { get; set; }
        public Vector2 origin { get; set; }

    }
}
