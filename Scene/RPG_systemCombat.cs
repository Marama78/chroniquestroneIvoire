using CTI_RPG.CombatSystem;
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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using CTI_RPG;

namespace CTI_RPG.Scene
{
    public class KeyboardInput
    {
        public KeyboardInput(Keys keys, string name)
        {
            _Keys = keys;
            _Name = name;
        }

        public Keys _Keys { get; set; }
        public string _Name { get; set; }

    }

    public class KeyboardManager
    {
        public KeyboardInput[] _inputs;

        public Keys GetKey(string name)
        {
            if(_inputs == null) return Keys.None;

            for (int i = 0; i < _inputs.Length; i++)
            {
                if (_inputs[i]._Name == name)
                {
                    return _inputs[i]._Keys;
                }
            }

            return Keys.None;
        }
    }
    public class RPG_systemCombat : ModelScene
    {
        Song bgmusic;
        Texture2D[] textures;
        Texture2D[] backgrounds;
        Texture2D[] portraits_combattants;

        Texture2D statusTex;
        Texture2D SFX;
        Texture2D texennemiaction;

        KeyboardManager keyboardmanager;

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
        SoundEffect[] snd_SFX;
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

        int ennemiNextID = -10, ennemyPreviousID = -10;


        int startFriendsCount, startEnemiesCount;


        int currentEnnemyFrame_btn_Wheel = 0;
        int oldEnemyFrame_btn_wheel = 0;
        int oldFrame_btn_Wheel = 0;
        string HIT_player = string.Empty, HIT_ennemis = string.Empty;
        int hitterShowState = 0;



        Rectangle ennemiAction, ennemiFrameAction;
        int oldFriendID = 0;
        int nextEnnemiID = 0;
        int stopAction = 7;
        float chronoAutoAction = 0.0f;
        int randomEndWheel_Ennemy = 4
            ;
        float alphaactionennemi = 0.0f;
        int atk_btn_ticks = 0;

        bool isQuitEnnemyTurn = false;
        float chronoNext = 0;
        float alphaALL = 1.0f;

        TE_Button[] btn_combatPanel;

        List<int> openlistActor = new List<int>();
        int ennemi_team_state = 0;
        int randFriend = 0;

        float chronoIA_EnnemiState = 0.0f;
        bool hasPlayerLaunchAttack = false;
        private void CleanAllActors()
        {
            if (mouseticks > 0) return;
            actors_ennemies.RemoveAll(x => x.HP <= 0);
            actors_friends.RemoveAll(x => x.HP <= 0);

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
            if (currentEnnemyListID >= actors_ennemies.Count)
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

        public RPG_systemCombat(MainClass _mainclass) : base(_mainclass)
        {

        }

        #region <LOADING>

        Rectangle[] cursors;
        private void SetMouseCursor()
        {
            //  frame_cursor = new Rectangle(0, 0, 8, 8);
            cursors = new Rectangle[]
            {
                new Rectangle(0, 0, 16, 16),
                new Rectangle(16, 0, 16, 16),
                new Rectangle(0, 16, 16, 16),
                new Rectangle(16, 16, 16, 16),
                new Rectangle(0, 32, 16, 16),
            };
        }
        private void LoadSounds(ref ContentManager _content)
        {
            snd_iswheeling = _content.Load<SoundEffect>("Audio\\wheelaction\\UI2_Button_7");
            snd_badresult = _content.Load<SoundEffect>("Audio\\wheelaction\\UI2_Decline_4");
            snd_good_result = _content.Load<SoundEffect>("Audio\\wheelaction\\UI2_Trophy_2");

            snd_SFX = new SoundEffect[]
                {
                _content.Load<SoundEffect>("Audio\\combat\\UI_Swish_2"), //-- 0 - selectionner adversaire
                _content.Load<SoundEffect>("Audio\\UI2_Button_2"), //-- 1 - bouton de commande panneau principal
                _content.Load<SoundEffect>("Audio\\UI2_Accept_1"), //-- 2 - 'VALIDER' phase1
                _content.Load<SoundEffect>("Audio\\UI2_Click_3"), //-- 3 - 'VALIDER' phase1
                _content.Load<SoundEffect>("Audio\\combat\\Spell_Healing_2"), //-- 4 - mainpanel snd
                _content.Load<SoundEffect>("Audio\\Spell_Casting_3"), //-- 5 - mainpanel snd
        };

        }

        private void LoadTextures(ref ContentManager _content)
        {
            textures = new Texture2D[]
            {
                _content.Load<Texture2D>("system\\actions"), //-- 0 action
                _content.Load<Texture2D>("units\\battle\\unitsfriends"), //-- 1 unités amies
                _content.Load<Texture2D>("units\\battle\\badguys"), //-- 2 unités ennemies définitive
                _content.Load<Texture2D>("plateau\\plateaugreen"), //-- 3 plateau jeu
                _content.Load<Texture2D>("system\\outline_btn"), //-- 4 outline aux dimensions des boutons
                _content.Load<Texture2D>("system\\outline_btn_atk"), //-- 5 outline attaques aux dimensions des boutons
            };

            backgrounds = new Texture2D[]
            {
                _content.Load<Texture2D>("units\\battle\\backgrounds\\bg_battle1"),
            };

            portraits_combattants = new Texture2D[]
            {
                _content.Load<Texture2D>("plateau\\ActorsPortraits\\princess"),
                _content.Load<Texture2D>("plateau\\ActorsPortraits\\soldier"),
                _content.Load<Texture2D>("plateau\\ActorsPortraits\\archer"),
                _content.Load<Texture2D>("plateau\\ActorsPortraits\\mage"),
                _content.Load<Texture2D>("plateau\\ActorsPortraits\\priest"),
            };

            cursor = _content.Load<Texture2D>("system\\cursor");
            SFX = _content.Load<Texture2D>("units\\battle\\fxatk");
            statusTex = _content.Load<Texture2D>("statusBar");
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
            actors_friends = new List<Actor>(); ;


            int2 myunitFramesize = new int2(75, 95);
            int2 ennemiesFramesize = new int2(48, 64);


            picker_actors_friends = new Actor[5]
            {
                new Actor(actorType.princess, 1, new int2(0, 0), myunitFramesize,0),
                new Actor(actorType.soldier, 1, new int2(1, 0), myunitFramesize, 1),
                new Actor(actorType.archer, 1, new int2(2, 0), myunitFramesize, 2),
                new Actor(actorType.mage, 1, new int2(3, 0), myunitFramesize, 3),
                new Actor(actorType.priest, 1, new int2(4, 0), myunitFramesize,4),
            };

            picker_actors_ennemies = new Actor[]
            {
                new Actor(actorType.kingGolem, 2, new int2(0, 0), ennemiesFramesize),
                new Actor(actorType.zombie, 2, new int2(1, 0), ennemiesFramesize),
                new Actor(actorType.wolf, 2, new int2(2, 0), ennemiesFramesize),
                new Actor(actorType.carnivoreplant, 2, new int2(3, 0), ennemiesFramesize),
                new Actor(actorType.slim, 2, new int2(0, 1), ennemiesFramesize),
                new Actor(actorType.golemstone, 2, new int2(1, 1), ennemiesFramesize),
                new Actor(actorType.golemfire, 2, new int2(2, 1), ennemiesFramesize),
                new Actor(actorType.golemice, 2, new int2(3, 1), ennemiesFramesize),

                new Actor(actorType.soldier_bad, 2, new int2(0, 2), ennemiesFramesize),
                new Actor(actorType.archer_bad, 2, new int2(1, 2), ennemiesFramesize),
                new Actor(actorType.mage_bad, 2, new int2(2, 2), ennemiesFramesize),
                new Actor(actorType.priest_bad, 2, new int2(3, 2), ennemiesFramesize),

                new Actor(actorType.emperor, 2, new int2(0, 3), ennemiesFramesize),
                new Actor(actorType.queen, 2, new int2(1, 3), ennemiesFramesize),
                new Actor(actorType.soldiershief, 2, new int2(2, 3), ennemiesFramesize),
                new Actor(actorType.priestchief, 2, new int2(3, 3), ennemiesFramesize),
                new Actor(actorType.magechief, 2, new int2(0, 4), ennemiesFramesize),
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

                    Actor temp = new Actor(actorType,
                      picker_actors_friends[pick].textureID,
                      picker_actors_friends[pick].frameposition,
                      picker_actors_friends[pick].frameSize,
                      picker_actors_friends[pick].textur2D_portraitID,
                      picker_actors_friends[pick].level);


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
                    Actor temp = new Actor(actorType,
                        picker_actors_ennemies[pick].textureID,
                        picker_actors_ennemies[pick].frameposition,
                        picker_actors_ennemies[pick].frameSize);

                    actors_ennemies.Add(temp);
                }
            }

            while(true)
            {
                actors_friends.ForEach(x => x.UpdateLevel(5));
                actors_ennemies.ForEach(x => x.UpdateLevel(2));
                break;
            }
         

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

        int UI_enemy_lifebarwidth_Maximum;
        private void SetupLifeAllLifeBarPosition()
        {
            UI_lifebarwidth_Maximum = 80 - 20;
            UI_enemy_lifebarwidth_Maximum = 60;

            playerLifeBar = ToOffset(new Rectangle(120 + 30, 108, UI_lifebarwidth_Maximum, 6));
            playerLifeBar_BG = new Rectangle(playerLifeBar.X - 1, playerLifeBar.Y - 1, UI_lifebarwidth_Maximum + 2, 6 + 2);

            ennemiLifeBar = ToOffset(new Rectangle(23 + 30, 20, UI_enemy_lifebarwidth_Maximum, 6));
            ennemiLifeBar_BG = new Rectangle(ennemiLifeBar.X - 1, ennemiLifeBar.Y - 1, UI_enemy_lifebarwidth_Maximum + 2, 6 + 2);

        }

        Rectangle bgCommandPanel;
        Rectangle bgPlateauEnnemi, bgPlateauPlayer;
        Rectangle bgPlateauEnnemi_previous, bgPlateauEnnemi_next;
        Rectangle bgPlateauEnnemi_previous_default, bgPlateauEnnemi_next_default;
        Rectangle bgPlateauEnnemi_previous_fx1, bgPlateauEnnemi_next_fx1;
        Rectangle bgPlateauEnnemi_previous_fx2, bgPlateauEnnemi_next_fx2;
        Rectangle bgPlateauEnnemi_previous_fx3, bgPlateauEnnemi_next_fx3;
        Rectangle bgstatsEnnemi, bgstatsPlayer;

        Rectangle ennemicard_previous, ennemicard_next;
        Color col_enemi_next = Color.DarkGray;
        Color col_enemi_previous = Color.DarkGray;

        Rectangle ennemicard_DEFAULT;

        Rectangle mainscreen_switchMode_panel;
        private void SetupUI()
        {
            mainscreen_switchMode_panel = ToOffset(new Rectangle(0,80,240,40));

            playerCard = ToOffset(new Rectangle(7, 30, 75, 95));
            ennemiCard = ToOffset(new Rectangle(152, 12, 48, 64));
            ennemicard_DEFAULT = ennemiCard;

            backgroundPosition = ToOffset(new Rectangle(0, 0, 240, 160));

            playerNamePosition = new Vector2(playerLifeBar_BG.X + 4 - 30, playerLifeBar_BG.Y - 9);
            enemiNamePosition = new Vector2(ennemiLifeBar_BG.X + 4 - 30, ennemiLifeBar_BG.Y - 9);

            bg_commands_position_vect2 = ToOffset(new Rectangle(148, 89, 84, 60));

            ennemiAction = ToOffset(new Rectangle(109, 29, 24, 24));

            //-- partie action
            bgCommandPanel = ToOffset(new Rectangle(0, 123, 240, 160));

            bgstatsEnnemi = new Rectangle((int)enemiNamePosition.X - 6, (int)enemiNamePosition.Y - 4, 100, 24);
            bgstatsPlayer = new Rectangle((int)playerNamePosition.X - 6, (int)playerNamePosition.Y - 4, 100, 24);

            //-- plateau des actors
            bgPlateauEnnemi =
                new Rectangle(
                    ennemiCard.X - (int)(ennemiCard.Width * 0.4f),
                    ennemiCard.Y + ennemiCard.Height - (int)(ennemiCard.Height * 0.3f),
                    (int)(ennemiCard.Width * 1.8f),
                    (int)(ennemiCard.Height * 0.5f));

            //--
            bgPlateauEnnemi_previous =
                new Rectangle(
                    (int)(bgPlateauEnnemi.X+ennemiCard.Width*1.3f),
                    bgPlateauEnnemi.Y-10,
                    (int)(bgPlateauEnnemi.Width*0.3f),
                    (int)(bgPlateauEnnemi.Height*0.3f));

            bgPlateauEnnemi_previous_default = bgPlateauEnnemi_previous;


            bgPlateauEnnemi_previous_fx1 =
              new Rectangle(
                  (int)(bgPlateauEnnemi.X + ennemiCard.Width * 1.3f) +10,
                  bgPlateauEnnemi.Y - 10 + 15,
                  (int)(bgPlateauEnnemi.Width * 0.3f),
                  (int)(bgPlateauEnnemi.Height * 0.3f));
            bgPlateauEnnemi_previous_fx2 =
              new Rectangle(
                  (int)(bgPlateauEnnemi.X + ennemiCard.Width * 1.3f) + 20,
                  bgPlateauEnnemi.Y - 10 + 10,
                  (int)(bgPlateauEnnemi.Width * 0.3f),
                  (int)(bgPlateauEnnemi.Height * 0.3f));
            bgPlateauEnnemi_previous_fx3 =
              new Rectangle(
                  (int)(bgPlateauEnnemi.X + ennemiCard.Width * 1.3f)+10,
                  bgPlateauEnnemi.Y - 10 + 5,
                  (int)(bgPlateauEnnemi.Width * 0.3f),
                  (int)(bgPlateauEnnemi.Height * 0.3f));

            bgPlateauEnnemi_next =
               new Rectangle(
                   (int)(bgPlateauEnnemi.X - ennemiCard.Width * 0.3f),
                   bgPlateauEnnemi.Y - 10,
                   (int)(bgPlateauEnnemi.Width * 0.3f),
                   (int)(bgPlateauEnnemi.Height * 0.3f));
            bgPlateauEnnemi_next_default = bgPlateauEnnemi_next;

            bgPlateauEnnemi_next_fx1 =
              new Rectangle(
                  (int)(bgPlateauEnnemi.X - ennemiCard.Width * 0.3f)-10,
                  bgPlateauEnnemi.Y - 10 + 5,
                  (int)(bgPlateauEnnemi.Width * 0.3f),
                  (int)(bgPlateauEnnemi.Height * 0.3f));

            bgPlateauEnnemi_next_fx2 =
              new Rectangle(
                  (int)(bgPlateauEnnemi.X - ennemiCard.Width * 0.3f)-20,
                  bgPlateauEnnemi.Y - 10 + 10,
                  (int)(bgPlateauEnnemi.Width * 0.3f),
                  (int)(bgPlateauEnnemi.Height * 0.3f));

            bgPlateauEnnemi_next_fx3 =
              new Rectangle(
                  (int)(bgPlateauEnnemi.X - ennemiCard.Width * 0.3f)-10,
                  bgPlateauEnnemi.Y - 10 + 15,
                  (int)(bgPlateauEnnemi.Width * 0.3f),
                  (int)(bgPlateauEnnemi.Height * 0.3f));


            ennemicard_next = new Rectangle(
                bgPlateauEnnemi_next.X + (int)(bgPlateauEnnemi_next.Width / 2) - (int)(ennemiCard.Width/4),
                bgPlateauEnnemi_next.Y + (int)(bgPlateauEnnemi_next.Height / 2) - (int)(ennemiCard.Height/2),
                (int)(ennemiCard.Width / 2),
                (int)(ennemiCard.Height / 2));
                ;
            ennemicard_previous = new Rectangle(
                 bgPlateauEnnemi_previous.X + (int)(bgPlateauEnnemi_previous.Width / 2) - (int)(ennemiCard.Width / 4),
                bgPlateauEnnemi_previous.Y + (int)(bgPlateauEnnemi_previous.Height / 2) - (int)(ennemiCard.Height / 2),
                (int)(ennemiCard.Width / 2),
                (int)(ennemiCard.Height / 2));

            bgPlateauPlayer =
               new Rectangle(
                   playerCard.X - (int)(playerCard.Width * 0.2f),
                   playerCard.Y + playerCard.Height - (int)(playerCard.Height * 0.3f),
                   (int)(playerCard.Width * 1.4f),
                   (int)(playerCard.Height * 0.63f));
        }

        TE_Button[] btn_select_ennemi;
        TE_Button[] btn_attaque;
        Rectangle bg_maincommands_panelcombat, bg_maincommands_informations, bg_maincommands_attaquepanel;
        private void Setup_UI_CombatPanelCommands()
        {
            //-*-*- PHASE 1 : sélectionner une cible
            //-- trois boutons
            //-- à gauche, à droite, valider
            //-*-*- PHASE 2 : sélectionner une action
            //-- 4 commandes 
            //-- attaquer
            //-- changer attaquant
            //-- sac
            //-- fuir


            bg_maincommands_panelcombat = ToOffset(new Rectangle(154, 126, 84, 30));
            bg_maincommands_informations = ToOffset(new Rectangle(3, 126, 148, 30));
            //-- 4 atttaques (dépend de la classe du personnage)

            Rectangle temp1 = ToOffset(new Rectangle(156, 128, 37, 11));
            Rectangle temp2 = ToOffset(new Rectangle(199, 128, 37, 11));
            Rectangle temp3 = ToOffset(new Rectangle(156, 128 + 14, 37, 11));
            Rectangle temp4 = ToOffset(new Rectangle(199, 128 + 14, 37, 11));

            DrawRect dr1 = new DrawRect(temp1, 16);
            DrawRect dr2 = new DrawRect(temp2, 16);
            DrawRect dr3 = new DrawRect(temp3, 16);
            DrawRect dr4 = new DrawRect(temp4, 16);

            btn_combatPanel = new TE_Button[]
            {
                new TE_Button(dr1,new int2(0,0),16),
                new TE_Button(dr2,new int2(0,0),16),
                new TE_Button(dr3,new int2(0,0),16),
                new TE_Button(dr4,new int2(0,0),16),
            };

            btn_combatPanel[0].text = "ATTAQUE";
            btn_combatPanel[1].text = "CHANGER";
            btn_combatPanel[2].text = "SAC";
            btn_combatPanel[3].text = "FUIR";

            //----------------------------------------------------

            Rectangle ytemp1 = ToOffset(new Rectangle(6, 128, 60, 11));
            Rectangle ytemp2 = ToOffset(new Rectangle(80, 128, 60, 11));
            Rectangle ytemp3 = ToOffset(new Rectangle(6, 128 + 14, 60, 11));
            Rectangle ytemp4 = ToOffset(new Rectangle(80, 128 + 14, 60, 11));

            DrawRect ydr1 = new DrawRect(ytemp1, 16);
            DrawRect ydr2 = new DrawRect(ytemp2, 16);
            DrawRect ydr3 = new DrawRect(ytemp3, 16);
            DrawRect ydr4 = new DrawRect(ytemp4, 16);

            btn_attaque = new TE_Button[]
            {
                new TE_Button(ydr1,new int2(0,0),16),
                new TE_Button(ydr2,new int2(0,0),16),
                new TE_Button(ydr3,new int2(0,0),16),
                new TE_Button(ydr4,new int2(0,0),16),
            };

            btn_attaque[0].text = "Coup de sabre";
            btn_attaque[1].text = "Cri de guerre";
            btn_attaque[2].text = "Multiple estocs";
            btn_attaque[3].text = "Tornade";

            //----------------------------------------------------

            Rectangle ztemp1 = ToOffset(new Rectangle(156, 128, 37, 11));
            Rectangle ztemp2 = ToOffset(new Rectangle(199, 128, 37, 11));
            Rectangle ztemp3 = ToOffset(new Rectangle(156, 128 + 14, 37, 11));
            Rectangle ztemp4 = ToOffset(new Rectangle(199, 128 + 14, 37, 11));
            DrawRect zdr1 = new DrawRect(ztemp1, 16);
            DrawRect zdr2 = new DrawRect(ztemp2, 16);
            DrawRect zdr3 = new DrawRect(ztemp3, 16);
            DrawRect zdr4 = new DrawRect(ztemp4, 16);

            btn_select_ennemi = new TE_Button[]
            {
                new TE_Button(zdr1,new int2(0,0),16),
                new TE_Button(zdr2,new int2(0,0),16),
                new TE_Button(zdr3,new int2(0,0),16),
                new TE_Button(zdr4,new int2(0,0),16),
            };

            btn_select_ennemi[0].text = "PRECEDENT";
            btn_select_ennemi[1].text = "SUIVANT";
            btn_select_ennemi[2].text = "SAC";
            btn_select_ennemi[3].text = "VALIDER";
        }

        float chronoLoader = 0;
        Song[] endingSongs;

        private void SetupKeyboardManager()
        {
            keyboardmanager = new KeyboardManager();
            keyboardmanager._inputs = new KeyboardInput[]
            {
                new KeyboardInput(Keys.Left,"left"),
                new KeyboardInput(Keys.Right,"right"),
                new KeyboardInput(Keys.Up,"up"),
                new KeyboardInput(Keys.Down,"down"),
                new KeyboardInput(Keys.Space,"valid1"),
                new KeyboardInput(Keys.Enter,"valid2"),

            };
        }

        public override void Load(ref ContentManager _content)
        {
            frame_cursor = new Rectangle(0, 0, 16, 16);
            maincamera = new TE_Camera();
            uicamera = new TE_Camera();
            friendcamera = new TE_Camera();
            ennemycamera = new TE_Camera();

            LoadTextures(ref _content);
            LoadMusics(ref _content);
            MediaPlayer.Play(bgmusic);
            MediaPlayer.IsMuted = true;
            LoadSounds(ref _content);
            LoadSound_Combat(ref _content);

            //-- installation des composants visuels --
            SetBattlers();
            SetActionPanel();
            SetupLifeAllLifeBarPosition();
            SetupUI();
            Setup_UI_CombatPanelCommands();
            SetMouseCursor();
            SetupKeyboardManager();
            SetupMainScreenComponents();
            //-end-

            sfxCard = ennemiCard;

          
            base.Load(ref _content);
        }
        private void SetupMainScreenComponents()
        {
            mainscreen_str = new string[]
            {
                "C'est votre tour!",
                "A l'attaque!",
                "Montrez toute votre rage!",
                "Vos ennemis attaquent!",
                "Patientez, ce n'est pas votre tour",
                "Au tour de vos adveraires",
            };

            mainstring_position_current =ToOffset( new Vector2(-140, 88));
            mainstring_position_default = ToOffset(new Vector2(-140, 88));
            mainstring_position_phase1 = ToOffset(new Vector2(24, 88));
            mainstring_position_phase2 = ToOffset(new Vector2(240, 88));
        }

        private void SetupEnnemies_platforms_and_cards()
        {
            currentEnnemyListID = 0;

            int temp = currentEnnemyListID + 1;

            if (temp > actors_ennemies.Count - 1 && actors_ennemies.Count > 1)
            {
                temp = 0;
            }
            else if (actors_ennemies.Count < 1)
            {
                temp = -100;
            }
            ennemiNextID = temp;

            temp = currentEnnemyListID - 1;

            if (temp < 0 && actors_ennemies.Count > 1)
            {
                temp = actors_ennemies.Count - 1;
            }
            else if (actors_ennemies.Count < 1)
            {
                temp = -100;
            }
            ennemyPreviousID = temp;
        }

        #endregion </LOADING>


        #region <Get Status Informations>
        public void Get_ENNEMY_LifeBar()
        {
            int life = actors_ennemies[currentEnnemyListID].HP;
            int width = 35;
            int height = 11;
            float result;
            int maxlife = actors_ennemies[currentEnnemyListID].MAX_HP;
            result = life;
            result /= maxlife;

            result *= UI_enemy_lifebarwidth_Maximum;

            if (result < 0) { result = 0; }
            else if (result < 0.1f) result = 4;
            ennemiLifeBar = new Rectangle(ennemiLifeBar.X, ennemiLifeBar.Y, (int)result, ennemiLifeBar.Height);
        }
       
        int UI_lifebarwidth_Maximum;

        public void Get_Friend_LifeBar()
        {
            int life = actors_friends[currentFriendListID].HP;
            float result;
            int maxlife = actors_friends[currentFriendListID].MAX_HP;
            result = life;
            result /= maxlife;

            result *= UI_lifebarwidth_Maximum;

            if (result < 0) { result = 0; }
            else if (result < 0.1f) result = 4;

            playerLifeBar = new Rectangle(playerLifeBar.X, playerLifeBar.Y, (int)result, playerLifeBar.Height);
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
            return new Rectangle(frameX * framesize, 0, 48, 64);
        }


        SoundEffect[] sndCombats;

        public int nextFriendAttadked_by_ennemi_tick = 0;
        public int IA_state = 0;


        scene nextscene;
        private void Exit_Scene()
        {
#if DEBUG
            //-- on quitte la scene pour une nouvelle
            chronoNext += 0.015f;
             alphaALL -= 0.015f;
             if (alphaALL <= 0) { alphaALL = 0; }

             if (chronoNext >= 3.5f)
             {
                 main.ChangeScene(scene.standardFight);
             }
            
             return;
#else
            // -DEBUG POUT CYCLE INFINI-
            if (nextscene == scene.refuge && chronoNext == 0)
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
#endif

        }

        private void UpdateSFX()
        {
            if (alpha_playerSFX != 0)
            {
                alpha_playerSFX -= 0.035f;
                if (alpha_playerSFX <= 0)
                {
                    alpha_playerSFX = 0;
                }
            }
            if (alpha_ENNEMI_SFX != 0)
            {
                alpha_ENNEMI_SFX -= 0.035f;
                if (alpha_ENNEMI_SFX <= 0)
                {
                    alpha_ENNEMI_SFX = 0;
                }
            }
        }

        bool isloading = true;
        float cleanerTime = 0;

        float chronoSwitch = 0.0f;

        bool changePlayer = false;
        int changePlayerState = 0;
        float chronoChangePlayer = 0.0f;

        bool isDisplayMainscreenPanel = true;
        string[] mainscreen_str;
        int mainscreen_str_ID = 0;
        Vector2 mainstring_position_current ;
        Vector2 mainstring_position_default ;
        Vector2 mainstring_position_phase1 ;
        Vector2 mainstring_position_phase2 ;

        bool canDraw = false;

        public override void Update()
        {
            UpdateSFX();
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
                        SetupEnnemies_platforms_and_cards();
                        break;
                    }
                }
            }
            //-end- </charger les combattants>

            if (isDisplayMainscreenPanel)
            {
                if(chronoSwitch==0.0f) 
                {
                    if(!isEnnemyTurn)
                    {
                        mainscreen_str_ID = Randomizer.GiveRandomInt(0, 3);
                    }
                    else
                    {
                        mainscreen_str_ID = Randomizer.GiveRandomInt(3, 6);
                    }
                    PlayAudio(snd_SFX[4], 0.5f); 
                }
                chronoSwitch += 0.015f;

                if(chronoSwitch<=2.0f)
                {
                    mainstring_position_current =
                        new Vector2(MathHelper.Lerp(mainstring_position_current.X, mainstring_position_phase1.X, 0.35f), mainstring_position_current.Y);
                }
                else if(chronoSwitch<= 2.5f)
                {
                    mainstring_position_current =
                        new Vector2(MathHelper.Lerp(mainstring_position_current.X, mainstring_position_phase2.X, 1.5f), mainstring_position_current.Y);

                }
                else
                {
                    PlayAudio(snd_SFX[5],0.5f);
                    chronoSwitch = 0.0f;
                    isDisplayMainscreenPanel = false;
                    mainstring_position_current = mainstring_position_default;
                }

                return;
            }


            //-- attaque copain lancée, attendre avant de choisir suivant
            if(hasPlayerLaunchAttack)
            {
                chronoChangePlayer += 0.15f;
                if(chronoChangePlayer>=5.0f)
                {
                    chronoChangePlayer = 0.0f;
                    hasPlayerLaunchAttack = false;
                    changePlayer = true;
                    UI_Player_Has_Ennemi_Selected = false;
                }
                return;
            }
            //-- animer le changement de player
            if(changePlayer)
            {
                AnimateChangePlayerSelection();
                return;
            }

            #region <animer la sélection des ennemis>
            if (isSwitchEnnemi_Reverse)
            {
                canDraw = false;
                AnimateEnnemiesSelection_Right_to_Left();
                return;
            }
            else if (isSwitchEnnemi)
            {
                canDraw = false;
                AnimateEnnemiesSelection_Left_to_Right();
                return;
            }
            #endregion </animer la sélection des ennemis>


            //-- <fin de partie>
            if (shutdownCombat)
            {
                Exit_Scene();
                return;
            }
            //-end- </fin de partie>

           

            MouseState mousestate = Mouse.GetState();
            Point mouseposition = mousestate.Position;
            var isleftclicked = mousestate.LeftButton == ButtonState.Pressed;

            UIcursorPos = uicamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();

            //-- autoSFX range alpha
           

            CleanAllActors();

            #region <IA Ennemies group>
            if (isEnnemyTurn)
            {

                switch (ennemi_team_state)
                {
                    case 0:
                        //-- initialiser les paramètres
                        ennemi_team_state = 1;
                        break;

                    case 1:
                        //-- choisir une cible vivante
                        if (actors_friends.Count > 1 && chronoIA_EnnemiState == 0.0f)
                        {
                            //-- il reste plus de 1 copain
                            randFriend = Randomizer.GiveRandomInt(0, actors_friends.Count);
                        }
                        else if (actors_friends.Count == 1)
                        {
                            //-- il ne reste qu'1 copain
                            randFriend = 0;
                        }
                        else if (actors_friends.Count == 0)
                        {
                            //-- il n'y a plus de copains, la partie est finie

                            //-- on déclare la prochaine scène et on ferme le prog
                            nextscene = scene.loose;
                            Exit_Scene();
                            shutdownCombat = true;
                            return;
                        }
                        currentFriendListID = randFriend;
                        chronoIA_EnnemiState += 0.05f;

                        if (chronoIA_EnnemiState > 1.0f)
                        {
                            chronoIA_EnnemiState = 0;
                            ennemi_team_state = 2;

                        }

                        break;

                    case 2:

                        chronoIA_EnnemiState += 0.045f;

                        if (chronoIA_EnnemiState > 1.0f)
                        {
                            ennemi_team_state = 3;
                            chronoIA_EnnemiState = 0;

                            int limit = 2;

                            if (actors_ennemies[currentEnnemyListID].level > 10)
                            {
                                if (actors_ennemies[currentEnnemyListID].level > 20)
                                {
                                    limit = 4;
                                }
                                else
                                {
                                    limit = 3;
                                }
                            }

                            int attackRange = Randomizer.GiveRandomInt(0, limit + 1);
                            ennemiATTACK_ID = attackRange;
                            BuildAttack(actors_ennemies[currentEnnemyListID].Attack[attackRange]
                                , actors_ennemies[currentEnnemyListID]
                                , actors_friends[currentFriendListID]);

                            hasEnnemiLaunchedAttack = true;
                        }

                        break;
                    case 3:

                        chronoIA_EnnemiState += 0.015f;

                        if (chronoIA_EnnemiState >= 1.0f && hasEnnemiLaunchedAttack)
                        {
                            ennemiATTACK_ID = -10;

                            hasEnnemiLaunchedAttack = false;
                            int temp = currentEnnemyListID;

                            temp++;
                            ennemi_IA_Counter++;

                          
                            if (ennemi_IA_Counter > actors_ennemies.Count-1)
                            {
                                //-- c'est au tour des copains!
                                currentEnnemyListID = 0;
                                currentFriendListID = 0;
                                isEnnemyTurn = false;
                                isDisplayMainscreenPanel = true;
                                chronoIA_EnnemiState = 0.0f;
                                isSwitchEnnemi = true;
                                ennemi_IA_Counter = 0;
                                friend_counter = 0;
                                return;
                            }
                            else if (temp > actors_ennemies.Count - 1 && actors_ennemies.Count > 1)
                            {
                                //-- On continue
                                currentEnnemyListID = 0;
                                chronoIA_EnnemiState = 0.0f;
                                isSwitchEnnemi = true;
                                return;
                            }
                            else if (actors_ennemies.Count == 1)
                            {
                                //-- c'est au tour des copains!
                                currentEnnemyListID = 0;
                                currentFriendListID = 0;
                                isEnnemyTurn = false;
                                isDisplayMainscreenPanel = true;
                                chronoIA_EnnemiState = 0.0f;
                                friend_counter = 0;
                                ennemi_IA_Counter = 0;
                                return;
                            }
                            isSwitchEnnemi = true;
                            currentEnnemyListID = temp;

                            if (temp++ > actors_ennemies.Count - 1)
                            {
                                nextEnnemiID = 0;
                            }
                            else
                            {
                                nextEnnemiID = currentEnnemyListID + 1;
                            }

                        }
                        else if (chronoIA_EnnemiState >= 3.0f)
                        {

                            chronoIA_EnnemiState = 0.0f;
                            ennemi_team_state = 0;
                        }
                        break;
                }


                return;
            }

            #endregion <IA Ennemies group>

            #region <Friends Behaviours>
            else
            {
                KeyboardState kbs = Keyboard.GetState();

                OnClickGetNewEnnemi(ref UIcursorPos, ref isleftclicked);

                if(UI_Player_Has_Ennemi_Selected)
                {
                    OnKeyboardGetCommand(ref kbs);
                    OnClickGetCommand(ref UIcursorPos, ref isleftclicked);

                    if (UI_cmd_ATTAQUE_clicked)
                    {
                        OnClickAttaquePanel(ref UIcursorPos, ref isleftclicked);
                        OnKeyboardAttaquePanel(ref kbs);
                    }
                }
                else
                {
                    OnKeyboardTargetToHit(ref kbs);
                    OnClickTargetToHit(ref UIcursorPos, ref isleftclicked);
                }
            }
            #endregion </Friends Behaviours>

            base.Update();
        }


        private void OnKeyboardAttaquePanel(ref KeyboardState kbs)
        {
            
            if (keyboardmanager == null) { return; }

            bool isleft = kbs.IsKeyDown(keyboardmanager.GetKey("left"));
            bool isright = kbs.IsKeyDown(keyboardmanager.GetKey("right"));
            bool isup = kbs.IsKeyDown(keyboardmanager.GetKey("up"));
            bool isdown = kbs.IsKeyDown(keyboardmanager.GetKey("down"));
            bool isvalid1 = kbs.IsKeyDown(keyboardmanager.GetKey("valid1"));
            bool isvalid2 = kbs.IsKeyDown(keyboardmanager.GetKey("valid2"));
            Color selectedButton = Color.DarkGreen;
            if (!isvalid1 && !isvalid2 && !isleft && !isright && !isup && !isdown)
            {
                keyboardticks = 0;
            }
            else
            {
                if ((isvalid1 || isvalid2) && keyboardticks == 0)
                {
                    keyboardticks++;
                    PlayAudio(snd_SFX[2], 0.5f);

                   
                    BuildAttack(
                               actors_friends[currentFriendListID].Attack[phase3_btn_ID_selected],
                               actors_friends[currentFriendListID],
                               actors_ennemies[currentEnnemyListID]);

                    //-- remise à zéro et recommencer un cycle
                    changePlayer = true;
                    UI_cmd_ATTAQUE_clicked = false;
                    UI_Player_Has_Ennemi_Selected = false;
                }

                int limit = 2;

                if (actors_friends[currentFriendListID].level > 10)
                {
                    if (actors_friends[currentFriendListID].level > 20)
                    {
                        limit = 4;
                    }
                    else
                    {
                        limit = 3;
                    }
                }

                float volumeAudio = 0.2f;
                    if (isright && keyboardticks == 0)
                    {
                        PlayAudio(snd_SFX[3], volumeAudio);
                        keyboardticks++;
                        int temp = phase3_btn_ID_selected;
                        temp++;
                        if (temp > limit) { temp = 0; }
                    phase3_btn_ID_selected = temp;
                    }
                    else if (isleft && keyboardticks == 0)
                    {
                        PlayAudio(snd_SFX[3], volumeAudio);
                        keyboardticks++;

                        int temp = phase3_btn_ID_selected;
                        temp--;
                        if (temp < 0) { temp = limit-1; }
                    phase3_btn_ID_selected = temp;
                    }


                    if (isup && keyboardticks == 0)
                    {
                        PlayAudio(snd_SFX[3], volumeAudio);
                        keyboardticks++;

                        int temp = phase3_btn_ID_selected;
                        temp -= 2;
                        if (temp < 0) { temp += limit; }
                    phase3_btn_ID_selected = temp;
                    }
                    else if (isdown && keyboardticks == 0)
                    {
                        PlayAudio(snd_SFX[3], volumeAudio);
                        keyboardticks++;

                        int temp = phase3_btn_ID_selected;
                        temp += 2;
                        if (temp > limit) { temp -= limit; }
                    phase3_btn_ID_selected = temp;
                    }
                

            }
        }

        int phase1_btn_ID_selected = 0;
        int phase2_btn_ID_selected = 0;
        int phase3_btn_ID_selected = 0;
        private void OnKeyboardGetCommand(ref KeyboardState kbs)
        {
            if(UI_cmd_ATTAQUE_clicked) { return; }
            if (keyboardmanager == null) { return; }

            bool isleft = kbs.IsKeyDown(keyboardmanager.GetKey("left"));
            bool isright = kbs.IsKeyDown(keyboardmanager.GetKey("right"));
            bool isup = kbs.IsKeyDown(keyboardmanager.GetKey("up"));
            bool isdown = kbs.IsKeyDown(keyboardmanager.GetKey("down"));
            bool isvalid1 = kbs.IsKeyDown(keyboardmanager.GetKey("valid1"));
            bool isvalid2 = kbs.IsKeyDown(keyboardmanager.GetKey("valid2"));
            Color selectedButton = Color.DarkGreen;
            if (!isvalid1 && !isvalid2 && !isleft && !isright && !isup && !isdown)
            {
                keyboardticks = 0;
            }
            else 
            {
                if(keyboardticks>0) { return; }
                if(mouseticks>0) { return; }

                if((isvalid1|| isvalid2) && keyboardticks==0)
                {
                    keyboardticks++;
                    PlayAudio(snd_SFX[2], 0.5f);
                    switch (phase2_btn_ID_selected)
                    {
                        case 0:UI_cmd_ATTAQUE_clicked = true; break;
                        case 1: UI_Player_Has_Ennemi_Selected = false; break;
                        case 2: break;
                        case 3: break;
                    }
                }

                float volumeAudio = 0.2f;
                if (isright)
                {
                    PlayAudio(snd_SFX[3], volumeAudio);
                    keyboardticks++;
                    int temp = phase2_btn_ID_selected;
                    temp++;
                    if (temp > 4) {temp = 0;}
                    phase2_btn_ID_selected = temp;
                }
                else if (isleft)
                {
                    PlayAudio(snd_SFX[3], volumeAudio);
                    keyboardticks++;

                    int temp = phase2_btn_ID_selected;
                    temp--;
                    if (temp <0) { temp = 3; }
                    phase2_btn_ID_selected = temp;
                }
                
                if(isup)
                {
                    PlayAudio(snd_SFX[3], volumeAudio);
                    keyboardticks++;

                    int temp = phase2_btn_ID_selected;
                    temp-=2;
                    if (temp < 0) { temp += 4; }
                    phase2_btn_ID_selected = temp;
                }
                else if (isdown)
                {
                    PlayAudio(snd_SFX[3], volumeAudio);
                    keyboardticks++;

                    int temp = phase2_btn_ID_selected;
                    temp += 2;
                    if (temp > 4) { temp -= 4; }
                    phase2_btn_ID_selected = temp;
                }
            }
        }
        private void OnKeyboardTargetToHit(ref KeyboardState kbs)
        {
            if(keyboardmanager==null) {  return; }  

            bool isleft = kbs.IsKeyDown(keyboardmanager.GetKey("left"));
            bool isright = kbs.IsKeyDown(keyboardmanager.GetKey("right"));
            bool isup = kbs.IsKeyDown(keyboardmanager.GetKey("up"));
            bool isdown = kbs.IsKeyDown(keyboardmanager.GetKey("down"));
            bool isvalid1 = kbs.IsKeyDown(keyboardmanager.GetKey("valid1"));
            bool isvalid2 = kbs.IsKeyDown(keyboardmanager.GetKey("valid2"));
            
            bool selectprevious = false, selectnext = false;

            if (!isvalid1 && !isvalid2 && !isleft && !isright && !isup && !isdown)
            {
                mouseticks = 0;
                keyboardticks = 0;
            }
            else
            {
                if ((isvalid1 || isvalid2) && keyboardticks == 0)
                {
                    keyboardticks++;
                    PlayAudio(snd_SFX[2], 0.5f);

                    switch (phase1_btn_ID_selected)
                    {
                        case 0: selectprevious = true; break;
                        case 1: selectnext = true; break;
                        case 2: /*-- ouvrir le sac */ break;
                        case 3: UI_Player_Has_Ennemi_Selected = true; break;
                    }

                    if (!selectnext && !selectprevious)
                    {
                        return;
                    }
                }

                if (!selectnext && !selectprevious)
                {

                    float volumeAudio = 0.2f;
                    if (isright && keyboardticks == 0)
                    {
                        PlayAudio(snd_SFX[3], volumeAudio);
                        keyboardticks++;
                        int temp = phase1_btn_ID_selected;
                        temp++;
                        if (temp > 4) { temp = 0; }
                        phase1_btn_ID_selected = temp;
                    }
                    else if (isleft && keyboardticks == 0)
                    {
                        PlayAudio(snd_SFX[3], volumeAudio);
                        keyboardticks++;

                        int temp = phase1_btn_ID_selected;
                        temp--;
                        if (temp < 0) { temp = 3; }
                        phase1_btn_ID_selected = temp;
                    }


                    if (isup && keyboardticks == 0)
                    {
                        PlayAudio(snd_SFX[3], volumeAudio);
                        keyboardticks++;

                        int temp = phase1_btn_ID_selected;
                        temp -= 2;
                        if (temp < 0) { temp += 4; }
                        phase1_btn_ID_selected = temp;
                    }
                    else if (isdown && keyboardticks == 0)
                    {
                        PlayAudio(snd_SFX[3], volumeAudio);
                        keyboardticks++;

                        int temp = phase1_btn_ID_selected;
                        temp += 2;
                        if (temp > 4) { temp -= 4; }
                        phase1_btn_ID_selected = temp;
                    }
                    return;
                }

                int previous = currentEnnemyListID - 1;
                int next = currentEnnemyListID + 1;

                if (previous < 0 && actors_ennemies.Count > 1)
                {
                    previous = actors_ennemies.Count - 1;
                }
                else if (actors_ennemies.Count == 1)
                {
                    previous = -100;
                }

                if (next >= actors_ennemies.Count && actors_ennemies.Count > 1)
                {
                    next = 0;
                }
                else if (actors_ennemies.Count == 1)
                {
                    next = -100;
                }

                if (previous >= 0)
                {
                    if (selectprevious)
                    {
                        phase1_btn_ID_selected = 0;

                        mouseticks++;
                        PlayAudio(snd_SFX[0], 0.5f);
                        isSwitchEnnemi_Reverse = true;
                        int temp = currentEnnemyListID;
                        temp--;
                        if (temp < 0)
                        {
                            temp = actors_ennemies.Count - 1;
                        }

                        currentEnnemyListID = temp;

                        TE_Manager.shakeennemy = true;

                        bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx3;
                        bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx3;
                    }

                }

                if (next >= 0)
                {
                    if (selectnext)
                    {
                        phase1_btn_ID_selected = 1;

                        isSwitchEnnemi = true;
                        PlayAudio(snd_SFX[0], 0.5f);
                        mouseticks++;
                        int temp = currentEnnemyListID;
                        temp++;
                        if (temp >= actors_ennemies.Count)
                        {
                            temp = 0;
                        }

                        currentEnnemyListID = temp;
                        TE_Manager.shakeennemy = true;

                        bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx1;
                        bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx1;
                    }

                }
            }

        }

        private void OnClickTargetToHit(ref Point mouseposition, ref bool isclicked)
        {
            if(!isclicked) { mouseticks = 0; }
            for (int i = 0; i < btn_select_ennemi.Length; i++)
            {
                if (btn_select_ennemi[i].IsCollide(mouseposition))
                {
                    btn_select_ennemi[i].drawrect.color = Color.DarkGreen;

                    phase1_btn_ID_selected = i;

                    int previous = currentEnnemyListID - 1;
                    int next = currentEnnemyListID + 1;
                    if (isclicked && mouseticks == 0)
                    {
                        if (previous < 0 && actors_ennemies.Count > 1)
                        {
                            previous = actors_ennemies.Count - 1;
                        }
                        else if (actors_ennemies.Count == 1)
                        {
                            previous = -100;
                        }

                        if (next >= actors_ennemies.Count && actors_ennemies.Count > 1)
                        {
                            next = 0;
                        }
                        else if (actors_ennemies.Count == 1)
                        {
                            next = -100;
                        }

                        if (previous >= 0)
                        {
                            if (i==0)
                            {
                                PlayAudio(snd_SFX[0], 0.5f);
                                isSwitchEnnemi_Reverse = true;
                                int temp = currentEnnemyListID;
                                temp--;
                                if (temp < 0)
                                {
                                    temp = actors_ennemies.Count - 1;
                                }

                                currentEnnemyListID = temp;

                                TE_Manager.shakeennemy = true;

                                bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx3;
                                bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx3;
                            }

                        }

                        if (next >= 0)
                        {
                            if (i==1)
                            {
                                isSwitchEnnemi = true;
                                PlayAudio(snd_SFX[0], 0.5f);
                                int temp = currentEnnemyListID;
                                temp++;
                                if (temp >= actors_ennemies.Count)
                                {
                                    temp = 0;
                                }

                                currentEnnemyListID = temp;
                                TE_Manager.shakeennemy = true;

                                bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx1;
                                bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx1;
                            }

                        }

                    }










                    if (isclicked && mouseticks == 0)
                    {
                        mouseticks++;

                        switch(i)
                        {
                            case 0: break;
                            case 1: break;
                            case 2: break;
                            case 3: UI_Player_Has_Ennemi_Selected = true; break;
                        }
                    }
                }
                else
                {
                    btn_select_ennemi[i].drawrect.color = Color.Green;
                }
            }



        }

        private int GetDamageMelee(Actor attacker, Actor defender, int critigueaverage = 0)
        {
            int defense = defender.Defense;
            int force = attacker.Force;

            int rand = Randomizer.GiveRandomInt(85, 100);

            int level = attacker.level;

            float reducer = (float)rand / 100;

            float multiplier = 1;
            int baseDamage = 2;
            if (critigueaverage > 0)
            {
                int random = Randomizer.GiveRandomInt(0, 100);
                if (critigueaverage < (int)rand)
                {
                    multiplier = 1.3f;
                }
            }

            int damage = (int)((((2*level +10)/250)*(force/defense*baseDamage)+2)*multiplier) ; //(int)(((force / defense)) * multiplier * 10 * reducer);

            return damage;
        }

        bool isSwitchEnnemi_Reverse = false;
        bool isSwitchEnnemi = false;

        private void OnClickGetNewEnnemi(ref Point mouseposition, ref bool isclicked)
        {
            if (!isclicked)
            {
                mouseticks = 0;
            }

            int previous = currentEnnemyListID - 1;
            int next = currentEnnemyListID + 1;

            if(previous<0 && actors_ennemies.Count>1)
            {
                previous = actors_ennemies.Count - 1;
            }
            else if(actors_ennemies.Count==1)
            {
                previous = -100;
            }

            if(next>=actors_ennemies.Count && actors_ennemies.Count>1)
            {
                next = 0;
            }
            else if (actors_ennemies.Count == 1)
            {
                next = -100;
            }

            if (previous >= 0)
            {
                if (ennemicard_previous.Contains(mouseposition))
                {
                    col_enemi_previous = Color.White;
                   
                    if (isclicked && mouseticks==0)
                    {
                        mouseticks++;
                        PlayAudio(snd_SFX[0], 0.5f);
                        isSwitchEnnemi_Reverse = true;

                        int temp = currentEnnemyListID;
                        temp--;
                        if(temp <0)
                        {
                            temp = actors_ennemies.Count-1;
                        }

                        currentEnnemyListID = temp;

                        TE_Manager.shakeennemy = true;

                        bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx3;
                        bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx3;
                    }
                }
                else
                {
                    col_enemi_previous = Color.Black;
                }
            }

            if (next >=0)
            {
                if (ennemicard_next.Contains(mouseposition))
                {
                    col_enemi_next = Color.White;

                    if (isclicked && mouseticks == 0)
                    {
                        isSwitchEnnemi = true;
                        PlayAudio(snd_SFX[0], 0.5f);
                        mouseticks++;
                        int temp = currentEnnemyListID;
                        temp++;
                        if (temp >= actors_ennemies.Count)
                        {
                            temp = 0;
                        }

                        currentEnnemyListID = temp;
                        TE_Manager.shakeennemy = true;

                        bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx1;
                        bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx1;
                    }
                }
                else
                {
                    col_enemi_next = Color.Black;

                }
            }
        }

        private void BuildAttack(ATTACK ownerAttack,Actor attacker, Actor defender)
        {
            int sfxIndex = 0;
            PlayAudio(sndCombats[0]);
            switch (ownerAttack.type)
            {
                /*
                    reduceHP,      //- 0 - réduit santé
                    reduceSPEED,   //- 1 - réduit vitesse victime  --> pourcentage esquive victime réduit
                    reduceFORCE,   //- 2 -    ''  force ''         --> dégâts physiques victime réduits
                    reduceDEFENSE, //- 3 -    ''  défense ''       --> blocage attaque victime réduit
                    drainHP,       //- 4 - ponctionne santé et distribue à attaquant
                    SLEEP,         //- 5 - annule X tours --> réveil avec attaque!
                    POISON,        //- 6 - dégât tout les tours pendant X tours
                    PARALYSIE,     //- 7 - annule tout les tours (nécessite antipara)
                    FIRE,          //- 8 - dégâts tout les tours pendant X tours
                    SHADOW,        //- 9 - coup critiques annulés
                */

                case ATTACK_TYPE.reduceHP:

                    int damage = GetDamageMelee(attacker, defender);
                    TE_Manager.shakeennemy = true;
                  
                    defender.HP -= damage;
                    
                    break;

                case ATTACK_TYPE.reduceSPEED:

                    defender.Speed -= 1;
                    if(defender.Speed < 0) defender.Speed = 1;

                    break;
                case ATTACK_TYPE.reduceFORCE:
                    defender.Force -= 1;
                    if (defender.Force < 0) defender.Force = 1; 
                    break;
                case ATTACK_TYPE.reduceDEFENSE:
                    defender.Defense -= 1;
                    if (defender.Defense < 0) defender.Defense = 1; 
                    break;
                case ATTACK_TYPE.drainHP:
                    int value = (int)(attacker.level * 0.8f);
                    defender.HP -= value;
                    attacker.HP += value;
                    break;
                case ATTACK_TYPE.SLEEP:
                    attacker.sleeping = 1;
                    break;
                case ATTACK_TYPE.POISON: 
                    attacker.poisoned = 1;
                    break;
                case ATTACK_TYPE.PARALYSIE: 
                    attacker.paralysed = 1;
                    break;
                case ATTACK_TYPE.FIRE: 
                    attacker.onFire = 1;
                    break;
                case ATTACK_TYPE.SHADOW: 
                    attacker.shadowed = 1;
                    break;
            }

            sfxIndex = (int)ownerAttack.type;
            if (isEnnemyTurn)
            {
                currentframeX_btn_wheel = 0;
                currentEnnemyFrame_btn_Wheel = sfxIndex;
                alpha_ENNEMI_SFX = 1.0f;
            }
            else
            {
                currentframeX_btn_wheel = sfxIndex;
                currentEnnemyFrame_btn_Wheel = 0;
                alpha_playerSFX = 1.0f;
            }

        }

        private void OnClickAttaquePanel(ref Point mouseposition, ref bool isclicked)
        {
            if (!isclicked && Keyboard.GetState().GetPressedKeyCount()==0)
            {
                mouseticks = 0;
            }
            int hitter = 0;


            int limit = 2;

            if (actors_friends[currentFriendListID].level > 10)
            {
                if (actors_friends[currentFriendListID].level > 20)
                {
                    limit = 4;
                }
                else
                {
                    limit = 3;
                }
            }

            for (int i = 0; i < btn_attaque.Length; i++)
            {
                if (btn_attaque[i].IsCollide(mouseposition))
                {
                    if (i >= limit) continue;

                    phase3_btn_ID_selected = i;
                    btn_attaque[i].drawrect.color = Color.Cyan;
                    frame_cursor = cursors[2];
                    hitter++;
                    int currentEnnemi = currentEnnemyListID;

                    if(isclicked && mouseticks == 0)
                    {
                        //-- attaque validée
                        mouseticks++;
                        BuildAttack(actors_friends[currentFriendListID].Attack[i]
                            , actors_friends[currentFriendListID]
                            ,actors_ennemies[currentEnnemyListID]);
                        hasPlayerLaunchAttack = true;
                        UI_cmd_ATTAQUE_clicked = false;
                    }
                }
                else
                {
                    btn_attaque[i].drawrect.color = Color.White;

                }
            }

            if (hitter == 0)
            {
                frame_cursor = cursors[0];
            }
        }

        private bool GetNextActor_Friend()
        {
           
            bool hasFoundAnActor = false;

            if (actors_friends.Count == 1) {currentFriendListID = 0; return false; }
            while (true)
            {
                if(actors_friends.Count==1)
                {
                    currentFriendListID = 0;
                    break;
                }

                int temp = currentFriendListID;

                temp++;
                friend_counter++;
                if (temp >= actors_friends.Count)
                {
                    temp = 0;
                    currentFriendListID = temp;
                }

                if (friend_counter > actors_friends.Count - 1 && actors_friends.Count != 0)
                {
                    //-- pas de combattants disponible selon les conditions exigées
                    //-- on bascule en mode [attaque-ennemis=true]
                    isEnnemyTurn = true;
                    isDisplayMainscreenPanel = true;
                    currentEnnemyListID = 0;
                    break;
                }

                if (actors_friends[temp].HP > 0)
                {
                    currentFriendListID = temp;
                    hasFoundAnActor = true;
                    break;
                }
                else if (actors_friends[temp].HP<=0)
                {
                    currentFriendListID = temp;
                }
            }

            return hasFoundAnActor;
        }

        bool hasEnnemiLaunchedAttack = false;
        int ennemi_IA_Counter = 0;
        private void OnClickGetCommand(ref Point mouseposition, ref bool isclicked)
        {
            if (!isclicked) { mouseticks = 0; }

            for (int i = 0; i < btn_combatPanel.Length; i++)
            {
                if (btn_combatPanel[i].IsCollide(mouseposition))
                {
                    btn_combatPanel[i].drawrect.color = myColors[9];

                    if (isclicked && mouseticks == 0)
                    {
                        UI_cmd_ATTAQUE_clicked = false;
                        mouseticks++;
                        PlayAudio(snd_SFX[3], 0.5f);

                        switch (i)
                        {
                            case 0: UI_cmd_ATTAQUE_clicked = true; break;
                            case 1: UI_Player_Has_Ennemi_Selected = false; break;
                            case 2: break;
                            case 3: break;
                        }
                    }
                }
                else
                {
                    btn_combatPanel[i].drawrect.color = btn_combatPanel[i].drawrect.DEFAULT_COLOR;
                }
            }
        }
        int friend_counter = 0;
        private void AnimateChangePlayerSelection()
        {
            
            //-- concerne les actor amis
            //-- donne un effet de [TRANSLATION - PINGPONG] vers la gauche
            //-- ALLER - RETOUR - RAZ (remise à zéro)

            chronoChangePlayer += 0.15f;

            if (chronoChangePlayer <= 1)
            {
                //-- translation vers la gauche
                friendcamera.MoveCamera(
                    new Vector2(
                    MathHelper.Lerp(0, 80, 2.5f), 0));

            }
            else if (chronoChangePlayer <= 2)
            {
                //-- retour du pingpong (translation vers la droite
                if (changePlayerState == 0)
                {
                    GetNextActor_Friend();
                    changePlayerState++;
                }

                friendcamera.MoveCamera(
                  new Vector2(
                  MathHelper.Lerp(0, -80, 2.5f), 0));
            }
            else if (chronoChangePlayer > 3)
            {
                //-- un temps de pause et réinitialisation des paramètres
                friendcamera.CenterOn(Vector2.Zero);
                chronoChangePlayer = 0.0f;
                changePlayerState = 0;
                changePlayer = false;
            }
        }
        private void AnimateEnnemiesSelection_Right_to_Left()
        {
            //-- concerne uniquement les ennemis !
            //-- donner un effet de déplacement des plateaux de la droite vers la gauche

            ennemiNextID = currentEnnemyListID + 1;
            if (ennemiNextID > actors_ennemies.Count - 1 && actors_ennemies.Count > 1)
            {
                ennemiNextID = 0;
            }

            ennemyPreviousID = currentEnnemyListID;
            chronoSwitch += 0.15f;

            if (chronoSwitch >= 3)
            {
                chronoSwitch = 0.0f;
                isSwitchEnnemi_Reverse = false;
                bgPlateauEnnemi_next = bgPlateauEnnemi_next_default;
                bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_default;
                int temp = currentEnnemyListID - 1;

                if (temp < 0 && actors_ennemies.Count > 1)
                {
                    temp = actors_ennemies.Count - 1;
                }
                else if (actors_ennemies.Count < 1)
                {
                    temp = -100;
                }
                ennemyPreviousID = temp;


                temp = currentEnnemyListID + 1;
                if (temp >= actors_ennemies.Count - 1 && actors_ennemies.Count > 1)
                {
                    temp = 0;
                }
                else if (actors_ennemies.Count == 0)
                {
                    temp = -100;
                }

                nextEnnemiID = temp;
            }
            else if (chronoSwitch >= 2)
            {
                bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx1;
                bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx1;
            }
            else if (chronoSwitch >= 1)
            {
                bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx2;
                bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx2;
            }
            else if (chronoSwitch >= 1)
            {
                bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx1;
                bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx1;
            }


            ennemicard_next = new Rectangle(
                bgPlateauEnnemi_next.X + (int)(bgPlateauEnnemi_next.Width / 2) - (int)(ennemiCard.Width / 4),
            bgPlateauEnnemi_next.Y + (int)(bgPlateauEnnemi_next.Height / 2) - (int)(ennemiCard.Height / 2),
            (int)(ennemiCard.Width / 2),
            (int)(ennemiCard.Height / 2));
            ;
            ennemicard_previous = new Rectangle(
                 bgPlateauEnnemi_previous.X + (int)(bgPlateauEnnemi_previous.Width / 2) - (int)(ennemiCard.Width / 4),
                bgPlateauEnnemi_previous.Y + (int)(bgPlateauEnnemi_previous.Height / 2) - (int)(ennemiCard.Height / 2),
                (int)(ennemiCard.Width / 2),
                (int)(ennemiCard.Height / 2));

            canDraw = true;
        }
        private void AnimateEnnemiesSelection_Left_to_Right()
        {
            //-- concerne uniquement les ennemis !
            //-- donner un effet de déplacement des plateaux de la gauche vers la droite
            ennemyPreviousID = currentEnnemyListID - 1;

            if (ennemyPreviousID < 0 && actors_ennemies.Count > 1)
            {
                ennemyPreviousID = actors_ennemies.Count - 1;
            }

            ennemiNextID = currentEnnemyListID;

            chronoSwitch += 0.15f;

            if (chronoSwitch >= 3)
            {
                chronoSwitch = 0.0f;
                isSwitchEnnemi = false;
                bgPlateauEnnemi_next = bgPlateauEnnemi_next_default;
                bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_default;

                int temp = currentEnnemyListID + 1;

                if (temp > actors_ennemies.Count - 1 && actors_ennemies.Count > 1)
                {
                    temp = 0;
                }
                else if (actors_ennemies.Count < 1)
                {
                    temp = -100;
                }
                ennemiNextID = temp;

                temp = currentEnnemyListID - 1;

                if (temp < 0 && actors_ennemies.Count > 1)
                {
                    temp = actors_ennemies.Count - 1;
                }
                else if (actors_ennemies.Count < 1)
                {
                    temp = -100;
                }
                ennemyPreviousID = temp;
            }
            else if (chronoSwitch >= 2)
            {
                bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx3;
                bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx3;
            }
            else if (chronoSwitch >= 1)
            {
                bgPlateauEnnemi_next = bgPlateauEnnemi_next_fx2;
                bgPlateauEnnemi_previous = bgPlateauEnnemi_previous_fx2;
            }


            ennemicard_next = new Rectangle(
                bgPlateauEnnemi_next.X + (int)(bgPlateauEnnemi_next.Width / 2) - (int)(ennemiCard.Width / 4),
            bgPlateauEnnemi_next.Y + (int)(bgPlateauEnnemi_next.Height / 2) - (int)(ennemiCard.Height / 2),
            (int)(ennemiCard.Width / 2),
            (int)(ennemiCard.Height / 2));

            ennemicard_previous = new Rectangle(
                 bgPlateauEnnemi_previous.X + (int)(bgPlateauEnnemi_previous.Width / 2) - (int)(ennemiCard.Width / 4),
                bgPlateauEnnemi_previous.Y + (int)(bgPlateauEnnemi_previous.Height / 2) - (int)(ennemiCard.Height / 2),
                (int)(ennemiCard.Width / 2),
                (int)(ennemiCard.Height / 2));

            canDraw = true;
        }
    
        int keyboardticks = 0;
     
        #region <Player Attack System> 
        public void UpdateStats()
        {
            //-- met à jour les barres de vie des ennemis et des amis
            Get_ENNEMY_LifeBar();
            Get_Friend_LifeBar();
        }
        #endregion </Player Attack System>

      
        public override void Draw(ref SpriteBatch _sp)
        {
            //-- afficher le décor en arrière plan
            _sp.Draw(backgrounds[0], backgroundPosition, new Rectangle(0, 0, 240, 160), Color.White * alphaALL,
                 0, Vector2.Zero, SpriteEffects.None, 0.9f);


            if (actors_friends != null)
            {
                if (actors_friends.Count > 0)
                {
                    _sp.DrawString(cutsceneFont,
                        ActorCombat_MainRules.Get_Name(actors_friends[currentFriendListID].GetActorTYpe()),
                        playerNamePosition, Color.White * alphaALL,
                        0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.2f);
                }
                _sp.Draw(statusTex, playerLifeBar_BG, new Rectangle(0, 0, 16, 16), Color.White * alphaALL,
                0, Vector2.Zero, SpriteEffects.None, 0.4f);
            }

            if (actors_ennemies != null)
            {
                if (actors_ennemies.Count > 0)
                {
                    _sp.DrawString(cutsceneFont,
                        ActorCombat_MainRules.Get_Name(actors_ennemies[currentEnnemyListID].GetActorTYpe()),
                        enemiNamePosition, Color.White * alphaALL,
                        0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.2f);

                }
                _sp.Draw(statusTex, ennemiLifeBar_BG, new Rectangle(0, 0, 16, 16), Color.White * alphaALL,
                     0, Vector2.Zero, SpriteEffects.None, 0.4f);
            }

            #region <Feature : anime switch opponents>
            if (actors_ennemies != null && actors_ennemies.Count>1
                && canDraw)
            {

                if (ennemyPreviousID >= 0 && actors_ennemies.Count > 1
                    && ennemyPreviousID < actors_ennemies.Count)
                {

                    //-- le plateau
                    _sp.Draw(textures[3], bgPlateauEnnemi_previous, Color.Black * alphaALL);
                    //-- la carte
                    _sp.Draw(textures[2],
                        ennemicard_previous,
                        actors_ennemies[ennemyPreviousID].frame,
                        col_enemi_previous * alphaALL);
                }

                if (ennemiNextID >= 0 && actors_ennemies.Count > 1
                    && ennemiNextID < actors_ennemies.Count
                    && ennemiNextID != actors_ennemies.Count)
                {
                    //-- le plateau
                    _sp.Draw(textures[3], bgPlateauEnnemi_next, Color.Black * alphaALL);
                    //-- la carte
                    _sp.Draw(textures[2],
                       ennemicard_next,
                       actors_ennemies[ennemiNextID].frame,
                       col_enemi_next * alphaALL);
                }
            }
            #endregion </Feature : anime switch opponents>

            _sp.Draw(statusTex, bgstatsEnnemi, new Rectangle(0, 0, 16, 16), myColors[1] * alphaALL,
                 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            _sp.Draw(statusTex, bgstatsPlayer, new Rectangle(0, 0, 16, 16), myColors[1] * alphaALL,
                 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            base.Draw(ref _sp);
        }





        public override void Draw_Friend(ref SpriteBatch _spUI)
        {
            _spUI.Draw(textures[3], bgPlateauPlayer, Color.White * alphaALL);

            if (actors_friends != null)
            {
                if (actors_friends.Count > 0 )
                {
                    _spUI.Draw(textures[1],
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
                if (actors_ennemies.Count > 0 && !isSwitchEnnemi && !isSwitchEnnemi_Reverse)
                {

                    _spUI.Draw(textures[3], bgPlateauEnnemi, Color.White * alphaALL);

                    _spUI.Draw(textures[2],
                            ennemiCard,
                            actors_ennemies[currentEnnemyListID].frame,
                            Color.White * alphaALL);
                }

              
            }



            base.Draw_Ennemy(ref _spUI);
        }


        bool shutdownCombat = false;
        float alpha_ENNEMI_SFX = 0.0f;

        bool UI_cmd_ATTAQUE_clicked = false;
        int randReplique = 0;
        bool UI_Player_Has_Ennemi_Selected = false;

        int ennemiATTACK_ID = 0;
        public override void Draw_UI(ref SpriteBatch _spUI)
        {
            if (shutdownCombat) return;
            if (actors_ennemies == null || actors_friends == null) return;

            if (isDisplayMainscreenPanel)
            {
                _spUI.Draw(statusTex, mainscreen_switchMode_panel, new Rectangle(0, 0, 16, 16), Color.Black,
                  0, Vector2.Zero, SpriteEffects.None, 0.2f);

                _spUI.DrawString(cutsceneFont, mainscreen_str[mainscreen_str_ID]
                  , mainstring_position_current,
                  Color.Yellow,
                 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);

                _spUI.Draw(statusTex, ToOffset(new Rectangle(0,0,240,160)), new Rectangle(0, 0, 16, 16), Color.Black*0.3f,
                 0, Vector2.Zero, SpriteEffects.None, 0.3f);
            }


            #region <Feature : main command panel (panneau de commmande en bas)>
            //-- background panneau principal des actions
            _spUI.Draw(statusTex, bgCommandPanel, new Rectangle(0, 0, 16, 16), myColors[7] * alphaALL,
                0, Vector2.Zero, SpriteEffects.None, 0.9f);

          

            if (!UI_Player_Has_Ennemi_Selected)
            {
                _spUI.Draw(statusTex, bg_maincommands_panelcombat, new Rectangle(0, 0, 16, 16), Color.CornflowerBlue * alphaALL,
              0, Vector2.Zero, SpriteEffects.None, 0.8f);

                _spUI.Draw(statusTex, bg_maincommands_informations, new Rectangle(0, 0, 16, 16), Color.CornflowerBlue * alphaALL,
                   0, Vector2.Zero, SpriteEffects.None, 0.8f);



                //-- PHASE 1 -> SELECTIONNER CIBLE boutons de commande en bas à gauche
                for (int i = 0; i < btn_select_ennemi.Length; i++)
                {

                    Color col_unavailable_btns = Color.White;
                      col_unavailable_btns =  btn_select_ennemi[i].drawrect.color;
                    if (i == 2)
                    {
                        col_unavailable_btns = Color.DarkGray;
                    }


                    _spUI.Draw(statusTex, btn_select_ennemi[i].drawrect.position,
                        new Rectangle(0, 0, 16, 16), col_unavailable_btns * alphaALL,
                         0, Vector2.Zero, SpriteEffects.None, 0.7f);
                    
                    if (phase1_btn_ID_selected == i)
                    {
                        _spUI.Draw(textures[4], btn_select_ennemi[i].drawrect.position,
                       new Rectangle(0, 0, 37, 11), Color.MediumPurple* alphaALL,
                        0, Vector2.Zero, SpriteEffects.None, 0.65f);
                    }

                   

                    _spUI.DrawString(cutsceneFont, btn_select_ennemi[i].text,
                       new Vector2(btn_select_ennemi[i].drawrect.position.X + 2, btn_select_ennemi[i].drawrect.position.Y + 2),
                       Color.White * alphaALL,
                        0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.6f);
                }

                //-end- PHASE 1 -> SELECTIONNER CIBLE boutons de commande en bas à gauche

                if (actors_ennemies.Count > 0 && actors_friends.Count > 0)
                {
                    string output = "Choisissez une cible et validez!";
                    string output2 = string.Empty;
                    if (isEnnemyTurn && ennemi_team_state >= 2)
                    {
                        if (ennemiATTACK_ID >= 0)
                        {
                            output = actors_ennemies[currentEnnemyListID].name + " utilise ";

                            output2 = actors_ennemies[currentEnnemyListID].Attack[ennemiATTACK_ID].Name;
                        }
                        else
                        {
                            output = "C'est au tour de ";
                            output2 = actors_ennemies[currentEnnemyListID].name;
                        }
                    }
                    else if (isEnnemyTurn && ennemi_team_state < 2)
                    {
                        output = actors_ennemies[currentEnnemyListID].name + " a choisit ";
                        output2 = actors_friends[currentFriendListID].name + " comme cible";
                    }

                    _spUI.DrawString(cutsceneFont, output,
                             ToOffset(new Vector2(8, 130 + 2)),
                            Color.White * alphaALL,
                              0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.6f);

                    if (isEnnemyTurn)
                    {
                        _spUI.DrawString(cutsceneFont, output2,
                                ToOffset(new Vector2(8, 130 + 2 + 8)),
                               Color.White * alphaALL,
                                 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.6f);
                    }
                }
            }
            else
            {
                _spUI.Draw(statusTex, bg_maincommands_panelcombat, new Rectangle(0, 0, 16, 16), myColors[3] * alphaALL,
              0, Vector2.Zero, SpriteEffects.None, 0.8f);

                _spUI.Draw(statusTex, bg_maincommands_informations, new Rectangle(0, 0, 16, 16), myColors[3] * alphaALL,
                   0, Vector2.Zero, SpriteEffects.None, 0.8f);


                //-- PHASE 2 -> boutons de commande en bas à gauche
                for (int i = 0; i < btn_combatPanel.Length; i++)
                {

                    Color col_unavailable_btns = btn_combatPanel[i].drawrect.color;
                    if (i == 2 || i == 3)
                    {
                        col_unavailable_btns = Color.DarkGray;
                    }

                    _spUI.Draw(statusTex, btn_combatPanel[i].drawrect.position,
                        new Rectangle(0, 0, 16, 16), col_unavailable_btns * alphaALL,
                         0, Vector2.Zero, SpriteEffects.None, 0.7f);

                    if (phase2_btn_ID_selected == i)
                    {
                        _spUI.Draw(textures[4], btn_select_ennemi[i].drawrect.position,
                       new Rectangle(0, 0, 37, 11), Color.MediumPurple * alphaALL,
                        0, Vector2.Zero, SpriteEffects.None, 0.65f);
                    }

                 

                    _spUI.DrawString(cutsceneFont, btn_combatPanel[i].text,
                       new Vector2(btn_combatPanel[i].drawrect.position.X + 2, btn_combatPanel[i].drawrect.position.Y + 2),
                   myColors[8] * alphaALL, //   myColors[8] * alphaALL,
                        0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.6f);
                }

                //-end- PHASE 2 -> boutons de commande en bas à gauche

                #region <Bouton ATTAQUER - on/off - >
                if (UI_cmd_ATTAQUE_clicked)
                {
                    int limit = 2;

                    if (actors_friends[currentFriendListID].level > 10)
                    {
                        if (actors_friends[currentFriendListID].level > 20)
                        {
                            limit = 4;
                        }
                        else
                        {
                            limit = 3;
                        }
                    }

                    //-- afficher les attaques des joueurs --
                    for (int i = 0; i < limit; i++)
                    {
                        _spUI.Draw(statusTex, btn_attaque[i].drawrect.position,
                            new Rectangle(0, 0, 16, 16), btn_attaque[i].drawrect.color * alphaALL * 0.1f,
                             0, Vector2.Zero, SpriteEffects.None, 0.7f);
                        if (phase3_btn_ID_selected == i)
                        {
                            _spUI.Draw(textures[5], btn_attaque[i].drawrect.position,
                          new Rectangle(0, 0, 60, 11), Color.White * alphaALL,
                           0, Vector2.Zero, SpriteEffects.None, 0.65f);
                        }
                        _spUI.DrawString(cutsceneFont, " - " + actors_friends[currentFriendListID].Attack[i].Name.ToString(),
                           new Vector2(btn_attaque[i].drawrect.position.X + 2, btn_attaque[i].drawrect.position.Y + 2),
                           btn_attaque[i].drawrect.color * alphaALL,
                            0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.6f);
                    }
                }
                else
                {
                    //-- afficher le portrait des combattants
                    _spUI.Draw(portraits_combattants[actors_friends[currentFriendListID].textur2D_portraitID],
                       new Rectangle(bg_maincommands_informations.X, bg_maincommands_informations.Y, 60, 30), Color.White);


                    string output = actors_friends[currentFriendListID].repliques[randReplique].ToString();
                    string line1 = string.Empty;
                    string line2 = string.Empty;

                    string currentline = string.Empty;
                    int iteration = 0;
                    if (output.Length > 25)
                    {
                        string[] data = output.Split(' ');


                        for (int i = 0; i < data.Length; i++)
                        {
                            iteration = i;
                            currentline += data[i] + ' ';

                            if (currentline.Length > 25)
                            {
                                break;
                            }
                            else
                            {
                                line1 = currentline;
                            }
                        }


                        for (int i = iteration; i < data.Length; i++)
                        {
                            iteration = i;
                            line2 += data[i] + ' ';
                        }

                    }
                    else
                    {
                        line1 = output;
                    }

                    _spUI.DrawString(cutsceneFont, line1,
                              ToOffset(new Vector2(62, 130 + 2)),
                             Color.White * alphaALL,
                               0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.6f);

                    _spUI.DrawString(cutsceneFont, line2,
                             ToOffset(new Vector2(62, 130 + 13)),
                             Color.White * alphaALL,
                              0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.6f);


                }
                #endregion </Bouton ATTAQUER>

            }
            #endregion </Feature : main command panel (panneau de commmande en bas)>

            //-- montrer SFX : attaque ami contre ennemi
            _spUI.Draw(SFX, ennemiCard,
            new Rectangle(currentframeX_btn_wheel * 40, 0, 40, 40),
                Color.White * alpha_playerSFX * alphaALL,
                0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.1f);

            //-- montrer SFX : attaque ennemi contre ami
            _spUI.Draw(SFX, playerCard,
                new Rectangle(currentEnnemyFrame_btn_Wheel * 40, 0, 40, 40),
                Color.White * alpha_ENNEMI_SFX * alphaALL,
                0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.1f);


            #region <Feature : affichage nom, level et HP>
            //-- life and magic points

            _spUI.DrawString(cutsceneFont, "PV",
                         new Vector2(ennemiLifeBar.X - 8, ennemiLifeBar.Y - 1),
                        Color.White * alphaALL,
                          0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);

            if (actors_ennemies.Count > 0)
            {
                _spUI.DrawString(cutsceneFont,
                actors_ennemies[currentEnnemyListID].HP.ToString() + " / " +
                actors_ennemies[currentEnnemyListID].MAX_HP.ToString(),
                        new Vector2(ennemiLifeBar.X + 20, ennemiLifeBar.Y - 1),
                       Color.Black * alphaALL,
                         0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);

                _spUI.DrawString(cutsceneFont, "Lvl:" + actors_ennemies[currentEnnemyListID].level,
                            new Vector2(-26, bgstatsEnnemi.Y + 2),
                           Color.White * alphaALL,
                             0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.4f);

            }

            _spUI.DrawString(cutsceneFont, "PV",
                     new Vector2(playerLifeBar.X - 8, playerLifeBar.Y - 1),
                     Color.White * alphaALL,
                      0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);

            if (actors_friends.Count > 0)
            {
                _spUI.DrawString(cutsceneFont,
                actors_friends[currentFriendListID].HP.ToString() + " / " +
                actors_friends[currentFriendListID].MAX_HP.ToString(),
                        new Vector2(playerLifeBar.X + 20, playerLifeBar.Y - 1),
                       Color.Black * alphaALL,
                         0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);


                _spUI.DrawString(cutsceneFont, "Lvl:" + actors_friends[currentFriendListID].level,
                          new Vector2(bgstatsPlayer.Width - 26, bgstatsPlayer.Y + 2),
                         Color.Orange * alphaALL,
                           0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.4f);
            }

            //-- les barres de HP
            _spUI.Draw(statusTex, playerLifeBar, new Rectangle(0, 0, 16, 16), myColors[9] * alphaALL,
             0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.5f);
            _spUI.Draw(statusTex, ennemiLifeBar, new Rectangle(0, 0, 16, 16), myColors[9] * alphaALL,
                 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.5f);
            #endregion </Feature : affichage nom, level et HP>



            //-- afficher les points de dégâts
            if (hitterShowState == 1)
            {
                _spUI.DrawString(mainFont, HIT_ennemis, ToOffset(new Vector2(61, 73)), Color.Red * alphaALL,
                     0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);
                _spUI.DrawString(mainFont, HIT_player, ToOffset(new Vector2(138, 27)), Color.Red * alphaALL,
                     0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);
            }


          
            /* //-- DEBUG--
             *  _spUI.DrawString(cutsceneFont, "/ " + startFriendsCount.ToString(), ToOffset(new Vector2(100, 110)), Color.Orange * alphaALL);
               _spUI.DrawString(cutsceneFont, "/ " + startEnemiesCount.ToString(), ToOffset(new Vector2(216, 3)), Color.Orange * alphaALL);

               _spUI.DrawString(cutsceneFont, actors_friends.Count.ToString(), ToOffset(new Vector2(90, 110)), Color.Orange * alphaALL);
               _spUI.DrawString(cutsceneFont, actors_ennemies.Count.ToString(), ToOffset(new Vector2(204, 3)), Color.Orange * alphaALL);

               for (int i = 0; i < actors_ennemies.Count; i++)
               {
                   _spUI.DrawString(cutsceneFont,
                      "[" + actors_ennemies[i].HP + "]",
                       ToOffset(new Vector2(3 + (10 + 2) * i, 3)),
                       Color.Orange * alphaALL,
                         0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.1f);

               }
            */


            /*  _spUI.DrawString(cutsceneFont, "/ " + startFriendsCount.ToString(), ToOffset(new Vector2(100, 110)), Color.Orange * alphaALL);
              _spUI.DrawString(cutsceneFont, "/ " + startEnemiesCount.ToString(), ToOffset(new Vector2(216, 3)), Color.Orange * alphaALL);

              _spUI.DrawString(cutsceneFont, actors_friends.Count.ToString(), ToOffset(new Vector2(90, 110)), Color.Orange * alphaALL);
              _spUI.DrawString(cutsceneFont, actors_ennemies.Count.ToString(), ToOffset(new Vector2(204, 3)), Color.Orange * alphaALL);
            */
        /*    if(actors_ennemies.Count > 0)
            {
                _spUI.DrawString(cutsceneFont,
                  "HP = [" + actors_ennemies[currentEnnemyListID].HP + "]",
                   ToOffset(new Vector2(3 + 3)),
                   Color.Orange * alphaALL,
                     0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.1f);
                _spUI.DrawString(cutsceneFont,
                      "FOR = [" + actors_ennemies[currentEnnemyListID].Force + "]",
                       ToOffset(new Vector2(3 + 13)),
                       Color.Orange * alphaALL,
                         0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.1f);
                _spUI.DrawString(cutsceneFont,
                       "DEF = [" + actors_ennemies[currentEnnemyListID].Defense + "]",
                        ToOffset(new Vector2(3 + 23)),
                        Color.Orange * alphaALL,
                          0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.1f);


            }

            if(actors_friends.Count > 0)
            {
                _spUI.DrawString(cutsceneFont,
               "HP = [" + actors_friends[currentFriendListID].HP + "]",
                ToOffset(new Vector2(33 + 3)),
                Color.Orange * alphaALL,
                  0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.1f);
                _spUI.DrawString(cutsceneFont,
                      "FOR = [" + actors_friends[currentFriendListID].Force + "]",
                       ToOffset(new Vector2(33 + 13)),
                       Color.Orange * alphaALL,
                         0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.1f);
                _spUI.DrawString(cutsceneFont,
                       "DEF = [" + actors_friends[currentFriendListID].Defense + "]",
                        ToOffset(new Vector2(33 + 23)),
                        Color.Orange * alphaALL,
                          0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.1f);
                _spUI.DrawString(cutsceneFont,
                      "AP = [" + actors_friends[currentFriendListID].actionPoint + "]",
                       ToOffset(new Vector2(33 + 33)),
                       Color.Red * alphaALL,
                         0, Vector2.Zero, 0.6f, SpriteEffects.None, 0.1f);
            }
         */

            if (cursor == null) return;
            Rectangle mousecursorPosition = new Rectangle(UIcursorPos.X, UIcursorPos.Y, 20, 20);
            _spUI.Draw(cursor, mousecursorPosition, frame_cursor, Color.White * alphaALL);
            base.Draw_UI(ref _spUI);
        }

    }
}
