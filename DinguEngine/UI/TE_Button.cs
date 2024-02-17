using Microsoft.Xna.Framework;
using DinguEngine.Shared;

/* élève de l'école de jeux vidéos, gamecodeur.fr, depuis 2017,
 * ceci est un extrait d'un code perso, dans le pur esprit viking
 * Faîtes en bon usage. Pas besoin de crédits. Un viking, çà vit de
 * conquêtes et de victoire, brouaaaaaahhhhhh!!!!
 * ---------------------------------------------------------------
 * tu peux le recopier, l'utiliser à des fins perso et commerciales,
 * ou bien le modifier à ta guise.
 * EBB Dan Marama
*/

namespace DinguEngine.UI
{
    public class TE_Button
    {

        public bool isUIcollisiondetected = true;
        public DrawRect drawrect;
        int2 frameData = new int2();
        int framesize=16;
        int framesizeW;
        int framesizeH;
        bool iscliked = false;

        bool isAvailable = true;

        public bool ISAvailable()
        { return isAvailable; }

        public void SetAsClicked(Color color)
        {
            drawrect.color = color;
            iscliked=true;
        }

        public void ReleaseClickedState()
        {
            drawrect.color = Color.White;
            iscliked = false;
        }

        public bool IsClicked()
        { return iscliked; }    

        public void SetDrawRect(DrawRect _drawrect)
        {
            this.drawrect = _drawrect;
        }
        public TE_Button(DrawRect _drawrect, int2 frameposition, int _frameSize)
        {
            this.drawrect = _drawrect;
            frameData = frameposition;
            framesize = _frameSize;
            framesizeH = framesize;
            framesizeW = framesize;
            drawrect.SetFrame(frameposition.x, frameposition.y, _frameSize);
        }

        public TE_Button(DrawRect _drawrect, int2 frameposition, int frameSizeW, int frameSizeH)
        {
            this.drawrect = _drawrect;
            drawrect.SetFrame(frameposition.x,frameposition.y,frameSizeW,frameSizeH);
            frameData = frameposition;

            framesize = 0;
            framesizeW= frameSizeW;
            framesizeH= frameSizeH;
        }

        public bool IsCollide(Point mousePosition)
        {
            if(!isUIcollisiondetected) { return false; }

            if (drawrect.DEFAULT_POSITION.Contains(mousePosition)) { return true; } return false;
        }

        public int2 GetFrameDataPosition()
        {
            return frameData;
        }

        public void SetFrameDataPosition(int2 frameposition, float _rotation,ref int _textID)
        {
            if(framesize!=0) { drawrect.SetFrame(frameposition.x, frameposition.y, framesize); return; }

            frameData = frameposition;

            this.drawrect.SetFrame(frameposition.x, frameposition.y, framesizeW, framesizeH);

            this.drawrect.SetRotation(ref _rotation);

            this.drawrect.SetTextureID(_textID);
        }

        public void FollowMouse(Point mousePosition)
        {
            drawrect.Move(mousePosition, -10, -10);
        }

        public Point GetPosition_as_POINT()
        {
            return new Point(drawrect.position.X, drawrect.position.Y);
        }
    }
}
