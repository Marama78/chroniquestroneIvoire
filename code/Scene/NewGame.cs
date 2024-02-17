using DinguEngine;
using DinguEngine.Camera;
using DinguEngine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using TheShelter;

namespace _TheShelter.Scene
{
    public class NewGame : ModelScene
    {
        SoundEffect clik;
        CutSceneGenerator cutscene1;
        Song musicCutscene1;
        List<Texture2D> _texturesCUTSCENE_1;
        List<string> _textCUTSCENE_1;
        List<string> _textCUTSCENE_1_line2;
        List<float> _timeCUTSCENE_1;


        float timer = 0.0f;
        float speed = 0.015f;
        public NewGame(MainClass _mainclass) : base(_mainclass)
        {

        }

       
        private void LoadCutScene_1(ref ContentManager _content)
        {
            _texturesCUTSCENE_1 = new List<Texture2D>()
            {
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene1"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene2"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene3"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene4"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene5"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene6"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene7"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene8"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene9"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene10"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene11"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene12"), 
                _content.Load<Texture2D>("Cutscenes\\cutscene1\\cutscene13"), 
            };
            _textCUTSCENE_1 = new List<string>()
            {
                "Dans un pays éloignée des rois ",

                "Un guerrier de la prophétie , ",

                "et instaura une paix durable ",

                "de son union avec " ,
                
                "naquit une jolie princesse ",
                
                "lorsque la princesse eut ",

                "une armée venue de l'océan ",
                
                "Le combat fût féroce ",
                
                "la jeune princesse survécut ",

                "abbatue et fatiguée, elle n'eut ",
                
                "vers les terres montagneuses ",
                
                "dont l'entrée est protégée ",

                "votre aventure commence "
            };
            _textCUTSCENE_1_line2 = new List<string>()
            {
                "se disputaient pour un trône",

                "mit fin à cette folie. ",

                " ",

                "la reine des étoiles, ",

                " qui suivra les traces de son père.",

                "quatorze ans,",

                "attaqua le pays",

                " et les troupes perdirent la guerre",

                "grâce au sacrifice de ses parents",

                "d'autre choix que de fuir",

                "des nains",

                "par le roi des golems",

                "MAINTENANT !"
            };
            _timeCUTSCENE_1 = new List<float>()
                {
                6.0f,
                6.0f,
                 6.0f, 6.0f, 6.0f, 6.0f, 6.0f, 6.0f, 6.0f, 6.0f, 6.0f, 6.0f,
                 6.0f,
            };
        
            cutscene1 = new CutSceneGenerator();

            if(cutscene1!=null)
            {
                cutscene1.SetTimer(ref _timeCUTSCENE_1);
            }

            outputTExt1 = string.Empty;
            outputTExt2 = string.Empty;
        }

        public override void Load(ref ContentManager _content)
        {
            //-- for battle scene --
            List<actorType> friends = new List<actorType>()
            {
                actorType.princess,
                actorType.soldier, 
                actorType.archer,
                actorType.soldier,
            };
            List<actorType> ennemies = new List<actorType>()
            {
                actorType.kingGolem,
                actorType.zombie,
                actorType.zombie,
            };

            TE_Manager.friends.Clear();
            TE_Manager.friends = friends;
            TE_Manager.ennemies.Clear();
            TE_Manager.ennemies = ennemies;






            clik = _content.Load<SoundEffect>("Audio\\UI2_Button_2");
            musicCutscene1 = _content.Load<Song>("Songs\\OST 2 - Hymn Of Eternal Glory");
            MediaPlayer.Play(musicCutscene1);
            MediaPlayer.IsRepeating = false;

            maincamera = new TE_Camera();
            uicamera = new TE_Camera();
            friendcamera = new TE_Camera();
            ennemycamera = new TE_Camera();
            LoadCutScene_1(ref _content);

            base.Load(ref _content);
        }

        float alpha = 0;
        int currentIteration=0;
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

            if(cutsceneIsOver )
            {
                if(timer >=8.0f)
                {
                    main.ChangeScene(scene.combatmode);
                }

                return;
            }

            if(timer >= 2.0f)
            {
                timer = 2.0f;

            }
            else
            {


                return;
            }



            if(cutscene1 != null && !cutsceneIsOver)
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

            if(_textCUTSCENE_1 == null) 
            { 
                return; 
            }

            if(iterationTEXT1 != _textCUTSCENE_1[currentIteration].Length-1
                && textstate==0)
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
            if (_textCUTSCENE_1 == null || outputTExt2==null) return;
            _sp.Draw(_texturesCUTSCENE_1[currentIteration],
               new Rectangle(-100,-70,200,100), Color.White * alpha);


          

            base.Draw(ref _sp);
        }





        public override void Draw_UI(ref SpriteBatch _spUI)
        {
            if(outputTExt1 == null)return;
            _spUI.DrawString(cutsceneFont, outputTExt1,
              new Vector2(-110, 35), Color.White * alpha);
            _spUI.DrawString(cutsceneFont, outputTExt2,
             new Vector2(-110, 55), Color.White * alpha);


            base.Draw_UI(ref _spUI);
        }

    }
}
