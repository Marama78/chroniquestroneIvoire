using _TheShelter.CombatSystem;
using DinguEngine;
using DinguEngine.Camera;
using DinguEngine.Shared;
using DinguEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TheShelter;

namespace _TheShelter.Scene
{
    public enum SFX_style
    {
        empty,
        sword,
        fire,
        ice,
        thunder,
        fistofryu,
        invoke,
        earth,
        arrow,

        skull,
        mouse,
    }
    public enum magicType
    {
        fire,
        earth,
        ice,
        healing,
        drainmagic,
        drainlife,
    }



    public class CombatMode : ModelScene
    {
        Song bgmusic;
        List<Texture2D> textures;
        List<Texture2D> backgrounds;
        Texture2D statusTex;
        Texture2D SFX;
        Texture2D texennemiaction;


        Rectangle playerCard, ennemiCard, sfxCard;

        Vector2 playerNamePosition, enemiNamePosition;

        Rectangle playerLifeBar, playerMagicBar, ennemiMagicBar, ennemiLifeBar;
        Rectangle playerLifeBar_BG, playerMagicBar_BG, ennemiMagicBar_BG, ennemiLifeBar_BG;

        Rectangle backgroundPosition;

        Actor princesse, soldier, archer, mage, priest;

        Actor kinggolem;
        bool canChangeActor = false;
        float chronoChange = 0.0f;

        List<Actor> actors_friends, actors_ennemies;
        Actor[] picker_actors_friends, picker_actors_ennemies;
        int currentFriendListID = 0, currentEnnemyListID = 0;
        Rectangle bg_commands_position_vect2;

        SFX_style currentsf_style;
        float alpha_playerSFX = 0.0f;

        float openCommand = 0.0f;

        TE_Button btn_actionWheel;
        int texturewheelID = 0;

        SoundEffect snd_iswheeling, snd_badresult, snd_good_result;
        float alpha_friend = 1.0f;

        int switchfriendState = 0;
        int nextactorID = 0;


        bool closeFight = false;

        bool isEnnemyTurn = false;

        int currentframeX_btn_wheel = 0;
        float chronoWheel = 0.0f;
        float speedwheel = 0.25f;
        public bool startWheel = false;

        Point UIcursorPos;
        int wheelhitter = 0;
        Texture2D cursor;
        Rectangle frame_cursor;
        int mouseticks = 0;

        Rectangle ennemiAction, ennemiFrameAction;
        int oldFriendID = 0;
        int nextEnnemiID = 0;
        int stopAction = 7;
        float chronoAutoAction = 0.0f;
        int randomEndWheel_Ennemy=4
            ;
        float alphaactionennemi = 0.0f;
        int atk_btn_ticks = 0;

        bool isQuitEnnemyTurn = false;
        float chronoNext = 0;
        float alphaALL = 1.0f;

      
        private void CleanAllActors()
        {
          

            actors_ennemies.RemoveAll(x => x.life <= 0);
            actors_friends.RemoveAll(x => x.life <= 0);

            #region <is fight over?>
            if (actors_ennemies.Count <= 0)
            {
                shutdownCombat = true;
                Debug.WriteLine("LayoutKind win");
                nextscene = scene.refuge;
                Exit_Scene();
                return;
            }

            if (actors_friends.Count <= 0)
            {
                Debug.WriteLine("end game , you loose");
                shutdownCombat = true;

                //--- CEST ICI QUAND LE JOUEUR PERD !!!! ---
                nextscene = scene.loose;
                Exit_Scene();
                return;
            }
            #endregion </is fight over?>

            #region <list hitted an udpate>
            if(currentEnnemyListID>=actors_ennemies.Count)
            {
                currentEnnemyListID = actors_ennemies.Count - 1;
            }
            if (currentFriendListID >= actors_friends.Count)
            {
                currentFriendListID = actors_friends.Count - 1;
            }
            #endregion </list hitted an udpate>

            UpdateStats();

        }

        public CombatMode(MainClass _mainclass) : base(_mainclass)
        {

        }

        #region <LOADING>
        private void LoadSounds(ref ContentManager _content)
        {
            snd_iswheeling = _content.Load<SoundEffect>("Audio\\wheelaction\\UI2_Button_7");
            snd_badresult = _content.Load<SoundEffect>("Audio\\wheelaction\\UI2_Decline_4");
            snd_good_result = _content.Load<SoundEffect>("Audio\\wheelaction\\UI2_Trophy_2");

        }

        private void LoadTextures(ref ContentManager _content)
        {
            textures = new List<Texture2D>()
            {
                _content.Load<Texture2D>("system\\actions"),
                _content.Load<Texture2D>("units\\battle\\unitsfriends"),
                _content.Load<Texture2D>("units\\battle\\enemies"),
                _content.Load<Texture2D>("units\\battle\\badguys"),
            };

            backgrounds = new List<Texture2D>()
            {
                _content.Load<Texture2D>("units\\battle\\backgrounds\\bg_battle1"),
            };

            cursor = _content.Load<Texture2D>("system\\cursor");
            SFX = _content.Load<Texture2D>("units\\battle\\fxatk");
        }
        private void LoadMusics(ref ContentManager _content)
        {
            bgmusic = _content.Load<Song>("Songs\\OST 1 - Warrior's Oath (Loopable)");

            endingSongs = new Song[]
            {
                _content.Load<Song>("Songs\\OST 1 - Fizz and Fiddle"),
                _content.Load<Song>("Songs\\OST 5 - Wrath of the Norns"),
                _content.Load<Song>("Songs\\OST 2 - Hymn Of Eternal Glory"),
            };
        }


        Rectangle wheelRect;
        private void SetActionPanel()
        {
            wheelRect = ToOffset(new Rectangle(180, 98, 44, 46));

            DrawRect temp = new DrawRect(wheelRect, 24);
            btn_actionWheel = new TE_Button(temp, new int2(0, 0), 24);

        }

        private void SetBattlers()
        {
            actors_ennemies = new List<Actor>();
            actors_friends = new List<Actor>();;


            int2 myunitFramesize = new int2(75, 95);
            int2 ennemiesFramesize = new int2(48, 64);


            picker_actors_friends = new Actor[5]
            {
                 new Actor(actorType.princess, 1, new int2(0, 0), myunitFramesize),
             new Actor(actorType.soldier, 1, new int2(1, 0), myunitFramesize),
                new Actor(actorType.archer, 1, new int2(2, 0), myunitFramesize),
             new Actor(actorType.mage, 1, new int2(3, 0), myunitFramesize),
             new Actor(actorType.priest, 1, new int2(4, 0), myunitFramesize),
        };

            picker_actors_ennemies = new Actor[]
            {
                new Actor(actorType.kingGolem, 3, new int2(0, 0), ennemiesFramesize),
                new Actor(actorType.zombie, 3, new int2(1, 0), ennemiesFramesize),
                new Actor(actorType.wolf, 3, new int2(2, 0), ennemiesFramesize),
                new Actor(actorType.carnivoreplant, 3, new int2(3, 0), ennemiesFramesize),
                new Actor(actorType.slim, 3, new int2(0, 1), ennemiesFramesize),
                new Actor(actorType.golemstone, 3, new int2(1, 1), ennemiesFramesize),
                new Actor(actorType.golemfire, 3, new int2(2, 1), ennemiesFramesize),
                new Actor(actorType.golemice, 3, new int2(3, 1), ennemiesFramesize),

                new Actor(actorType.soldier_bad, 3, new int2(0, 2), ennemiesFramesize),
                new Actor(actorType.archer_bad, 3, new int2(1, 2), ennemiesFramesize),
                new Actor(actorType.mage_bad, 3, new int2(2, 2), ennemiesFramesize),
                new Actor(actorType.priest_bad, 3, new int2(3, 2), ennemiesFramesize),

                new Actor(actorType.emperor, 3, new int2(0, 3), ennemiesFramesize),
                new Actor(actorType.queen, 3, new int2(1, 3), ennemiesFramesize),
                new Actor(actorType.soldiershief, 3, new int2(2, 3), ennemiesFramesize),
                new Actor(actorType.priestchief, 3, new int2(3, 3), ennemiesFramesize),
                new Actor(actorType.magechief, 3, new int2(0, 4), ennemiesFramesize),
        };

        }


        public void ReadMemory()
        {


            if (TE_Manager.friends.Count > 0)
            {
                foreach (actorType actorType in TE_Manager.friends)
                {
                    int pick = 0;
                    switch (actorType)
                    {
                        case actorType.princess: pick = 0; break;
                        case actorType.soldier: pick = 1; break;
                        case actorType.archer: pick = 2; break;
                        case actorType.mage: pick = 3; break;
                        case actorType.priest: pick = 4; break;
                    }

                    actors_friends.Add(picker_actors_friends[pick]);
                }
            }

            if (TE_Manager.ennemies.Count > 0)
            {
                foreach (actorType actorType in TE_Manager.ennemies)
                {
                    int pick = 0;
                    switch (actorType)
                    {
                        case actorType.kingGolem: pick = 0; break;
                        case actorType.zombie: pick = 1; break;
                        case actorType.wolf: pick = 2; break;
                        case actorType.carnivoreplant: pick = 3; break;
                        case actorType.slim: pick = 4; break;
                        case actorType.golemstone: pick = 5; break;
                        case actorType.golemfire: pick = 6; break;
                        case actorType.golemice: pick = 7; break;

                        case actorType.soldier_bad: pick = 8; break;
                        case actorType.archer_bad: pick = 9; break;
                        case actorType.mage_bad: pick = 10; break;
                        case actorType.priest_bad: pick = 11; break;

                        case actorType.emperor: pick = 12; break;
                        case actorType.queen: pick = 13; break;
                        case actorType.soldiershief: pick = 14; break;
                        case actorType.priestchief: pick = 15; break;
                        case actorType.magechief: pick = 16; break;


                    }

                    actors_ennemies.Add(picker_actors_ennemies[pick]);
                }
            }

            actors_friends.ForEach(x => x.AddEXP(800));
            actors_ennemies.ForEach(x => x.AddEXP(800));

            startFriendsCount = actors_friends.Count;
            startEnemiesCount = actors_ennemies.Count;

        }

        private void LoadSound_Combat(ref ContentManager _content)
        {
            sndCombats = new SoundEffect[8]
            {
                _content.Load<SoundEffect>("Audio\\combat\\GS1_Hit_4"), //sword
                _content.Load<SoundEffect>("Audio\\combat\\GS1_Spell_Thunder"), //tonerre
                _content.Load<SoundEffect>("Audio\\combat\\GS2_Wood_Breaking_2"), //griffe
                _content.Load<SoundEffect>("Audio\\combat\\Spell_Divine_1"), //livre
                _content.Load<SoundEffect>("Audio\\combat\\Magic_Spell_30"), //feu
                _content.Load<SoundEffect>("Audio\\combat\\GS2_Vases_Breaking_1"), //feuille
                _content.Load<SoundEffect>("Audio\\combat\\Magic_Spell_19"), //ice
                _content.Load<SoundEffect>("Audio\\combat\\UI_Swish_2"), //ice

            };
        }

        float chronoLoader = 0;
        Song[] endingSongs;
        public override void Load(ref ContentManager _content)
        {
            frame_cursor = new Rectangle(0, 0, 16, 16);
            maincamera = new TE_Camera( );
            uicamera = new TE_Camera( );
            friendcamera = new TE_Camera( );
            ennemycamera = new TE_Camera( );

            // maincamera.MoveCamera(new Vector2(800, 400));
            LoadTextures(ref _content);
            LoadMusics(ref _content);
            MediaPlayer.Play(bgmusic);
            LoadSounds(ref _content);
            LoadSound_Combat(ref _content);

            SetBattlers();
            SetActionPanel();

           


            statusTex = _content.Load<Texture2D>("statusBar");


            playerLifeBar = ToOffset(new Rectangle(87, 125, 54, 11));
            playerMagicBar = ToOffset(new Rectangle(87, 138, 54, 11));
            playerLifeBar_BG = ToOffset(new Rectangle(87, 125, 54, 11));
            playerMagicBar_BG = ToOffset(new Rectangle(87, 138, 54, 11));


            ennemiLifeBar = ToOffset(new Rectangle(202, 14, 35, 11));
            ennemiMagicBar = ToOffset(new Rectangle(202, 27, 35, 11));
            ennemiLifeBar_BG = ToOffset(new Rectangle(202, 14, 35, 11));
            ennemiMagicBar_BG = ToOffset(new Rectangle(202, 27, 35, 11));

            /* playerCard = ToOffset(new Rectangle(7, 53, 75, 95));
             ennemiCard = ToOffset(new Rectangle(148, 12, 48, 64));*/

             playerCard = ToOffset(new Rectangle(7, 16, 112, 144));
            ennemiCard = ToOffset(new Rectangle(148, 12, 48, 64));

            backgroundPosition = ToOffset(new Rectangle(0, 0, 240, 160));
            playerNamePosition = ToOffset(new Vector2(8, 37));
            enemiNamePosition = ToOffset(new Vector2(68, 13));

            bg_commands_position_vect2 = ToOffset(new Rectangle(148, 89, 84, 60));

            ennemiAction = ToOffset(new Rectangle(109, 29, 24, 24));
            sfxCard = ennemiCard;


           
           


          


            base.Load(ref _content);
        }
        #endregion </LOADING>


        #region <Get Status Informations>
        public void Get_ENNEMY_LifeBar()
        {
            int life = actors_ennemies[currentEnnemyListID].life;
            int width = 35;
            int height = 11;
            float result;
            int maxlife = actors_ennemies[currentEnnemyListID].GetLifeMAXBar();
            result = life;
            result /= maxlife;

            result *= 35;

            if (result < 0) { result = 0; }
            else if (result < 0.1f) result = 4;
            ennemiLifeBar = new Rectangle(ennemiLifeBar.X, ennemiLifeBar.Y, (int)result, height);
        }
        public void Get_ENNEMY_MagicBar()
        {
            int life = actors_ennemies[currentEnnemyListID].magic;
            int width = 35;
            int height = 11;
            float result;
            int maxlife = actors_ennemies[currentEnnemyListID].GetMagicMaxBAR();
            result = life;
            result /= maxlife;

            result *= 35;

            if (result < 0) { result = 0; }
            else if (result < 0.1f) result = 4;
            ennemiMagicBar = new Rectangle(ennemiMagicBar.X, ennemiMagicBar.Y, (int)result, height);
        }
        public void Get_Friend_LifeBar()
        {
            int life = actors_friends[currentFriendListID].life;
            int height = 11;
            float result;
            int maxlife = actors_friends[currentFriendListID].GetLifeMAXBar();
            result = life;
            result /= maxlife;

            result *= 54;

            if (result < 0) { result = 0; }
            else if (result < 0.1f) result = 4;

            Debug.WriteLine("firen life ! " + maxlife + "  : " + result);
            playerLifeBar = new Rectangle(playerLifeBar.X, playerLifeBar.Y, (int)result, height);
        }
        public void Get_Friend_MagicBar()
        {
            int life = actors_friends[currentFriendListID].magic;
            int height = 11;
            float result;
            int maxlife = actors_friends[currentFriendListID].GetMagicMaxBAR();
            result = life;
            result /= maxlife;

            result *= 54;

            if (result < 0) { result = 0; }
            else if (result < 0.1f) result = 4;
            playerMagicBar = new Rectangle(playerMagicBar.X, playerMagicBar.Y, (int)result, height);
        }
        #endregion </Get Status Informations>


        public Rectangle GetFrame_SFX(SFX_style style)
        {
            int frameX = 0;
            int framesize = 48;
            switch (style)
            {
                case SFX_style.sword: frameX = 0; break;
                case SFX_style.fire: frameX = 3; break;
                case SFX_style.ice: frameX = 1; break;
                case SFX_style.thunder: frameX = 4; break;
                case SFX_style.fistofryu: frameX = 7; break;
                case SFX_style.invoke: frameX = 8; break;
                case SFX_style.earth: frameX = 6; break;
                case SFX_style.arrow: frameX = 9; break;

                default: return Rectangle.Empty; // échec et crane
            }
            return new Rectangle(frameX* framesize, 0, 48, 64);
        }


        SoundEffect[] sndCombats;

        public int nextFriendAttadked_by_ennemi_tick = 0;
        public int IA_state = 0;


        scene nextscene;
        private void Exit_Scene()
        {
            //-- on quitte la scene pour une nouvelle
           /** chronoNext += 0.015f;
            alphaALL -= 0.015f;
            if (alphaALL <= 0) { alphaALL = 0; }

            if (chronoNext >= 3.5f)
            {
                main.ChangeScene(scene.standardFight);
            }
           
            return;*/


            if (nextscene == scene.refuge && chronoNext==0)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(endingSongs[0]);
            }
            else if (chronoNext == 0)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(endingSongs[1]);
            }


            chronoNext += 0.015f;
            alphaALL -= 0.015f;
            if (alphaALL <= 0) { alphaALL = 0; }

            if (chronoNext >= 3.5f)
            {
                main.ChangeScene(nextscene);
            }
        }

        bool isloading = true;
        float cleanerTime = 0;
        public override void Update()
        {
            //-- <fin de partie>
            if (shutdownCombat)
            {
                Exit_Scene();
                return;
            }
            //-end- </fin de partie>

            //-- <charger les combattants>
            if (isloading)
            {
                while (true)
                {
                    chronoLoader += 0.025f;

                    if (chronoLoader > 5)
                    {
                        ReadMemory(); 
                        isloading = false;
                        break;
                    }
                }
            }
            //-end- </charger les combattants>

            MouseState mousestate = Mouse.GetState();
            Point mouseposition = mousestate.Position;
            var isleftclicked = mousestate.LeftButton == ButtonState.Pressed;

            UIcursorPos = uicamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();

            CleanAllActors();

            #region <IA Ennemies group>
            if (isEnnemyTurn)
            {
                if (IA_state <= 1)
                {
                    chronoAutoAction += 0.02f;

                    alpha_ENNEMI_SFX -= 0.015f;
                    if(alpha_ENNEMI_SFX < 0) alpha_ENNEMI_SFX = 0;
                }

                if (chronoAutoAction >= 1.0f     //-- choisir au hasard un ami
                    && IA_state == 0
                    && !isQuitEnnemyTurn)
                {
                    alpha_ENNEMI_SFX = 0;
                    hitterShowState = 0;
                    IA_state = 1;
                    //-- sélectionner au hasard, une cible (mes soldats) --
                    if (actors_friends != null)
                    {
                        if (actors_friends.Count > 0)
                        {
                            if (actors_friends.Count >= 1)
                            {
                                int randtarget = Randomizer.GiveRandomInt(0, actors_friends.Count - 1);
                                currentFriendListID = randtarget;
                                CleanAllActors();
                            }
                            else
                            {
                                currentFriendListID = 0;
                                CleanAllActors();
                            }
                        }
                    }
                }
                  else  if (chronoAutoAction >= 2.0f  //-- start roulette
                    && IA_state == 1 
                    && !isQuitEnnemyTurn)
                {

                    IA_state = 2;
                    currentEnnemyListID = nextEnnemiID;
                    
                    currentsf_style = SFX_style.empty;
                    alpha_playerSFX = 0;
                    chronoAutoAction = 0;
                    chronoWheel = 0.0f;
                    
                    //-- démarrer la roulette
                    startWheel = true;
                    alpha_ENNEMI_SFX = 0;
                    return;
                }

                else if (chronoAutoAction >= 1.5f //-- bye bye mode
                    && isQuitEnnemyTurn)
                {
                    //-- quitter le mode ennemi
                    chronoAutoAction = 0;
                    isEnnemyTurn = false;
                    isQuitEnnemyTurn = true;
                    currentsf_style = SFX_style.empty;
                    switchfriendState = 10;
                    canChangeActor = false;
                    chronoChange = 0;
                    chronoWheel = 0;

                    HIT_ennemis = string.Empty;
                    HIT_player = string.Empty;
                }

                else if (chronoAutoAction >= randomEndWheel_Ennemy
                    && IA_state < 4)
                {
                    //-- arrêter la roulette russe --
                    startWheel = false;
                    chronoAutoAction = 0;
                }

                if (IA_state == 4)
                {
                    //-- 2 missions :
                    //-- * 1 * sélectionner prochain ennemi
                    //-- * 2 * on conitue sur le même mode ou pas?

                    chronoAutoAction += 0.02f;

                    if (chronoAutoAction > 1.0f)
                    {
                        chronoAutoAction = 0;
                        IA_state = 0;

                        //-- choisir prochain ennemi --
                        int temp = currentEnnemyListID;
                        temp++;

                        if (temp >= actors_ennemies.Count - 1)
                        {
                            //-- fin de course : pas d'ennemis de disponible
                            temp = 0;
                            nextEnnemiID = temp;
                            isQuitEnnemyTurn = true;

                            sfxCard = ennemiCard;
                            currentFriendListID = 0;
                            currentframeX_btn_wheel = 0;
                            chronoAutoAction = 0;
                            IA_state = 0;

                            canChangeActor = false;
                            startWheel = false;
                        }
                        else
                        {
                            nextEnnemiID = temp;
                            isQuitEnnemyTurn = false;
                            IA_state = 0;
                            chronoAutoAction = 0;
                        }
                    }
                }

                if (startWheel)
                {
                    //-- Jouer l'animation de la roulette russe des actions --
                    wheelhitter++;
                    chronoWheel += speedwheel;
                    
                    //-- ce trigger commande l'arrêt de la roulette russe
                    chronoAutoAction += 0.045f;

                    alpha_playerSFX = 0.0f;

                    if (chronoWheel >= 3.0f)
                    {
                        chronoWheel = 0.0f;
                        int temp = currentEnnemyFrame_btn_Wheel;

                       while(true)
                        { 
                            if(temp!=oldEnemyFrame_btn_wheel)
                            {
                                currentEnnemyFrame_btn_Wheel = temp;
                                break;
                            }
                            else
                            {
                                temp = Randomizer.GiveRandomInt(0, 10);
                            }
                        }

                        oldEnemyFrame_btn_wheel = temp;

                        PlayAudio(snd_iswheeling, 0.2f);
                    }
                }

                else if (!startWheel 
                    && IA_state ==2 
                    & !isQuitEnnemyTurn)
                {
                    chronoAutoAction = 0;
                    randomEndWheel_Ennemy = Randomizer.GiveRandomInt(2, 5);
                    Attack_Action(ref currentEnnemyFrame_btn_Wheel,true);
                    
                    switch (currentEnnemyFrame_btn_Wheel)
                    {
                        case 0:
                            currentsf_style = SFX_style.sword;
                            break;
                        case 1:
                            currentsf_style = SFX_style.ice; 
                            break;
                        case 2:
                            currentsf_style = SFX_style.mouse;
                            break;                       
                        case 3:
                            currentsf_style = SFX_style.fire;        
                            break;
                        case 4:
                            currentsf_style = SFX_style.thunder; 
                            break;
                        case 5:
                            currentsf_style = SFX_style.skull; 
                            break;
                        case 6:
                            currentsf_style = SFX_style.earth;
                            break;
                        case 7:
                            currentsf_style = SFX_style.fistofryu;
                            break;
                        case 8:
                            currentsf_style = SFX_style.invoke;
                            break;
                        case 9:
                            currentsf_style = SFX_style.arrow;
                            break;

                        default:
                            currentsf_style = SFX_style.mouse;
                            break;
                    }

                    ennemiFrameAction = new Rectangle(currentframeX_btn_wheel*0, 0, 24, 24);

                    IA_state = 4;
                }

                sfxCard = playerCard;

                return;
            }

            #endregion <IA Ennemies group>

            /*
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             */


            else
            {

                #region <transitions sfx>
                if (switchfriendState == 1)
                {
                    alpha_friend -= 0.35f / 4;
                    if (alpha_friend < 0)
                    {
                        HIT_ennemis = string.Empty;
                        HIT_player = string.Empty;
                        alpha_friend = 0;
                        switchfriendState = 2;
                        currentFriendListID = nextactorID;

                        CleanAllActors();
                        chronoChange = 0;
                    }
                    return;
                }
                if (switchfriendState == 2)
                {


                    alpha_friend += 0.35f / 4;

                    if (alpha_friend > 1)
                    {
                        HIT_ennemis = string.Empty;
                        HIT_player = string.Empty;

                        alpha_friend = 1.0f;
                        switchfriendState = 0;

                          if(nextactorID == 0)
                        {
                            isEnnemyTurn = true;
                            return;
                        }

                        //-- coisir un ennemi au hasard --

                        if (actors_ennemies.Count > 0)
                        {
                            if (actors_ennemies.Count != 1)
                            {
                                int temp = Randomizer.GiveRandomInt(0, actors_ennemies.Count - 1);
                                currentEnnemyListID = temp;
                                CleanAllActors();
                            }
                            else
                            {
                                currentEnnemyListID = 0;
                                CleanAllActors();
                            }
                        }
                    }

                    alpha_playerSFX -= 0.35f / 3;
                    if (alpha_playerSFX < 0.0f) alpha_playerSFX = 0.0f;
                    return;
                }

                if (switchfriendState == 10)
                {
                    alpha_friend -= 0.35f / 4;
                    if (alpha_friend < 0)
                    {
                        alpha_friend = 0;
                        switchfriendState = 20;
                        currentFriendListID = 0;
                    }


                    return;
                }
                if (switchfriendState == 20)
                {
                    alpha_friend += 0.35f / 4;

                    if (alpha_friend > 1)
                    {
                        HIT_ennemis = string.Empty;
                        HIT_player = string.Empty;
                        alpha_friend = 1.0f;
                        switchfriendState = 0;
                        mouseticks = 0;
                        wheelhitter = 0;
                        canChangeActor = false;

                        IA_state = 0;
                        chronoAutoAction = 0;
                        isQuitEnnemyTurn = false;
                        chronoWheel = 0;
                    }

                    alpha_playerSFX -= 0.35f / 3;
                    if (alpha_playerSFX < 0.0f) alpha_playerSFX = 0.0f;

                    return;
                }
                #endregion </transitions sfx>

                if(canChangeActor && !startWheel)
                {
                   GetNextPlayer();
                    return;
                }

                ReadInputsPlayer(ref UIcursorPos, ref isleftclicked);

                if (startWheel)
                {
                    WheelPlayer();
                }

            }
            base.Update();
        }


        private void WheelPlayer()
        {
            //-- la roulette roune
            chronoWheel += speedwheel;

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

            if (atk_btn_ticks >= 30)
            {
                SetActions(ref currentsf_style);
                canChangeActor = true;
                startWheel = false;
                atk_btn_ticks = 0;
            }

            sfxCard = ennemiCard;
        }

        private void GetNextPlayer()
        {
            //-- on a finit avec la roulette
            //-- on change de player
            chronoChange += 0.035f;

            if (chronoChange >= 4.0f)
            {
                hitterShowState = 0;

                ///chronoChange = 0;
                canChangeActor = false;
                //-- choisir prochain player

                int temp = currentFriendListID;
                int counter = 0;

                while (true)
                {
                    temp++;

                    if (temp >= actors_friends.Count)
                    {
                        //-- fin de liste atteinte :
                        temp = 0;
                        // au tour des ennemis :
                        nextactorID = 0;
                        nextEnnemiID = 0;
                        currentEnnemyListID = 0;
                        isEnnemyTurn = true;
                        sfxCard = playerCard;
                        canChangeActor = false;
                        startWheel = true;
                        break;
                    }

                    else if (actors_friends[temp].life > 0)
                    {
                        break;
                    }

                    else if (counter >= actors_friends.Count)
                    {

                        break;
                    }
                    counter++;
                }

                nextactorID = temp;

                //-- effet de transition 
                switchfriendState = 1;
            }
        }

        int keyboardticks = 0;
        private void ReadInputsPlayer(ref Point mouseposition, ref bool isclicked)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && keyboardticks == 0)
            {
                if (wheelhitter >= 4)
                {
                    SetActions(ref currentsf_style);
                    canChangeActor = true;
                    startWheel = false;
                    atk_btn_ticks = 0;
                }
                else
                {
                    keyboardticks++;
                    atk_btn_ticks = 0;
                    startWheel = true;
                    mouseticks++;
                    wheelhitter = 4;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space) && keyboardticks > 0)
            {
                keyboardticks=0;
                mouseticks = 0;
            }

            if (!isclicked) { mouseticks = 0; }

            if (btn_actionWheel.IsCollide(mouseposition))
            {
                btn_actionWheel.drawrect.color = Color.DarkOrange;

                if (isclicked && mouseticks == 0 && !canChangeActor)
                {
                    atk_btn_ticks = 0;
                    startWheel = true;
                    mouseticks++;

                    if (wheelhitter >= 2)
                    {
                        SetActions(ref currentsf_style);
                        canChangeActor = true;
                        startWheel = false;
                        atk_btn_ticks = 0;
                    }
                    wheelhitter++;

                }
            }
            else
            {

                btn_actionWheel.drawrect.color = Color.White;
            }
        }


        #region <Player Attack System>

        private void FailAttack()
        {
            currentsf_style = SFX_style.empty;
            PlayAudio(snd_badresult);
            wheelhitter = 0;
        }

        int startFriendsCount, startEnemiesCount;

        private void SetActions(ref SFX_style _sfx)
        {

            switch (currentframeX_btn_wheel)
            {
                case 0: _sfx = SFX_style.sword; break;
                case 1: _sfx = SFX_style.ice; break;
                case 2: _sfx = SFX_style.mouse; break;
                case 3: _sfx = SFX_style.fire; break;
                case 4: _sfx = SFX_style.thunder; break;
                case 5: _sfx = SFX_style.skull; break;
                case 6: _sfx = SFX_style.earth; break;
                case 7: _sfx = SFX_style.fistofryu; break;
                case 8: _sfx = SFX_style.invoke; break;
                case 9: _sfx = SFX_style.arrow; break;
                default: break;
            }

            Attack_Action(ref currentframeX_btn_wheel, false);

        }

        int currentEnnemyFrame_btn_Wheel = 0;
        int oldEnemyFrame_btn_wheel = 0;
        int oldFrame_btn_Wheel = 0;
        string HIT_player = string.Empty, HIT_ennemis=string.Empty;
        int hitterShowState = 0;
        private void Attack_Action(ref int wantedAction, bool isEnnemy = false)
        {

            //-- play sound --
            switch (wantedAction)
            {
                case 0: PlayAudio(sndCombats[0]); break; //-- 0 sword
                case 1: PlayAudio(sndCombats[6]); break; //-- 1 Ice
                case 2: FailAttack(); break;             //-- 2 mouse
                case 3: PlayAudio(sndCombats[4]); break; //-- 3 fire
                case 4: PlayAudio(sndCombats[1]); break; //-- 4 thunder
                case 5: FailAttack(); break;             //-- 5 skull
                case 6: PlayAudio(sndCombats[5]); break; //-- 6 earth
                case 7: PlayAudio(sndCombats[2]); break; //-- 7 fistofryu
                case 8: PlayAudio(sndCombats[3]); break; //-- 8 invoke
                case 9: PlayAudio(sndCombats[0]); break; //-- 9 archer
            }

            //-- <jouer cam shake>
            if (!isEnnemy) 
            { alpha_playerSFX = 1.0f;
                TE_Manager.shakeennemy = true;
            }
            else
            { 
                alpha_ENNEMI_SFX = 1.0f;
                TE_Manager.shakefriend = true;
            }
            //-end- <jouer cam shake>

            //-- réinitialise le clavier
            wheelhitter = 0;

            //-- protège une erreur de lecture des listes
            if (currentEnnemyListID >= actors_ennemies.Count)
            {
                currentEnnemyListID = actors_ennemies.Count - 1;
            }

            if (currentFriendListID >= actors_friends.Count)
            {
                currentFriendListID = actors_ennemies.Count - 1;
            }


            HIT_player = string.Empty;
            HIT_ennemis = string.Empty;

            /* CleanAllActors();

             if (actors_ennemies.Count == 0)
             {
                 CleanAllActors();
                 return;
             }
             if (actors_friends.Count == 0)
             {
                 CleanAllActors();
                 return;
             }*/

            actorType actor_caster = actorType.soldier;
            SFX_style spellCast = currentsf_style;
            int cost_castMagic = 0;
            int qty_totalMagic = 0;
            int hitPts=0;

            if(wantedAction == 2 ||wantedAction == 5)
            {
                //-- attaque ratée --
                FailAttack();
            }
            else if (wantedAction != 0 && wantedAction != 9)
            {
                //-- attaque magique --
                if (!isEnnemy)
                {
                    actor_caster = actors_friends[currentFriendListID].GetActorTYpe();
                    cost_castMagic = ActorsStatesRules.Get_MAGIC_cost(actor_caster);
                    qty_totalMagic = actors_friends[currentFriendListID].magic;
                }
                else
                {
                    actor_caster = actors_ennemies[currentEnnemyListID].GetActorTYpe();
                    cost_castMagic = ActorsStatesRules.Get_MAGIC_cost(actor_caster);
                    qty_totalMagic = actors_ennemies[currentEnnemyListID].magic;
                }

                int result = qty_totalMagic - cost_castMagic;

                if (result < 0)
                {

                    FailAttack();
                    wantedAction = 0;
                    return;
                }

                if (!isEnnemy)
                {
                    actors_friends[currentFriendListID].magic = result;
                     hitPts = ActorsStatesRules.GetAtk_MAGIC_degats(spellCast);
                    actors_ennemies[currentEnnemyListID].Hitted(ref hitPts);
                }
                else
                {
                    actors_ennemies[currentEnnemyListID].magic = result;
                     hitPts = ActorsStatesRules.GetAtk_MAGIC_degats(spellCast);
                    actors_friends[currentFriendListID].Hitted(ref hitPts);
                }
            }
            else if (wantedAction == 0)
            {
                if (!isEnnemy)
                {
                    actorType at = actors_friends[currentFriendListID].GetActorTYpe();
                     hitPts = ActorsStatesRules.GetAtk_melee_degats(at);
                    actors_ennemies[currentEnnemyListID].Hitted(ref hitPts);
               }
                else
                {
                    actorType at = actors_ennemies[currentEnnemyListID].GetActorTYpe();
                     hitPts = ActorsStatesRules.GetAtk_melee_degats(at);
                    actors_friends[currentFriendListID].Hitted(ref hitPts);
                }
            }
            else 
            {
                if (!isEnnemy)
                {
                    actorType at = actors_friends[currentFriendListID].type;
                     hitPts = ActorsStatesRules.GetAtk_melee_degats(at);
                    actors_ennemies[currentEnnemyListID].Hitted(ref hitPts);
                }
                else
                {
                    actorType at = actors_ennemies[currentEnnemyListID].type;
                     hitPts = ActorsStatesRules.GetAtk_melee_degats(at);
                    actors_friends[currentFriendListID].Hitted(ref hitPts);
                }
            }

            hitterShowState = 1;
            if(isEnnemy)
            {
                HIT_ennemis = " - "+hitPts.ToString();
                HIT_player = string.Empty;
            }
            else
            {
                HIT_player = " - " + hitPts.ToString();
                HIT_ennemis = string.Empty;

            }



            if (actors_friends[currentFriendListID].life<=0)
            {
                CleanAllActors();
            }
            if (actors_ennemies[currentEnnemyListID].life <= 0)
            {
                CleanAllActors();
            }



        }

        public void UpdateStats()
        {
            Get_ENNEMY_LifeBar();
            Get_ENNEMY_MagicBar();
            Get_Friend_LifeBar();
            Get_Friend_MagicBar();
        }
        #endregion </Player Attack System>

        public override void Draw(ref SpriteBatch _sp)
        {
            _sp.Draw(backgrounds[0], backgroundPosition, Color.White * alphaALL);

            if (actors_friends != null)
            {
                if (actors_friends.Count > 0)
                {
                    _sp.DrawString(cutsceneFont, ActorsStatesRules.Get_Name(actors_friends[currentFriendListID].GetActorTYpe()), playerNamePosition, Color.White * alphaALL);
                }
                _sp.Draw(statusTex, playerLifeBar_BG, Color.DarkGray * alphaALL);
                _sp.Draw(statusTex, playerMagicBar_BG, Color.DarkGray * alphaALL);
            }

            if (actors_ennemies != null)
            {
                if (actors_ennemies.Count > 0)
                {
                    _sp.DrawString(cutsceneFont, ActorsStatesRules.Get_Name(actors_ennemies[currentEnnemyListID].GetActorTYpe()), enemiNamePosition, Color.White * alphaALL);

                }
                _sp.Draw(statusTex, ennemiLifeBar_BG, Color.DarkGray * alphaALL);
                _sp.Draw(statusTex, ennemiMagicBar_BG, Color.DarkGray * alphaALL);
            }


            base.Draw(ref _sp);
        }





        public override void Draw_Friend(ref SpriteBatch _spUI)
        {
            if (actors_friends != null)
            {
                if (actors_friends.Count > 0)
                {
                    _spUI.Draw(textures[actors_friends[currentFriendListID].textureID],
               playerCard,
               actors_friends[currentFriendListID].frame,
               Color.White * alpha_friend * alphaALL);
                }
            }

            base.Draw_Friend(ref _spUI);
        }

        public override void Draw_Ennemy(ref SpriteBatch _spUI)
        {
            if (actors_ennemies != null)
            {
                if (actors_ennemies.Count > 0)
                {
                    _spUI.Draw(textures[3],
               ennemiCard,
               actors_ennemies[currentEnnemyListID].frame,
               Color.White * alphaALL);
                }
            }



            base.Draw_Ennemy(ref _spUI);
        }


        bool shutdownCombat = false;
        float alpha_ENNEMI_SFX=0.0f;
        public override void Draw_UI(ref SpriteBatch _spUI)
        {

            if (actors_ennemies == null || actors_friends == null) return;

            //-- bouton d'action player
            _spUI.Draw(textures[0],
                btn_actionWheel.drawrect.position,
                new Rectangle(currentframeX_btn_wheel * 24, 0, 24, 24),
                btn_actionWheel.drawrect.color * alphaALL);
           
            //-- bouton action des ennemis
            _spUI.Draw(textures[0], 
                ennemiAction,
                new Rectangle(currentEnnemyFrame_btn_Wheel * 24, 0, 24, 24),
                Color.White * alphaALL);



            //-- sfxEnnemi
            _spUI.Draw(SFX, ennemiCard,
            new Rectangle(currentframeX_btn_wheel * 48, 0, 48, 64), 
                Color.White * alpha_playerSFX * alphaALL,
                0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.5f);

            //-- sfxplayer
            _spUI.Draw(SFX, playerCard,
                new Rectangle(currentEnnemyFrame_btn_Wheel * 48, 0, 48, 64),
                Color.White * alpha_ENNEMI_SFX * alphaALL,
                0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.5f) ;

            //-- life and magic points
            _spUI.Draw(statusTex, playerLifeBar, Color.Red * alphaALL);
            _spUI.Draw(statusTex, playerMagicBar, Color.Blue * alphaALL);

            _spUI.Draw(statusTex, ennemiLifeBar, Color.Red * alphaALL);
            _spUI.Draw(statusTex, ennemiMagicBar, Color.Blue * alphaALL);

            //-- afficher les points de dégâts
            if (hitterShowState == 1)
            {
                _spUI.DrawString(mainFont, HIT_ennemis,ToOffset( new Vector2(61, 73)), Color.Red* alphaALL,
                     0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);
                _spUI.DrawString(mainFont, HIT_player, ToOffset( new Vector2(138, 27)), Color.Red * alphaALL,
                     0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);
            }

            _spUI.DrawString(cutsceneFont, "/ " + startFriendsCount.ToString(),ToOffset(new Vector2(100,110)), Color.Orange * alphaALL);
            _spUI.DrawString(cutsceneFont, "/ " + startEnemiesCount.ToString(), ToOffset(new Vector2(216, 3)), Color.Orange * alphaALL);

            _spUI.DrawString(cutsceneFont, actors_friends.Count.ToString(), ToOffset(new Vector2(90, 110)), Color.Orange * alphaALL);
            _spUI.DrawString(cutsceneFont, actors_ennemies.Count.ToString(), ToOffset(new Vector2(204, 3)), Color.Orange * alphaALL);


            if (cursor == null) return;
            Rectangle mousecursorPosition = new Rectangle(UIcursorPos.X, UIcursorPos.Y, 20, 20);
            _spUI.Draw(cursor, mousecursorPosition, frame_cursor, Color.White * alphaALL);
            base.Draw_UI(ref _spUI);
        }

    }
}
