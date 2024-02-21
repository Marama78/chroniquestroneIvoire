using _TheShelter.GameWrapping;
using DinguEngine;
using DinguEngine.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShelter;

namespace _TheShelter.Scene
{
    public class StandarFight : ModelScene
    {
        SoundEffect snd_iswheeling;
        SoundEffect sndEnd;
        public StandarFight(MainClass _mainclass) : base(_mainclass)
        {
        }

        public override void Load(ref ContentManager _content)
        {
            MediaPlayer.Stop();
            List<actorType> ennemies = null;
            List<actorType> friends = null;
            //-- for battle scene --
           friends = new List<actorType>()
            {
                actorType.princess,
                actorType.soldier,
                actorType.archer,
                actorType.mage,
            };

            switch (RefugeWrapper._fightmode)
            {
                case FightMode.monsterlvl1:
                    ennemies = new List<actorType>()
                    {
                        actorType.wolf,
                        actorType.zombie,
                        actorType.wolf,
                         actorType.wolf,
                        actorType.zombie,
                        actorType.wolf,
                         actorType.wolf,
                        actorType.zombie,
                        actorType.wolf,
                    };
                    TE_Manager.bank_bonus = 50;
                    break;
                case FightMode.monsterlvl2:
                    ennemies = new List<actorType>()
                    {
                        actorType.zombie,
                        actorType.zombie,
                        actorType.zombie,
                         actorType.zombie,
                        actorType.zombie,
                        actorType.zombie,
                    };
                    TE_Manager.bank_bonus = 150;

                    break;

                case FightMode.monsterlvl3:
                    ennemies = new List<actorType>()
                    {
                        actorType.slim,
                        actorType.zombie,
                        actorType.slim,
                          actorType.slim,
                        actorType.zombie,
                        actorType.slim,
                          actorType.slim,
                        actorType.zombie,
                        actorType.slim,
                    };
                    TE_Manager.bank_bonus = 200;
                    break;

            }


            TE_Manager.friends.Clear();
            TE_Manager.friends = friends;
            TE_Manager.ennemies.Clear();
            TE_Manager.ennemies = ennemies;

            snd_iswheeling = _content.Load<SoundEffect>("Audio\\wheelaction\\UI2_Button_7");
            sndEnd = _content.Load<SoundEffect>("Audio\\\\Spell_Casting_3");
            maincamera = new TE_Camera( );
            system = _content.Load<Texture2D>("system\\actions");

            ennemiAction = ToOffset(new Rectangle(100, 64, 24, 24));

            end = Randomizer.GiveRandomInt(20, 40);
            base.Load(ref _content);
        }
        Rectangle ennemiAction;
        float chronochange = 0.0f;
        float chronoWheel = 0.0f;
        bool startWheel = true;
        int currentframeX_btn_wheel, oldFrame_btn_Wheel;
        int atk_btn_ticks;
        bool changescene = false;
        int end = 80;
        float alphaAll = 1.0f;

      
        public override void Update()
        {




            if(changescene)
            {
                chronochange += 0.015f;
                if (chronochange > 2.0f)
                {
                    changescene = false;
                    chronochange = 0;
                    main.ChangeScene(scene.combatmode);
                }

                return;
            }

            if (atk_btn_ticks >= end)
            {
                changescene = true;
                startWheel = false;

                int multiple = 1;

                if(currentframeX_btn_wheel>8)
                {
                    multiple = 3;
                }
                else if(currentframeX_btn_wheel>5)
                {
                    multiple = 2;
                }

                TE_Manager.bank_bonus *= multiple;

                PlayAudio(sndEnd,0.5f);
            }

            if (startWheel)
            {
                chronoWheel += 0.25f;

                if (chronoWheel >= 1.0f)
                {
                    chronoWheel = 0.0f;

                    int temp = currentframeX_btn_wheel;
                    while (true)
                    {
                        if (temp != oldFrame_btn_Wheel)
                        {
                            currentframeX_btn_wheel = temp;
                            break;
                        }
                        else
                        {
                            temp = Randomizer.GiveRandomInt(0, 10);
                        }
                    }
                    oldFrame_btn_Wheel = temp;
                    atk_btn_ticks++;
                    PlayAudio(snd_iswheeling, 0.2f);
                }

               

            }

            base.Update();
        }
        Texture2D system;
        public override void Draw(ref SpriteBatch _sp)
        {
            _sp.Draw(system,
             ennemiAction,
             new Rectangle(currentframeX_btn_wheel * 24, 0, 24, 24),
             Color.White * alphaAll);

            base.Draw(ref _sp);
        }
    }
}
