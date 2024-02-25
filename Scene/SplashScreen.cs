using DinguEngine;
using DinguEngine.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CTI_RPG;

namespace CTI_RPG.Scene
{
    public class SplashScreen : ModelScene
    {
        Texture2D[] textures;


        float delayedChrono = 0.0f;
        float alpha = 0.0f;

        int state = 0;


        SoundEffect splash;
        public SplashScreen( MainClass _mainclass) : base( _mainclass)
        {
        }

        public override void Load(ref ContentManager _content)
        {
            maincamera = new TE_Camera( );
            uicamera = new TE_Camera( );
            friendcamera = new TE_Camera( );
            ennemycamera = new TE_Camera( );
            splash = _content.Load<SoundEffect>("Audio\\splash");

            PlayAudio(splash,0.25f);


            textures = new Texture2D[1]
            {
                _content.Load<Texture2D>("Tilesets\\monogameLogo"),
            };

            base.Load(ref _content);
        }

        public override void Update()
        {
            float speed = 0.015f;

            switch(state)
            {
                case 0:

                    if(alpha>=1.0f)
                    {
                        alpha = 1.0f;
                        state=1;
                    }

                    alpha += 0.015f;
                    break;

                case 1:
                    delayedChrono += speed;

                    if(delayedChrono >= 1.0f)
                    {
                        delayedChrono = 0.0f;
                        state = 2;
                    }

                    break;

                case 2:

                    if (alpha <= 0.0f)
                    {
                        alpha = 0.0f;
                        state = 3;
                    }

                    alpha -= 0.005f;
                    break;

                    case 3: main.ChangeScene(scene.start); break;

            }



            base.Update();
        }

        public override void Draw(ref SpriteBatch _sp)
        {
            _sp.DrawString(cutsceneFont, "coded with ", new Vector2(-100, -50), Color.Orange*alpha);
            _sp.Draw(textures[0], new Rectangle(-100, -20, 200, 60), Color.White*alpha);


            base.Draw(ref _sp);
        }

        public override void Draw_UI(ref SpriteBatch _spUI)
        {
            base.Draw_UI(ref _spUI);
        }

    }
}
