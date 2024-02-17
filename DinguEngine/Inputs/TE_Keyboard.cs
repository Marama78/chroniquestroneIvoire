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
    public static class TE_Keyboard
    {
        public static bool GetKeyDown(Keys key)
        {
            TE_Manager.kbstate = Keyboard.GetState();

            if (TE_Manager.kbstate.IsKeyDown(key) && TE_Manager.old_key != key)
            {
                TE_Manager.old_key = key;
                return true;
            }
            else if (TE_Manager.kbstate.IsKeyUp(key) && TE_Manager.old_key == key)
            {
                TE_Manager.old_key = 0;
            }

            return false;
        }
        public static bool IsKeyPressed(Keys key)
        {
            TE_Manager.kbstate = Keyboard.GetState();

            if (TE_Manager.kbstate.IsKeyDown(key) && TE_Manager.old_key != key)
            {
                TE_Manager.old_key = key;
                return true;
            }
            else if (TE_Manager.kbstate.IsKeyUp(key) && TE_Manager.old_key == key)
            {
                TE_Manager.old_key = 0;
            }

            return false;
        }


      public static bool MoveLeft()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Left);
        }
        public static bool MoveRight()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Right);
        }
        public static bool MoveUp()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Up);
        }
        public static bool MoveDown()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Down);
        }

        public static bool IsScrollLeft()
        {
            return IsKeyPressed(Keys.Left);
        }
        public static bool IsScrollRight()
        {
            return IsKeyPressed(Keys.Right);
        }
        public static bool IsScrollUp()
        {
            return IsKeyPressed(Keys.Up);
        }
        public static bool IsScrollDown()
        {
            return IsKeyPressed(Keys.Down);
        }

        public static bool IsZoomOut()
        {
            return IsKeyPressed(Keys.Y);
        }
        public static bool IsZoomIn()
        {
            return IsKeyPressed(Keys.U);
        }

        public static bool IsValidate()
        {
            return IsKeyPressed(Keys.Enter);
        }

        public static bool IsCancel()
        {
            return IsKeyPressed(Keys.Escape);
        }

        public static bool IsDebuggingTimeLiner()
        {
            return IsKeyPressed(Keys.Space);
        }

    }
}
