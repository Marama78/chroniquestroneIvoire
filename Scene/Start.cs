using DinguEngine.Camera;
using DinguEngine.Shared;
using DinguEngine.UI;
using DinguEngine.UI.TE_Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Threading;
using TheShelter;

namespace _TheShelter.Scene
{
    public class Start : ModelScene
    {
        Texture2D mainscreen;
        Texture2D panel;
        Texture2D cursor;
        Texture2D buttonCMD;


        SoundEffect snd_drawUI;
        SoundEffect snd_mouseHover;
        SoundEffect snd_validate;
        Song musicEffect;


        float alpha=0.0f;
        float delayedChrono = 0.0f;
        int state = 0;

        bool isPingPongOver = false;
        TE_Button[] commands;
        TE_Window windowCommand;
        DrawRect[] commandPanel;

        float uiAlpha = 0.7f;
        float uiAlpha_CMD = 0.7f;

        Point cursorPos; Rectangle cursorRect; bool isFocusOnCommand = false;

        int2 cameraoffset;
        int2 panelCMDOffset;
        int colliderTicks = 0;
        int mouseTicks = 0;

        List<string> commandNames;

        bool isLOCKED = false;
        scene nextscene;

        public Start(MainClass _mainclass) : base(_mainclass)
        {
        }

      private void SetCommands()
        {
            commandNames = new List<string>()
            { 
                "Nouveau",
                "Charger",
                "Credits",
                "Quitter"
            };

            commands = new TE_Button[4];

            for (int i = 0; i < 4; i++)
            {
                int posX = -76 + panelCMDOffset.x;
                int posY = -26 + panelCMDOffset.y + i * 18;

                Rectangle temp = new Rectangle(posX, posY, 100, 16);
                DrawRect alta = new DrawRect(temp, 32);
                commands[i] = new TE_Button(alta, new int2(0,0),32);
                commands[i].drawrect.frame = new Rectangle(0, 0, 100, 28);
            }
        }
        public override void Load(ref ContentManager _content)
        {
            panelCMDOffset = new int2(80, 30);
            cameraoffset = new int2(-120, -80);

            maincamera = new TE_Camera( );
            uicamera = new TE_Camera( );
            friendcamera = new TE_Camera( );
            ennemycamera = new TE_Camera( );

            cursorRect = new Rectangle(0, 0, 16, 16);

            snd_drawUI = _content.Load<SoundEffect>("Audio\\UI2_Splash_1");
            snd_mouseHover = _content.Load<SoundEffect>("Audio\\UI2_Button_7");
            snd_validate = _content.Load<SoundEffect>("Audio\\UI2_Accept_1");

            musicEffect = _content.Load<Song>("Songs\\OST 5 - Wrath of the Norns");
            mainscreen = _content.Load<Texture2D>("Tilesets\\mainscreen");
            panel = _content.Load<Texture2D>("statusBar");
            cursor = _content.Load<Texture2D>("system\\cursor");
            buttonCMD = _content.Load<Texture2D>("system\\menu");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(musicEffect);



            windowCommand = new TE_Window();
            commandPanel = windowCommand.DrawWindow(new int2(6, 4), new int2(-100 + panelCMDOffset.x, -40 + panelCMDOffset.y), 24, 12);

            SetCommands();

            base.Load(ref _content);




        }

        private void PingPongAlphaMainscreen()
        {

            switch (state)
            {
                case 0:
                    delayedChrono += 0.015f;

                    if (delayedChrono >= 1.0f)
                    {
                        delayedChrono = 0.0f;
                        state = 1;
                    }

                    break;

                case 1:
                    alpha += 0.005f;

                    if (alpha >= 1.0f)
                    {
                        alpha = 1.0f;
                        state = 2;
                    }

                    break;

                

                case 2:
                    alpha = 1.0f; isPingPongOver = true;
                    state++; break;

            }
        }

        private void ExitScene()
        {

            switch (state)
            {

                case 3:
                    alpha -= 0.005f;
                    uiAlpha_CMD = alpha;
                    if (alpha <= 0.0f)
                    {
                        alpha = 0.0f;
                        state = 4;
                    }

                    if (alpha <= 0.8f) isFocusOnCommand = false;
                    break;



                case 4:
                    main.ChangeScene(nextscene);
                    MediaPlayer.Stop();
                    state++; break;

            }
        }


        private void ReadCommands(ref Point mouseposition, ref bool isclicked)
        {
          

            if (!isFocusOnCommand) { return; }

            delayedChrono += 0.015f;

            if (delayedChrono <= 3.0f) return;
            else if (delayedChrono >= 3.0f) delayedChrono = 4.0f;

            int hitter = 0;
            for (int i = 0; i < commands.Length; i++)
            {

                if (commands[i].IsCollide(mouseposition))
                {
                    if(colliderTicks==0)
                    {
                        PlayAudio(snd_mouseHover, 0.5f);
                    }
                    hitter++;
                    colliderTicks++;
                    commands[i].drawrect.color = Color.Cyan;

                    if(isclicked)
                    {
                        if(mouseTicks==0)
                        {
                            PlayAudio(snd_validate, 0.5f);
                        }

                        mouseTicks++;   
                        switch(i)
                        {
                                case 0: isLOCKED = true; nextscene = scene.newgame; break;
                                case 1: isLOCKED = true; nextscene = scene.refuge; break;  
                                case 2: isLOCKED = true; nextscene = scene.credits; break;
                                case 3: isLOCKED = true; nextscene = scene.quit; break;
                        }
                    }
                }
                else
                {
                    commands[i].drawrect.color = Color.White;
                }
            }

            if (hitter == 0)
            {
                mouseTicks = 0;
                colliderTicks = 0;
            }
        }

        public override void Update()
        {
            if (isLOCKED)
            {
                ExitScene();
                uicamera.ZoomOUT(alpha);
                return;
            }
            if (!isPingPongOver) 
            {
                PingPongAlphaMainscreen();
                return;
            }

            if (uicamera == null || maincamera == null) return;
            MouseState mouseState = Mouse.GetState();
            Point mouseposition = mouseState.Position;
            cursorPos = uicamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();
            var click = mouseState.LeftButton == ButtonState.Pressed;

            if(click && !isFocusOnCommand) {
                PlayAudio(snd_drawUI, 0.5f);
                isFocusOnCommand = true; 
            }

            ReadCommands(ref cursorPos, ref click);

            base.Update();
        }

        public override void Draw(ref SpriteBatch _sp)
        {
            _sp.Draw(mainscreen, new Rectangle(cameraoffset.x, cameraoffset.y, 240,160), Color.White*alpha);
            base.Draw(ref _sp);
        }

        public override void Draw_UI(ref SpriteBatch _spUI)
        {

            if(!isFocusOnCommand) { return; }
            for (int i = 0; i<commandPanel.Length; i++)
            {
                _spUI.Draw(panel
                    , commandPanel[i].position
                    , commandPanel[i].frame
                    ,Color.White* uiAlpha* uiAlpha_CMD,
                    0.0f,Vector2.Zero,SpriteEffects.None,0.8f
                    );
            }


            for (int i = 0; i < commands.Length; i++)
            {
                _spUI.Draw(buttonCMD
                   , commands[i].drawrect.position
                   , commands[i].drawrect.frame
                   , commands[i].drawrect.color* uiAlpha_CMD,
                    0.0f, Vector2.Zero, SpriteEffects.None, 0.4f);

                Vector2 posText = new Vector2(
                    commands[i].drawrect.position.X + 4
                    , commands[i].drawrect.position.Y -2
                    );

                _spUI.DrawString(cutsceneFont, commandNames[i], posText,Color.Black);

            }

            if (cursor == null) return;
            Rectangle mousecursorPosition = new Rectangle(cursorPos.X, cursorPos.Y, 20, 20);
            _spUI.Draw(cursor, mousecursorPosition, cursorRect, Color.White);


            base.Draw_UI(ref _spUI);
        }

    }
}
