using DinguEngine;
using DinguEngine.Camera;
using DinguEngine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using CTI_RPG;

namespace CTI_RPG.Scene
{
    public class Credits : ModelScene
    {
        public Credits(MainClass _mainclass) : base(_mainclass)
        {
        }

        SoundEffect clik;
        CutSceneGenerator cutscene1;
        Song musicCutscene1;
        List<Texture2D> _texturesCUTSCENE_1;
        List<string> _textCUTSCENE_1;
        List<string> _textCUTSCENE_1_line2;
        List<float> _timeCUTSCENE_1;


        float timer = 0.0f;
        float speed = 0.015f;

        private void LoadCutScene_1(ref ContentManager _content)
        {
            _texturesCUTSCENE_1 = new List<Texture2D>()
            {
                _content.Load<Texture2D>("Tilesets\\entree"),
                _content.Load<Texture2D>("Tilesets\\entree"),
                _content.Load<Texture2D>("Tilesets\\entree"),
                _content.Load<Texture2D>("Tilesets\\entree"),
                _content.Load<Texture2D>("Tilesets\\entree"),
                _content.Load<Texture2D>("Tilesets\\accueil"),
                _content.Load<Texture2D>("Tilesets\\accueil"),
                _content.Load<Texture2D>("Tilesets\\accueil"),
                _content.Load<Texture2D>("Tilesets\\accueil"),

            };
            _textCUTSCENE_1 = new List<string>()
            {
                "Credits to ",

                "Remierciements à ",

                "Graphismes réalisés par ",

                "Musiques jouée par  " ,

                "Effets sonores ",

                "Programmation ",

                "GameDesign ",

                "Une réalisation Gamecodeur ",

                "Merci d'avoir lu jusqu'au bout ",
            };
            _textCUTSCENE_1_line2 = new List<string>()
            {
                "GameCodeur.fr",

                "Gamecodeur discord community ",

                "Kendar Varnor     ",

                "ELV Games ",

                "ELV Games",

                "EBB Dan",

                "EBB Dan",

                " par des élèves GameCodeur ",

                "la version définitive est cours ",
            };
            _timeCUTSCENE_1 = new List<float>()
                {
                26.0f,
                26.0f,
                 26.0f, 26.0f, 26.0f, 26.0f, 26.0f, 26.0f, 
            };

            cutscene1 = new CutSceneGenerator();

            if (cutscene1 != null)
            {
                cutscene1.SetTimer(ref _timeCUTSCENE_1);
            }

            outputTExt1 = string.Empty;
            outputTExt2 = string.Empty;
        }

        public override void Load(ref ContentManager _content)
        {
            clik = _content.Load<SoundEffect>("Audio\\UI2_Button_2");
            musicCutscene1 = _content.Load<Song>("Songs\\OST 4 - Winged Swords of Mercy (Bonus Vocals) (Loopable)");
            MediaPlayer.Play(musicCutscene1);
            MediaPlayer.IsRepeating = false;

            maincamera = new TE_Camera( );
            uicamera = new TE_Camera( );
            friendcamera = new TE_Camera( );
            ennemycamera = new TE_Camera( );
            LoadCutScene_1(ref _content);

            base.Load(ref _content);
        }

        float alpha = 0;
        int currentIteration = 0;
        bool cutsceneIsOver = false;
        int olditeration = 10;
        int iterationTEXT1 = 0, iterationTEXT2 = 0;
        float speedText = 0.08f;
        string outputTExt1 = string.Empty;
        string outputTExt2 = string.Empty;
        float chronoText = 0;
        int textstate = 0;
        List<actorType> friends;
        List<actorType> ennemies;

        public override void Update()
        {
            timer += speed;

            if (cutsceneIsOver)
            {
                if(Mouse.GetState().LeftButton==ButtonState.Pressed)
                {
                    timer = 60;
                }
                if (timer >= 42.0f)
                {
                    MediaPlayer.Stop();
                    main.ChangeScene(scene.splashscreen);
                }

                return;
            }

            if (timer >= 2.0f)
            {
                timer = 2.0f;

            }
            else
            {


                return;
            }



            if (cutscene1 != null && !cutsceneIsOver)
            {
                cutsceneIsOver = cutscene1.Update().isOver;
                currentIteration = cutscene1.Update().iteration;
                alpha = cutscene1.GetAlpha();


                if (olditeration != currentIteration)
                {
                    iterationTEXT1 = 0;
                    iterationTEXT2 = 0;

                    outputTExt1 = string.Empty;
                    outputTExt2 = string.Empty;



                    outputTExt1 += _textCUTSCENE_1[currentIteration][iterationTEXT1];

                    textstate = 0;
                }


            }



            olditeration = currentIteration;

            if (_textCUTSCENE_1 == null)
            {
                return;
            }

            if (iterationTEXT1 != _textCUTSCENE_1[currentIteration].Length - 1
                && textstate == 0)
            {
                chronoText += speedText;

                if (chronoText >= 0.2f)
                {
                    if (iterationTEXT1 == _textCUTSCENE_1[currentIteration].Length - 2)
                    {
                        textstate = 1;
                        iterationTEXT1 = 0;
                        outputTExt2 += _textCUTSCENE_1_line2[currentIteration][0];
                        return;
                    }
                    else
                    {
                        iterationTEXT1++;
                        outputTExt1 += _textCUTSCENE_1[currentIteration][iterationTEXT1];
                        chronoText = 0;
                        PlayAudio(clik, 0.3f);
                    }
                }
            }
            else if (iterationTEXT1 == _textCUTSCENE_1[currentIteration].Length - 1
                && textstate == 0)
            {
                outputTExt1 = _textCUTSCENE_1[currentIteration];
                textstate = 1;
            }

            if (iterationTEXT2 != _textCUTSCENE_1_line2[currentIteration].Length - 1
                && textstate == 1)
            {
                chronoText += speedText;
                if (chronoText >= 0.2f)
                {
                    iterationTEXT2++;
                    outputTExt2 += _textCUTSCENE_1_line2[currentIteration][iterationTEXT2];
                    chronoText = 0;
                    PlayAudio(clik, 0.3f);
                }
            }
            else if (iterationTEXT2 == _textCUTSCENE_1_line2[currentIteration].Length - 1
                && textstate == 1)
            {
                outputTExt2 = _textCUTSCENE_1_line2[currentIteration];
            }


            //  outputTExt2 = string.Empty;//+= _textCUTSCENE_1_line2[iterationTEXT2];

            base.Update();
        }

        public override void Draw(ref SpriteBatch _sp)
        {
            if (_textCUTSCENE_1 == null || outputTExt2 == null) return;
            _sp.Draw(_texturesCUTSCENE_1[currentIteration],
               new Rectangle(-100, -70, 200, 100), Color.White * alpha);




            base.Draw(ref _sp);
        }





        public override void Draw_UI(ref SpriteBatch _spUI)
        {
            if (outputTExt1 == null) return;
            _spUI.DrawString(cutsceneFont, outputTExt1,
              new Vector2(-110, 35), Color.White * alpha);
            _spUI.DrawString(cutsceneFont, outputTExt2,
             new Vector2(-110, 55), Color.White * alpha);


            base.Draw_UI(ref _spUI);
        }

    }
}
