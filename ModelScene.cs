using DinguEngine.Camera;
using DinguEngine.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CTI_RPG
{
    public abstract class ModelScene
    {
        public TE_Camera maincamera;
        public TE_Camera uicamera;
        public TE_Camera friendcamera;
        public TE_Camera ennemycamera;
        protected MainClass main;
        protected SpriteFont mainFont, cutsceneFont, UI_Fonts;
       // protected int2 cameraOffset = new int2(-120, -80);
        protected int2 cameraOffset = new int2(-120, -80);
        protected Color[] myColors;
        protected Rectangle ToOffset(Rectangle value)
        {
            return new Rectangle(value.X + cameraOffset.x, value.Y + cameraOffset.y, value.Width, value.Height); ;
        }

        protected Vector2 ToOffset(Vector2 value)
        {
            return new Vector2(value.X + cameraOffset.x, value.Y + cameraOffset.y);
        }

        protected Point ToOffset(Point value, bool reverse = false)
        {
            if(!reverse)
            return new Point(value.X + cameraOffset.x, value.Y + cameraOffset.y);
        else
                return new Point(value.X +240, value.Y +160);

        }


        protected ModelScene(MainClass _mainclass)
        {
            main = _mainclass;

            //-- setup From Dawnbringer palette
            myColors = new Color[]
            {
            new Color(34,32,52),
            new Color(69,40,60),
            new Color(102,57,49),
            new Color(143,86,59),
            new Color(223,113,38),
            new Color(217,160,102),
            new Color(238,195,154),
            new Color(63,63,116),
            new Color(48,96,130),
            new Color(153,229,180),
            new Color(55,18,110),


            };


        }

        public virtual void Load(ref ContentManager _content)
        {
            mainFont = _content.Load<SpriteFont>("mainFont");
            cutsceneFont = _content.Load<SpriteFont>("cutsceneFont");
            UI_Fonts = _content.Load<SpriteFont>("UI_Fonts");
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(ref SpriteBatch _sp)
        {

        }

        public virtual void Draw_UI(ref SpriteBatch _spUI)
        {

        }

        public virtual void Draw_Friend(ref SpriteBatch _spUI)
        {

        }

        public virtual void Draw_Ennemy(ref SpriteBatch _spUI)
        {

        }

        protected void PlayAudio(SoundEffect effect, float volume = 1.0f)
        {
            effect.Play(volume, pitch: 0.0f, pan: 1.0f);
        }

    }
}
