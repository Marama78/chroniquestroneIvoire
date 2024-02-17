using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;

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

    public class GameData
    {
        public int food;
        public int foodMAX;
        public int foodIncr;

        public int water,waterMAX,waterIncr;

        public int energy,energyMAX,energyIncr;


        public int totalsoldiers, totalmages, totalpriest, totalarcher;


    }
    public class Colliders
    {
        Rectangle position;
        Rectangle frame;
        bool ishidden;
        int framesize;
        int tilesize;
    }


    public struct TextureData
    {
        public string path;
        public int framesize;
        public int totalRows;
        public int totalColumns;

        public TextureData(string path, int framesize, int totalLine, int totalColumns)
        {
            this.path = path;
            this.framesize = framesize;
            this.totalRows = totalLine;
            this.totalColumns = totalColumns;
        }
    }
    public struct xmlTexture2D
    {
        public Texture2D tex;
        public int id = 0;

        public int totalLines = 0;
        public int totalcolumns = 0;
        public int framesize = 32;

        public xmlTexture2D(Texture2D tex, int id, int totalLRnes, int totalcolumns, int framesize)
        {
            this.tex = tex;
            this.id = id;
            this.totalLines = totalLRnes;
            this.totalcolumns = totalcolumns;
            this.framesize = framesize;
        }

        public int GetId()
        { return id; }
    }
    public struct int2
    {
        public int x; public int y;

        public int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    public class DrawRect : IDrawRect
    {
        public int horizontalFrames { get; set; } //-- bouton selection horizontal --
        public int verticalFrames { get; set; } //-- bouton selection vertical --
        public Rectangle DEFAULT_POSITION { get; set; }
        public Rectangle DEFAULT_FRAME { get; set; }

        public int gridID { get; set; }
        public int outputTileW { get; set; }
        public int outputTileH { get; set; }

        public bool isloadedFromXML { get; set; }
        public Rectangle position { get; set; }
        public Rectangle frame { get; set; }
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
        public int2 frameref { get; set; }

        private void INITIALIZE()
        {
            frameSize = 16;
            horizontalFrames = 0;
            verticalFrames = 0;
            isloadedFromXML = false;
            isanimated = "no";
            isloop = "no";
            ispingpong = "no";
            totalframes = 0;
            speedAnim = 0.035f;
            chronoAnim = 0;

            rotation = 0;

            currentState = "empty"; //hidden - open - closed - destroyed
            origin = Vector2.Zero;
        }

        public DrawRect(float posX, float posY, int sizeW, int sizeH, int framesize = 16,float _rotation = 0, float _alpha = 1.0f)
        {
            INITIALIZE();
            this.position = new Rectangle((int)posX, (int)posY, sizeW, sizeH);
            this.DEFAULT_POSITION = new Rectangle((int)posX, (int)posY, sizeW, sizeH);
            this.color = Color.White;
            this.alpha = _alpha;
            this.frameSize = framesize;
            this.rotation = _rotation;
            this.outputTileW = sizeW; this.outputTileH = sizeH; 
            origin = new Vector2(frameSize/2, frameSize/2);
        }
        public DrawRect(Rectangle position, int framesize = 16, float _rotation = 0, float alpha = 1.0f)
        {
            INITIALIZE();
            this.position = position;
            this.DEFAULT_POSITION = position;
            
            this.alpha = alpha;
            this.color = Color.White;
            this.frameSize = framesize;
            this.rotation = _rotation;
            origin = new Vector2(frameSize / 2, frameSize / 2);
        }

        public DrawRect()
        {
        }

        public bool IsEmpty()
        {
            bool frameY = false;
            bool texID = false;

            if(frame.Y<40) frameY = true;
            if (textureID == 3) texID = true;

            if (texID && frameY) return true;
            return false;
        }

        public void PushRotation(bool reverse = false)
        {
            float tick = MathHelper.ToRadians(90.0f);
            float limit = MathHelper.ToRadians(360.0f);

            if (!reverse)
            {
                rotation += tick;
                if (rotation >= limit) rotation = 0;
            }
            else
            {
                rotation -= tick;
                if (rotation <= 0) rotation = limit;
            }

            SetMultipleSelectionSize(horizontalFrames, verticalFrames);
        }
        public Rectangle GetPosition()
        { return position; }
        public Rectangle GetFrame()
        { return frame; }
        public float GetAlpha() { return alpha; }
        public Color GetColor() { return color; }

        public void SetMultipleSelectionSize(int width, int height, int framesize = 16)
        {
            frameSize = framesize;
            horizontalFrames = width;
            verticalFrames = height;

            if (horizontalFrames != 0 || verticalFrames != 0)
            {
                int posX = 0;
                int posY = 0;

                int rectW = outputTileW * horizontalFrames + outputTileW;
                int rectH = outputTileH * verticalFrames + outputTileH;

                if (MathHelper.ToDegrees(rotation) >= 360) rotation = 0;
                if (MathHelper.ToDegrees(rotation) <0) rotation = 0;
                
                frame = new Rectangle(frame.X, frame.Y, frameSize + (horizontalFrames * frameSize), frameSize + (verticalFrames * frameSize));

                if(MathHelper.ToDegrees(rotation)>=0 &&  MathHelper.ToDegrees(rotation)<=80)
                {
                    posX = DEFAULT_POSITION.X - rectW + outputTileW;
                    posY = DEFAULT_POSITION.Y - rectH + outputTileH;

                }
                if (MathHelper.ToDegrees(rotation)>80 &&  MathHelper.ToDegrees(rotation)<=170)
                {
                    posX = DEFAULT_POSITION.X;
                    posY = DEFAULT_POSITION.Y - rectW + outputTileH;
                }
                if (MathHelper.ToDegrees(rotation) > 170 && MathHelper.ToDegrees(rotation) <= 260)
                {
                    posX = DEFAULT_POSITION.X;
                    posY = DEFAULT_POSITION.Y;
                }
                if (MathHelper.ToDegrees(rotation) > 260 && MathHelper.ToDegrees(rotation) < 350)
                {
                    posX = DEFAULT_POSITION.X - rectH + outputTileW;
                    posY = DEFAULT_POSITION.Y;
                }

                position = new Rectangle(posX, posY, rectW, rectH);
            }
            else
            {
                position = DEFAULT_POSITION;
                frame = DEFAULT_FRAME;
            }
        }

        public void ResetDrawRectPosition_Frame()
        {
            horizontalFrames = 0;
            verticalFrames = 0;
            position = DEFAULT_POSITION;
            SetFrame(0, 0,frameSize) ;
            rotation = 0.0f;
        }

        public void ResetDrawRect()
        {
            textureID = 0;
            
            horizontalFrames = 0;
            verticalFrames = 0;
            position = DEFAULT_POSITION; 
            SetFrame(0, 0,frameSize) ;
            rotation = 0.0f;
        }

        public void SetOutputTilSize(int width, int height)
        {
            outputTileW = width;
            outputTileH = height;
        }

        public void SetGridID(int gridID)
        { this.gridID = gridID; }
        public int GetGridID()
        { return gridID; }

       
        public bool isEmpty()
        {
            if (textureID == 0 && frame.X == 0 && frame.Y == 0) return true;
            return false;
        }

        public float GetRotation()
        { return rotation; }

        public void SetRotation(ref float _rotation)
        { rotation=_rotation; }
        public void SetAsLoadedFromXML()
        { isloadedFromXML = true; }
        public void SetAsAnimated(int _totalframes, bool _isloop = true, bool _ispingpong = false, float speed = 0.035f)
        {
           
            isanimated = "yes";
            totalframes = _totalframes;
            speedAnim = speed;

            if (_isloop) isloop = "yes";
            if(_ispingpong) ispingpong = "yes";

        }

        public void Animate(bool isVertical
            = false)
        {
            if (isanimated == "no") return;

            if(chronoAnim<=totalframes)
            {
                chronoAnim += speedAnim;
            }

            if(ispingpong == "no")
            {
                if (chronoAnim >= totalframes)
                {
                    if (isloop == "yes")
                    {
                        chronoAnim = 0;
                    }
                    else
                    {
                        chronoAnim = totalframes;
                    }
                }
            }
            else
            {
                if (chronoAnim >= totalframes ||chronoAnim<=0)
                {
                        speedAnim = -speedAnim;
                        chronoAnim = totalframes;
                }
            }
           

            if(!isVertical)
            {
                int newframeX_ref = DEFAULT_FRAME.X + (int)chronoAnim;
                frameref = new int2(newframeX_ref, frameref.y);
                int newframeX = DEFAULT_FRAME.X + (int)chronoAnim * frameSize;
                PushFrame_by_X(newframeX);
            }
            else
            {
                int newframeY_ref = DEFAULT_FRAME.Y + (int)chronoAnim;
                frameref = new int2(frameref.x, newframeY_ref);
                int newframeY = DEFAULT_FRAME.Y + (int)chronoAnim * frameSize;

                PushFrame_by_Y(newframeY);
            }
            

        }


        public void Move(Vector2 delta, int horizontalOffset = 0, int verticalOffset = 0)
        {
            Vector2 newposition = new Vector2(
        MathHelper.Lerp(position.X, delta.X, 0.08f),
        MathHelper.Lerp(position.Y, delta.Y, 0.08f));

            this.position = new Rectangle((int)delta.X + horizontalOffset, (int)delta.Y + verticalOffset, position.Width, position.Height);
        }

        public void Move(Point delta, int horizontalOffset = 0, int verticalOffset = 0)
        {
            Vector2 newposition = new Vector2(
        MathHelper.Lerp(position.X, delta.X, 0.08f),
        MathHelper.Lerp(position.Y, delta.Y, 0.08f));

            this.position = new Rectangle((int)delta.X + horizontalOffset, (int)delta.Y + verticalOffset, position.Width, position.Height);
        }

        private void PushFrame_by_X(int frameX)
        {
            frame = new Rectangle(frameX,frame.Y, frame.Width, frame.Height);
        }

        private void PushFrame_by_Y(int frameY)
        {
            frame = new Rectangle(frame.X, frameY, frame.Width, frame.Height);
        }

        public void SetXMLFrame(int x, int y, int tileSize)
        {
            frameref = new int2(x, y);
            frame = new Rectangle( x,  y, tileSize, tileSize);
            DEFAULT_FRAME = new Rectangle( x,  y, tileSize, tileSize);
        }


        public void SetFrame(int x, int y, int frameW, int frameH)
        {
            frameref = new int2(x, y);
            frame = new Rectangle(frameW * x, frameH * y, frameW, frameH);
            DEFAULT_FRAME = new Rectangle(frameW * x, frameH * y, frameW, frameH);
        }

        public void SetFrame(int x, int y, int frameSize)
        {
            frameref = new int2(x, y);
            frame = new Rectangle(frameSize * x, frameSize * y, frameSize, frameSize);
            DEFAULT_FRAME = new Rectangle(frameSize * x, frameSize * y, frameSize, frameSize);
        }

        public void SetTextureID(int _textureID)
        {
            this.textureID = _textureID;
        }

        public int GetTextureID()
        { return textureID; }   
    }

}
