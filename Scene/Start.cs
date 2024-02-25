using DinguEngine;
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
using CTI_RPG;

namespace CTI_RPG.Scene
{
    public class Start : ModelScene
    {
        Texture2D mainscreen;
        Texture2D panel;
        Texture2D cursor;
        Texture2D buttonCMD;
        Texture2D falling;

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
                int posX = -66 + panelCMDOffset.x;
                int posY = -26 + panelCMDOffset.y + i * 18-22;

                Rectangle temp = new Rectangle(posX, posY, 100, 16);
                DrawRect alta = new DrawRect(temp, 32);
                commands[i] = new TE_Button(alta, new int2(0,0),32);
                commands[i].drawrect.frame = new Rectangle(0, 0, 100, 28);
            }
        }

        float chronofallinganim;
        float chronofallinganim2;
        float chronofallinganim3;
        float chronofallinganim4;
        public override void Load(ref ContentManager _content)
        {

            //-- for battle scene --
            List<actorType> friends = new List<actorType>()
            {
                actorType.princess,
                actorType.soldier,
                actorType.archer,
            };
            List<actorType> ennemies = new List<actorType>()
            {
                actorType.kingGolem,
                actorType.zombie,
                actorType.zombie,
                actorType.zombie,
            };

            TE_Manager.friends.Clear();
            TE_Manager.friends = friends;
            TE_Manager.ennemies.Clear();
            TE_Manager.ennemies = ennemies;



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
            falling = _content.Load<Texture2D>("system\\falling_l");
            panel = _content.Load<Texture2D>("statusBar");
            cursor = _content.Load<Texture2D>("system\\cursor");
            buttonCMD = _content.Load<Texture2D>("statusBar");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(musicEffect);



            windowCommand = new TE_Window();
            commandPanel = windowCommand.DrawWindow(new int2(6, 4), new int2(-100 + panelCMDOffset.x, -40 + panelCMDOffset.y), 24, 12);

            SetCommands();

            positions = new List<Rectangle>();
            int rand = Randomizer.GiveRandomInt(4, 6);
            for (int i = 0; i < rand; i++)
            {
                int size = Randomizer.GiveRandomInt(6, 10);
                Rectangle temp = ToOffset(new Rectangle(Randomizer.GiveRandomInt(10, 220)
                    , Randomizer.GiveRandomInt(-10, 0), size, size));
                positions.Add(temp);
            }
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
                                case 1: isLOCKED = true; nextscene = scene.combatmode; break;  
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

        bool autoShow = false;
        float chronoshow = 0.0f;
        Rectangle frame1;
        List<Rectangle> positions;

        float chronoFalling_rate = 0;
        int limit = 4;
        public override void Update()
        {
           
            for (int i = 0; i < positions.Count; i++)
            {

           
                positions[i] = new Rectangle(positions[i].X+ Randomizer.GiveRandomInt(1, 3)-2,
                    positions[i].Y+ Randomizer.GiveRandomInt(1, 3)-1, positions[i].Width, positions[i].Height);
               
            }
           // positions.ForEach(x=>x=new Rectangle(x.X + Randomizer.GiveRandomInt(-3, 5), x.Y + 5, x.Width, x.Height));
            positions.RemoveAll(x => x.Y > 260);
            chronofallinganim += 0.15f;
            if (chronofallinganim>13)
            {
                

                chronofallinganim = 0.0f;
            }
            frame1 = new Rectangle((int)chronofallinganim * 32, 0, 32, 32);

            chronoFalling_rate += 0.15f;
            if(chronoFalling_rate>=limit)
            {
                chronoFalling_rate = 0;

                int rand = Randomizer.GiveRandomInt(4, 6);
                for (int i = 0; i < rand; i++)
                {
                    int size = Randomizer.GiveRandomInt(6, 10);
                    Rectangle temp = ToOffset(new Rectangle(Randomizer.GiveRandomInt(10, 220)
                        , Randomizer.GiveRandomInt(-10, 0), size, size));
                    positions.Add(temp);

                }

                limit = Randomizer.GiveRandomInt(3, 6);
            }

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
            
            
            if(!isFocusOnCommand)
            {
                chronoshow += 0.015f;
                if (chronoshow > 2.0f)
                {
                    autoShow = true;
                }
            }
           
            if((click && !isFocusOnCommand)||autoShow) {
                PlayAudio(snd_drawUI, 0.5f);
                isFocusOnCommand = true; 
                autoShow = false;
            }

            ReadCommands(ref cursorPos, ref click);

            base.Update();
        }

        public override void Draw(ref SpriteBatch _sp)
        {
            _sp.Draw(mainscreen, new Rectangle(cameraoffset.x, cameraoffset.y, 240,160), new Rectangle(0,0,240,160),
                Color.White*alpha,
                0, Vector2.Zero, SpriteEffects.None, 0.7f);

            for (int i = 0; i < positions.Count; i++)
            {
                _sp.Draw(falling, positions[i], frame1, Color.White);
            }


            _sp.Draw(buttonCMD,
               new Rectangle(commands[3].drawrect.position.X - 130, commands[3].drawrect.position.Y + 8 + 20, 180, 14),
               new Rectangle(0, 0, 16, 16),
               Color.Black,
               0, Vector2.Zero, SpriteEffects.None, 0.4f);
            _sp.DrawString(cutsceneFont, "F1,F2,F3,F4 - change screen resolution",
               new Vector2(commands[3].drawrect.position.X - 126, commands[3].drawrect.position.Y + 30), Color.Orange,
              0, Vector2.Zero, 0.8f, SpriteEffects.None, 0.1f);


            base.Draw(ref _sp);
        }

        public override void Draw_UI(ref SpriteBatch _spUI)
        {

            if(!isFocusOnCommand) { return; }
            /* for (int i = 0; i<commandPanel.Length; i++)
             {
                 _spUI.Draw(panel
                     , commandPanel[i].position
                     , commandPanel[i].frame
                     ,Color.White* uiAlpha* uiAlpha_CMD,
                     0.0f,Vector2.Zero,SpriteEffects.None,0.8f
                     );
             }*/


            _spUI.Draw(buttonCMD
                   ,new Rectangle(commands[0].drawrect.position.X-4, commands[0].drawrect.position.Y-4,
                   commands[0].drawrect.position.Width + 8, commands[0].drawrect.position.Height*4 + 20)
                   , new Rectangle(0,0,16,16)
                   , Color.Cyan * uiAlpha_CMD,
                    0.0f, Vector2.Zero, SpriteEffects.None, 0.4f);

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

            _spUI.DrawString(cutsceneFont, "version 1.0.0", 
                new Vector2(commands[3].drawrect.position.X + 24, commands[3].drawrect.position.Y + 15), Color.Black,
               0,Vector2.Zero,0.8f,SpriteEffects.None,0.1f );

           

            if (cursor == null) return;
            Rectangle mousecursorPosition = new Rectangle(cursorPos.X, cursorPos.Y, 20, 20);
            _spUI.Draw(cursor, mousecursorPosition, cursorRect, Color.White);


            base.Draw_UI(ref _spUI);
        }

    }
}
