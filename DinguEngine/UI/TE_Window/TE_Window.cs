using DinguEngine.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/* élève de l'école de jeux vidéos, gamecodeur.fr, depuis 2017,
 * ceci est un extrait d'un code perso, dans le pur esprit viking
 * Faîtes en bon usage. Pas besoin de crédits. Un viking, çà vit de
 * conquêtes et de victoire, brouaaaaaahhhhhh!!!!
 * ---------------------------------------------------------------
 * tu peux le recopier, l'utiliser à des fins perso et commerciales,
 * ou bien le modifier à ta guise.
 * EBB Dan Marama
*/

namespace DinguEngine.UI.TE_Window
{
    public class TE_Window
    {

        DrawRect[] windowCells;


        public DrawRect[] DrawWindow(int2 sizeOfWindow,int2 offsetOnScreen,int outputCellSize = 48,int framesize = 32)
        {
            int windowsize = sizeOfWindow.x*sizeOfWindow.y;
            int width = sizeOfWindow.x;
            int height = sizeOfWindow.y;

            DrawRect[] windowCells = new DrawRect[windowsize];

            int line = -1;
            int column = 0;

            for (int i = 0; i < windowsize; i++)
            {
                if (i % width == 0)
                {
                    column = 0;
                    line++;
                }
                else
                {
                    column++;
                }

                //-- poser les tuiles --
                int posX = column * outputCellSize + offsetOnScreen.x;
                int posY = line * outputCellSize + offsetOnScreen.y;

                DrawRect temp = new DrawRect(posX, posY, outputCellSize, outputCellSize, framesize);
                temp.color = Color.White;
                temp.alpha = 1.0f;

                if(i==0)
                {
                    temp.SetFrame(0, 0, framesize);
                }
                else if(line==0 && column==width-1)
                {
                    temp.SetFrame(2, 0, framesize);
                }
                else if (line == 0)
                {
                    temp.SetFrame(1, 0, framesize);
                }
                else if(line<height-1 && column==0)
                {
                    temp.SetFrame(0, 1, framesize);
                }
                else if (line < height-1 && column == width - 1)
                {
                    temp.SetFrame(2, 1, framesize);
                }
                else if (line < height-1)
                {
                    temp.SetFrame(1, 1, framesize);
                }
                else if(line == height-1 && column == 0)
                {
                    temp.SetFrame(0, 2, framesize);
                }
                else if(line == height-1 && column==width-1)
                {
                    temp.SetFrame(2, 2, framesize);
                }
                else if (line == height-1)
                {
                    temp.SetFrame(1, 2, framesize);
                }

                windowCells[i]=temp;
            }

            return windowCells;
        }

    }
}
