using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/* élève de l'école de jeux vidéos, gamecodeur.fr, depuis 2017,
 * ceci est un extrait d'un code perso, dans le pur esprit viking
 * Faîtes en bon usage. Pas besoin de crédits. Un viking, çà vit de
 * conquêtes et de victoire, brouaaaaaahhhhhh!!!!
 * ---------------------------------------------------------------
 * tu peux le recopier, l'utiliser à des fins perso et commerciales,
 * ou bien le modifier à ta guise.
 * EBB Dan Marama
*/

namespace DinguEngine.Inputs
{
    public class TE_Gamepad
    {
        public static float GetHorizontal()
        {
            return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
        }

    }
}
