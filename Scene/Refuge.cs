using _TheShelter.GameWrapping;
using _TheShelter.RefugeSystem;
using DinguEngine;
using DinguEngine.Camera;
using DinguEngine.Refuge;
using DinguEngine.Shared;
using DinguEngine.UI;
using DinguEngine.UI.TE_Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TheShelter;

namespace _TheShelter.Scene
{

    public enum ActionTile
    {
        empty,
        buildDortoir,
        buildFastfood,
        buildfiltreeau,
        buildminecharbon,
        buildstockage,
        buildmagie,
        buildforge,
        buildbiere,
        buildstairs,
        kill_tile,
    }
    public class Refuge : ModelScene
    {
        ActorRefuge stampActorRefuge;
        TE_Button[] map;
        TE_Button[] cmd_buildings;

        //-- assets externes 
        Texture2D tex_entree, tex_buildings;
        Texture2D[] textures;
        Texture2D cursor;
        //-end- assets externes

        //-- curseur 
        Rectangle frame_cursor;
        Rectangle[] cursors;
        Point cursorPos;
        Point UIcursorPos;
        //-end- curseur

        //-- taille de la map (-> clone dans TE_Manager)
        int gridW = 11;
        int gridH = 9;
        int gridSize;

        int tileW, tileH;
        int frameW, frameH;


        Vector2 mover;
        Point oldmouseposition, mouseposition;
        KeyboardState old_kbs;

        int mouseticks = 0;

        int offsetY_UIPANEL = 0;

        TE_Window buildPanel;
        DrawRect[] buildPanel_rect;

        TE_Button[] builderbtns;

        bool focusOnUI = false;
        bool isShowing_BuilderPanel = false;
        bool canStampLayer = false;
        TE_Button btn_build, btn_exploration, btn_Exit, btn_Next_Turn;
        Texture2D statusTex;

        Rectangle population_rect, food_rect, water_rect, energy_rect;

        Rectangle bg_infos;
        Rectangle bg_btn_main_commands;
        Rectangle bg_btn_dayNightPanel;

        DrawRect pop_dr, food_dr, water_dr, energy_dr;
        Vector2[] infospositions;
        bool canDetectTiles = true;

        int state_playerIsBuildingTile = 0;
        int bank = 1500;
        Rectangle bankPosition;
        Rectangle bankIconPosition;
        DrawRect stamp;
        int stairs_counter = 0;


        float alphaPrice_remover = 0.0f;
        float alphaPrice_adder = 0.0f;
        string hitBank_remover = string.Empty;

        Rectangle bg_buildPanel;

        TE_Button[] build_btn_panel;

        List<int> centralStairsID = new List<int>();
        List<int> allStairsID = new List<int>();

        bool canDoWork = false;
        Song[] bgsongs;
        List<ActorRefuge> actor_friends = new List<ActorRefuge>();
        bool canShowPanelsCMD = false;
        ActionTile doWork;
        int map_iteraton_dragged = 0;
        bool dragdetected = false;
        bool deleteDraggedActor = false;
        Point dragcursor;

        int totalpopulation = 0;
        int totalpain = 0;
        int totaleau = 0;
        int totalenergie = 0;

        DrawRect[] daysVisual;

        TE_Button btn_music;
        DataRefuge data = new DataRefuge();

        //-- mode exploration
        TE_Button[] btn_explore_world;
        Rectangle bgExplore_mode;

        string newBankAccount = string.Empty;
        bool displayNewBankAccount = false;
        int ticker = 0;
        public Refuge(MainClass _mainclass) : base(_mainclass)
        {

        }


        #region <unit generator>
        private void Add_Friend_ActorRefuge(int amount)
        {
            if (map == null) return;

            for (int i = 0; i < amount; i++)
            {
                ActorRefuge actor =
                    new ActorRefuge(
                    new Rectangle(0, 25, 10, 15), // position
                    new Rectangle(3 * 20, 0, 20, 30)); // frame

                actor.Set_Owner_PositionWidth(60, 20, 4);

                map[0].actors.Add(actor);
            }
        }
        #endregion </unit generator>

        #region <UX - UI Visual loaders>
        
        private void SetExplorationMode()
        {
            bgExplore_mode = ToOffset(new Rectangle(8, 22, 160, 100));

            int tilesize = 28;
            int margx = 10;
            int margy = 24;
            int space = 8;

            int height_margin = tilesize + 4;
            Rectangle build1 = ToOffset(new Rectangle(margx, margy, tilesize, tilesize));
            Rectangle build2 = ToOffset(new Rectangle(margx + tilesize + space, margy, tilesize, tilesize));
            Rectangle build3 = ToOffset(new Rectangle(margx + tilesize * 2 + space * 2, margy, tilesize, tilesize));

            Rectangle build4 = ToOffset(new Rectangle(margx, margy + height_margin, tilesize, tilesize));
            Rectangle build5 = ToOffset(new Rectangle(margx + space + tilesize, margy + height_margin, tilesize, tilesize));
            Rectangle build6 = ToOffset(new Rectangle(margx + space * 2 + tilesize * 2, margy + height_margin, tilesize, tilesize));

            Rectangle build7 = ToOffset(new Rectangle(margx, margy + height_margin * 2, tilesize, tilesize));
            Rectangle build8 = ToOffset(new Rectangle(margx + space + tilesize, margy + height_margin * 2, tilesize, tilesize));
            Rectangle build9 = ToOffset(new Rectangle(margx + space * 2 + tilesize * 2, margy + height_margin * 2, tilesize, tilesize));

            Rectangle build10 = ToOffset(new Rectangle(margx + space * 2 + tilesize * 3 + 8, margy, 24, 24));

            DrawRect dr1 = new DrawRect(build1, 64);
            DrawRect dr2 = new DrawRect(build2, 64);
            DrawRect dr3 = new DrawRect(build3, 64);
            DrawRect dr4 = new DrawRect(build4, 64);
            DrawRect dr5 = new DrawRect(build5, 64);
            DrawRect dr6 = new DrawRect(build6, 64);
            DrawRect dr7 = new DrawRect(build7, 64);
            DrawRect dr8 = new DrawRect(build8, 64);
            DrawRect dr9 = new DrawRect(build9, 64);

            DrawRect dr10 = new DrawRect(build10, 64);

            btn_explore_world = new TE_Button[]
            {
                new TE_Button(dr1,new int2(1,0),64), //-- 0 dortoir
                new TE_Button(dr2,new int2(2,0),64), //-- 1 fastfood
                new TE_Button(dr3,new int2(3,0),64), //-- 2 filtre eau
                new TE_Button(dr4,new int2(4,0),64), //-- 3 mine
                new TE_Button(dr5,new int2(5,0),64), //-- 4 stockage
                new TE_Button(dr6,new int2(6,0),64), //-- 5 magie
                new TE_Button(dr7,new int2(7,0),64), //-- 6 forge
                new TE_Button(dr8,new int2(8,0),64), //-- 7 biere
                new TE_Button(dr9,new int2(9,0),64), //-- 8 escalier (stairs)

                new TE_Button(dr10,new int2(9,0),24), //-- 9 !!! sprite != icone:détruire!
            };

            for (int i = 0; i < btn_explore_world.Length; i++)
            {
                if (i == btn_explore_world.Length - 1)
                {
                    btn_explore_world[i].drawrect.textureID = 4;
                }
                else
                {
                    btn_explore_world[i].drawrect.textureID = 5;
                }
            }

        }

        private void SetBuildPanel()
        {
            bg_buildPanel = ToOffset(new Rectangle(8, 22, 160, 100));

            int tilesize = 28;
            int margx = 10;
            int margy = 24;
            int space = 8;

            int height_margin = tilesize + 4;
            Rectangle build1 = ToOffset(new Rectangle(margx, margy, tilesize, tilesize));
            Rectangle build2 = ToOffset(new Rectangle(margx + tilesize + space, margy, tilesize, tilesize));
            Rectangle build3 = ToOffset(new Rectangle(margx + tilesize * 2 + space * 2, margy, tilesize, tilesize));

            Rectangle build4 = ToOffset(new Rectangle(margx, margy+height_margin, tilesize, tilesize));
            Rectangle build5 = ToOffset(new Rectangle(margx + space + tilesize, margy + height_margin, tilesize, tilesize));
            Rectangle build6 = ToOffset(new Rectangle(margx + space * 2 + tilesize * 2, margy + height_margin, tilesize, tilesize));

            Rectangle build7 = ToOffset(new Rectangle(margx, margy + height_margin *2, tilesize, tilesize));
            Rectangle build8 = ToOffset(new Rectangle(margx + space + tilesize, margy + height_margin *2, tilesize, tilesize));
            Rectangle build9 = ToOffset(new Rectangle(margx + space * 2 + tilesize*2, margy + height_margin *2, tilesize, tilesize));
            
            Rectangle build10 = ToOffset(new Rectangle(margx + space * 2 + tilesize*3+8, margy, 24, 24));

            DrawRect dr1 = new DrawRect(build1, 64);
            DrawRect dr2 = new DrawRect(build2, 64);
            DrawRect dr3 = new DrawRect(build3, 64);
            DrawRect dr4 = new DrawRect(build4, 64);
            DrawRect dr5 = new DrawRect(build5, 64);
            DrawRect dr6 = new DrawRect(build6, 64);
            DrawRect dr7 = new DrawRect(build7, 64);
            DrawRect dr8 = new DrawRect(build8, 64);
            DrawRect dr9 = new DrawRect(build9, 64);

            DrawRect dr10 = new DrawRect(build10, 64);

            build_btn_panel = new TE_Button[]
            {
                new TE_Button(dr1,new int2(1,0),64), //-- 0 dortoir
                new TE_Button(dr2,new int2(2,0),64), //-- 1 fastfood
                new TE_Button(dr3,new int2(3,0),64), //-- 2 filtre eau
                new TE_Button(dr4,new int2(4,0),64), //-- 3 mine
                new TE_Button(dr5,new int2(5,0),64), //-- 4 stockage
                new TE_Button(dr6,new int2(6,0),64), //-- 5 magie
                new TE_Button(dr7,new int2(7,0),64), //-- 6 forge
                new TE_Button(dr8,new int2(8,0),64), //-- 7 biere
                new TE_Button(dr9,new int2(9,0),64), //-- 8 escalier (stairs)

                new TE_Button(dr10,new int2(9,0),24), //-- 9 !!! sprite != icone:détruire!
            };

            for (int i = 0; i < build_btn_panel.Length; i++)
            {
                if (i == build_btn_panel.Length - 1)
                {
                    build_btn_panel[i].drawrect.textureID = 4;
                }
                else
                {
                    build_btn_panel[i].drawrect.textureID = 5;
                }
            }

        }
        private void SetBankUI()
        {
            bankPosition = ToOffset(new Rectangle(8, 8, 54, 10));
            bankIconPosition = ToOffset(new Rectangle(12, 9, 8, 8));
        }
        private void SetUIButtons()
        {
            int btn_cmdsize = 16;
            int btn_cmdposY = 130;

            bg_btn_main_commands = ToOffset(new Rectangle(8, btn_cmdposY - 2, btn_cmdsize * 2 + 8, btn_cmdsize + 4));
            //-- bouton pour afficher panneau de construction
            Rectangle temp = ToOffset(new Rectangle(10, btn_cmdposY, btn_cmdsize, btn_cmdsize));
            DrawRect rect = new DrawRect(temp, 16);
            btn_build = new TE_Button(rect, new int2(0, 0), 16);
            btn_build.drawrect.frame = new Rectangle(5 * 24, 0, 24, 24);

            //-- bouton pour afficher la map d'exploration
            Rectangle tempex = ToOffset(new Rectangle(8 + btn_cmdsize + 4, btn_cmdposY, btn_cmdsize, btn_cmdsize));
            DrawRect rectex = new DrawRect(tempex, 16);
            btn_exploration = new TE_Button(rectex, new int2(0, 0), 16);
            btn_exploration.drawrect.frame = new Rectangle(6 * 24, 0, 24, 24);

         
            //-- bouton pour quitter jeu
            Rectangle tempnt = ToOffset(new Rectangle(198, btn_cmdposY, btn_cmdsize, btn_cmdsize));
            DrawRect rectnt = new DrawRect(tempnt, 16);
            btn_Exit = new TE_Button(rectnt, new int2(0, 0), 16);
            btn_Exit.drawrect.frame = new Rectangle(7 * 24, 0, 24, 24);

            SetDayNightPanel();
        }

        int currentDay = 8;

        private void SetDayNightPanel()
        {
            int btn_cmdsize = 16;
            int btn_cmdposY = 130;

            bg_btn_dayNightPanel = ToOffset(new Rectangle(22 + btn_cmdsize * 2, btn_cmdposY - 2, btn_cmdsize * 5+ 12, btn_cmdsize + 4));


            //-- bouton pour passer le tour
            Rectangle tempext = ToOffset(new Rectangle(24 + btn_cmdsize * 2, btn_cmdposY, btn_cmdsize, btn_cmdsize));
            DrawRect rectext = new DrawRect(tempext, 16);
            btn_Next_Turn = new TE_Button(rectext, new int2(0, 0), 16);
            btn_Next_Turn.drawrect.frame = new Rectangle(8 * 24, 0, 24, 24);

            daysVisual = new DrawRect[7];

            int clipsize = 10;
            for (int i = 0; i < 7; i++)
            {

                Rectangle position = ToOffset(new Rectangle(24 + btn_cmdsize * 3 + clipsize * i + 2, btn_cmdposY + 2, 10, 10));
                DrawRect day = new DrawRect(position, 24);
                day.SetFrame(i, 0, 24);
                day.DEFAULT_COLOR = Color.White * 0.4f;
                daysVisual[i] = day;
                if (i != 0)
                {
                    daysVisual[i].color = Color.White * 0.4f;
                }
            }

        }

        private void SetInformationsPanel()
        {
            int offsetX = 180;
            int width = 240 - offsetX - 2;
            int height = 60 + 14;

            bg_infos = ToOffset(new Rectangle(offsetX - 2, 6, width, height));
            //-- UI : donne informations au joueur
            population_rect = ToOffset(new Rectangle(offsetX, 8, 14, 14));
            food_rect = ToOffset(new Rectangle(offsetX, 46, 14, 14));
            water_rect = ToOffset(new Rectangle(offsetX, 60, 14, 14));
            energy_rect = ToOffset(new Rectangle(offsetX, 30, 14, 14));

            //-- position des informations
            infospositions = new Vector2[]
            {
                ToOffset(new Vector2(offsetX + 16,8)),
                ToOffset(new Vector2(offsetX + 16,46)),
                ToOffset(new Vector2(offsetX + 16,60)),
                ToOffset(new Vector2(offsetX + 16,30)),
            };

            pop_dr = new DrawRect(population_rect, 24);
            food_dr = new DrawRect(food_rect, 24);
            water_dr = new DrawRect(water_rect, 24);
            energy_dr = new DrawRect(energy_rect, 24);

            pop_dr.frame = new Rectangle(0 * 24, 0, 24, 24);
            food_dr.frame = new Rectangle(1 * 24, 0, 24, 24);
            water_dr.frame = new Rectangle(2 * 24, 0, 24, 24);
            energy_dr.frame = new Rectangle(3 * 24, 0, 24, 24);
        }
        private void SetMouseCursor()
        {
            frame_cursor = new Rectangle(0, 0, 8, 8);
            cursors = new Rectangle[]
            {
                new Rectangle(0, 0, 16, 16),
                new Rectangle(16, 0, 16, 16),
                new Rectangle(0, 16, 16, 16),
                new Rectangle(16, 16, 16, 16),
                new Rectangle(0, 32, 16, 16),
            };
        }
        #endregion </UX - UI Visual loaders>

        #region <Externals Assets Loaders>
        private void LoadTextures(ref ContentManager _content)
        {
            if (_content == null) return;

            statusTex = _content.Load<Texture2D>("statusBar");

            cursor = _content.Load<Texture2D>("system\\cursor");
           


            textures = new Texture2D[]
            {
                 _content.Load<Texture2D>("Tilesets\\entree"), //0
                 _content.Load<Texture2D>("Tilesets\\accueil"), //1
                 _content.Load<Texture2D>("Tilesets\\stairs"), //2
                 _content.Load<Texture2D>("Tilesets\\build_standard"), //3
                 _content.Load<Texture2D>("Tilesets\\UIIcons"), //4
                _content.Load<Texture2D>("Tilesets\\builder_icon"), // 5 - panneau de construction
                _content.Load<Texture2D>("Tilesets\\build_s1"), //6
                _content.Load<Texture2D>("Tilesets\\build_s2"), //7
                _content.Load<Texture2D>("Tilesets\\stockage"), //8
                _content.Load<Texture2D>("units\\refuge\\units_refuge"), //9 -- unités 2d
                _content.Load<Texture2D>("Tilesets\\days"), //10 -- cycle jour nuit
                _content.Load<Texture2D>("Tilesets\\exploration"), //11 -- carte d'exploration
            };
        }
        private void GetSongs(ref ContentManager _content)
        {
            bgsongs = new Song[]
            {
                _content.Load<Song>("Songs\\refuge\\OST 1 - Beyond Infinity (Loopable)"),
                _content.Load<Song>("Songs\\refuge\\OST 2 - Lantern Light Lore (Loopable)"),
                _content.Load<Song>("Songs\\refuge\\OST 3 - Wanderers' Whispers (Loopable)"),
                _content.Load<Song>("Songs\\refuge\\OST 4 - Fireside Legends (Loopable)"),
               _content.Load<Song>("Songs\\refuge\\OST 5 - Moonlit Myths (Loopable)"),
            };


            Rectangle temp = ToOffset(new Rectangle(220, 130, 16, 16));
            DrawRect fr = new DrawRect(temp, 24);
            btn_music = new TE_Button(fr, new int2(10, 0),24);
        }


        bool isplaying = true;
        private void PunchMusic()
        {
            isplaying = !isplaying;

            if (isplaying)
            {
                PlayMusic();
            }
            else
            {
                MediaPlayer.Stop();
            }

        }
        private void PlayMusic()
        {
               int rand = Randomizer.GiveRandomInt(0, bgsongs.Length);
               if (rand >= 5) rand = 4;
                MediaPlayer.Play(bgsongs[rand]);
            
        }
        SoundEffect[] audio;
        private void LoadAudio(ref  ContentManager _content)
        {
            audio = new SoundEffect[]
            {
            _content.Load<SoundEffect>("Audio\\refuge\\GS2_Item_Acquire_5"), // 0 - next day
            _content.Load<SoundEffect>("Audio\\refuge\\UI_Menu_Open"), // 1 - open menu
            _content.Load<SoundEffect>("Audio\\refuge\\UI2_Start_3"), // 2 - next day end
            _content.Load<SoundEffect>("Audio\\refuge\\GS2_Jump_2"), // 3 - drag actor
            _content.Load<SoundEffect>("Audio\\refuge\\GS2_Land"), // 4 - drop actor
            _content.Load<SoundEffect>("Audio\\refuge\\UI_Button_Disable"), // 5 - btn build on
            _content.Load<SoundEffect>("Audio\\refuge\\UI_Button_Enable"), // 6 - btn build off
            _content.Load<SoundEffect>("Audio\\refuge\\UI2_Decline_3"), // 7 - btn build no enough cash
            _content.Load<SoundEffect>("Audio\\refuge\\Spell_Thunder_1"), // 8 - kill object
            };
        }
        #endregion </Externals Assets Loaders>

        #region <Grid Generator>
        private void GenerateMap()
        {
           

            gridSize = gridW * gridH;
            map = new TE_Button[gridSize];

            int line = -1;
            int column = 0;
            for (int i = 0; i < gridSize; i++)
            {
                if (i % this.gridW == 0)
                {
                    column = 0;
                    line++;
                }
                else
                {
                    column++;
                }

              

                //-- poser les tuiles --
                int posX = column * (tileW);
                if(column!=0 && line>=2)
                {
                    posX = column * (tileW)+tileW/2;
                }
                int posY = line * (tileH) + offsetY_UIPANEL;
                int rectWidth = tileW;

                if (column == 1 || column == 4 || column == 9) rectWidth = tileW / 2;
                if (column > 9)
                {
                    posX -= (int)(tileW * 1.5f);
                }
                else if (column > 4)
                {
                    posX -= tileW;

                }
                else if (column > 1)
                {
                    posX -= tileW / 2;

                }

                DrawRect temp = new DrawRect(posX, posY, rectWidth, tileH);
                temp.color = Color.White;
                temp.alpha = 1.0f;
                temp.gridID = i;
                temp.textureID = 3;

                if (line == 0)
                {
                    //-- le plafond
                    if (column == gridW - 1) temp.position = new Rectangle(posX, posY, tileW * 3, tileH);

                    temp.SetFrame(1, 0, frameW, frameH);

                    map[i] = new TE_Button(temp, new int2(1, 0), frameW, frameH);

                    map[i].style = build_style.forbidden;
                }
                else if ((column == 0) || (column == gridW - 1))
                {
                    //-- les bords à gauche et à droite
                    if (column == gridW - 1) temp.position = new Rectangle(posX, posY, tileW * 3, tileH);
                    if (column == 0) temp.position = new Rectangle(posX, posY, tileW +tileW/2, tileH);
                    temp.SetFrame(0, 0, frameW, frameH);

                    map[i] = new TE_Button(temp, new int2(0, 0), frameW, frameH);
                    map[i].style = build_style.forbidden;

                }
                else
                {
                    
                    //-- le reste 
                    if (column == 1 || column == 4 || column == 9)
                    {
                        //-- les escaliers sur les côtés --
                        allStairsID.Add(i);
                        temp.textureID = 3;
                        temp.SetFrame(4, 0, frameW / 2, frameH);
                        map[i] = new TE_Button(temp, new int2(4, 0), frameW / 2, frameH);
                        map[i].style = build_style.empty;
                    
                        if(column==4) { centralStairsID.Add(i); }
                    }
                    else
                    {
                        //-- tuile à construire
                        temp.textureID = 3;
                        temp.SetFrame(1, 0, frameW, frameH);

                        map[i] = new TE_Button(temp, new int2(1, 0), frameW, frameH);
                        map[i].style = build_style.empty;
                    }
                }

                map[i].columnInMap = column;
                map[i].rowInMap = line;
                map[i].gridID = i;
            }

            //-- Assembler les tuiles de l'entrée du refuge (2x2)
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    //-- entrée du refuge --
                    map[i].drawrect.position = new Rectangle(0, offsetY_UIPANEL, (int)(tileW * 1.5f), tileH * 2);
                    map[i].drawrect.textureID = 0;
                    map[i].drawrect.SetFrame(0, 0, 200, frameH * 2);
                    map[i].style = build_style.outside_entrance;
                }
                else
                {
                    map[i].drawrect.position = new Rectangle(0, 0, 0, 0);
                }
            }

            //-- désactiver les tuiles en trop
            for (int i = gridW; i < (gridW + 2); i++)
            {
                map[i].drawrect.position = new Rectangle(0, 0, 0, 0);
            }

            //-- Assembler les tuiles de la salle d'accueil refuge (1x2)
            int rectX = map[13].drawrect.position.X;
            int rectY = map[13].drawrect.position.Y;
            map[13].drawrect.textureID = 1;
            map[13].drawrect.frame = new Rectangle(0, 0, frameW * 2, frameH);
            map[13].drawrect.position = new Rectangle(rectX, rectY, 80, tileH);
            map[13].style = build_style.accueil;
            map[14].style = build_style.accueil;

            map[14].drawrect.position = Rectangle.Empty;
        }

        private void GenerateCentralStairs()
        {
              BUILD_Stairs(centralStairsID[0]);
            BUILD_Stairs(centralStairsID[1]) ;
        }
        private void GenerateDwarvesMines()
        {
            //-- construire les mines naines

            int minefer_ID = 16;
            int minefer_mithril = 18;
            int minefer_adamantium = 19;

            map[minefer_ID].style = build_style.minefer;
            map[minefer_mithril].style = build_style.minemitrhil;
            map[minefer_adamantium].style = build_style.mineadmantium;

          

            map[minefer_ID].drawrect.textureID = 6;
            map[minefer_mithril].drawrect.textureID = 6;
            map[minefer_adamantium].drawrect.textureID = 6;

            int width = 80;
            int height = 40;

            map[minefer_ID].drawrect.frame = new Rectangle(2 * width, 0, width, height);
            map[minefer_mithril].drawrect.frame = new Rectangle(3 * width, 0, width, height);
            map[minefer_adamantium].drawrect.frame = new Rectangle(4 * width, 0, width, height);

            map[minefer_ID].drawrect.DEFAULT_COLOR = Color.White;
            map[minefer_mithril].drawrect.DEFAULT_COLOR = Color.White;
            map[minefer_adamantium].drawrect.DEFAULT_COLOR = Color.White;

            map[minefer_ID].state = buildingState.ishidden;
            map[minefer_mithril].state = buildingState.ishidden;
            map[minefer_adamantium].state = buildingState.ishidden;
        }
        #endregion </Grid Generator>

        private void ReadDataRefuge()
        {
            //-- recopier les données dans le programme

            totalbuilddortoir = data.totalbuilddortoir;
            totalbuildfasfood = data.totalbuildfasfood;
            totalbuildminecharbon = data.totalbuildminecharbon;
            totalbuildmagic = data.totalbuildmagic; 
            totalbuildforge = data.totalbuildforge; 
            totalbuildfiltreeau = data.totalbuildfiltreeau;
            totalbuildbiere = data.totalbuildbiere;
            totalbuildstockage = data.totalbuildstockage;
            totaleau = data.totaleau;
            totalpain = data.totalpain;
            totalpopulation = data.totalpopulation;
            totalenergie = data.totalenergie;
            bank = data.bank;
            currentDay = data.currentDay;
            daysVisual = data.daysVisual;
            //startdayfromwrapper = true;
        }

        public override void Load(ref ContentManager _content)
        {
           
            maincamera = new TE_Camera( );
            uicamera = new TE_Camera( );
            friendcamera = new TE_Camera( );
            ennemycamera = new TE_Camera( );

            //-- <Charger Assets Exterieurs>
            LoadTextures(ref _content);
            GetSongs(ref _content);
            PlayMusic();
            LoadAudio(ref _content);
            //-end- </Charger Assets Exterieurs>

            playBuildingSound = new Action(PlayAudioBuilding);

            //-- <Générer la MAP> --
            tileW = TE_Manager.tileW;
            tileH = TE_Manager.tileH;

            //-- dimensions des sprites--
            frameW = 80;
            frameH = 40;

            if(RefugeWrapper.stateWrapping==0)
            {
                //-- un refuge n'est PAS en mémoire
                GenerateMap();
                GenerateDwarvesMines();
                GenerateCentralStairs();
                BUILD_Dortoir(24);
                Add_Friend_ActorRefuge(4);
            }
            else
            {
                //-- on charge un refuge en mémoire
                GenerateMap();
                GenerateDwarvesMines();
                GenerateCentralStairs();
                RefugeWrapper.LoadDataWrapper_MAP(ref map);
                RefugeWrapper.LoadDataRefuge(ref data);

                ReadDataRefuge();
               if(ticker==0)
                {
                    ticker++;
                    StartANewDay =true;
                }
                

                bank += TE_Manager.bank_bonus;
                displayNewBankAccount = true;
            }
            
            //-end- </Générer la map>

            //-- <Générer l'interface Utilisateur>
            SetUIButtons();
            SetInformationsPanel();
            SetBuildPanel();
            SetBankUI();
            SetMouseCursor();
            //-end- </Générer l'interface Utilisateur>


            UpdateBuilderPanelButtons();

            base.Load(ref _content);
        }

        #region <Building behaviours>

        private bool BUILD_Stairs(int iterationMapGrid)
        {
            map[iterationMapGrid].drawrect.textureID = 2;
            map[iterationMapGrid].drawrect.SetFrame(0, 0, frameW / 2, frameH);
            map[iterationMapGrid].style = build_style.stairs;
            return true;
        }

        private bool BUILD_FiltreEau(int iterationMapGrid)
        {
            //-rappel- frameW = 80; frameH == 40;
            map[iterationMapGrid].drawrect.textureID = 6;
            map[iterationMapGrid].drawrect.SetFrame(0, 0, frameW, frameH);
            map[iterationMapGrid].style = build_style.filtreEau;
            map[iterationMapGrid].state = buildingState.canRead;

            return true;
        }

        private bool BUILD_Biere(int iterationMapGrid)
        {
            //-rappel- frameW = 80; frameH == 40;
            map[iterationMapGrid].drawrect.textureID = 6;
            map[iterationMapGrid].drawrect.SetFrame(1, 0, frameW, frameH);
            map[iterationMapGrid].style = build_style.biere;
            map[iterationMapGrid].state = buildingState.canRead;

            return true;
        }

        private bool BUILD_Dortoir(int iterationMapGrid)
        {
            //-- canBuild? --
            int iteration_up = iterationMapGrid + 1;
            if (map[iteration_up].style != build_style.empty)
            {
                //todo message cannot build here!!!
                return false;
            }
            else
            {
                //-- destroy iteration_up
                map[iteration_up].style = build_style.mergedTile;
                map[iteration_up].merged_style = build_style.dortoir;
            }


            //-rappel- frameW = 80; frameH == 40;
            map[iterationMapGrid].drawrect.textureID = 7;
            map[iterationMapGrid].drawrect.DoubleSizePosition(tileW);
            if (map[iterationMapGrid].columnInMap == 3 || map[iterationMapGrid].columnInMap == 8)
            {
                map[iterationMapGrid].drawrect.DoubleSizePosition_Merged(tileW);
            }
            map[iterationMapGrid].drawrect.SetFrame(0, 0, frameW * 2, frameH);
            map[iterationMapGrid].style = build_style.dortoir;
            map[iterationMapGrid].state = buildingState.canRead;

            return true;
        }

        private bool BUILD_FastFood(int iterationMapGrid)
        {
            //-- canBuild? --
            int iteration_up = iterationMapGrid + 1;
            if (map[iteration_up].style != build_style.empty)
            {
                //todo message cannot build here!!!
                return false;
            }
            else
            {
                //-- destroy iteration_up
                map[iteration_up].style = build_style.mergedTile;
                map[iteration_up].merged_style = build_style.fastfood;

            }


            //-rappel- frameW = 80; frameH == 40;
            map[iterationMapGrid].drawrect.textureID = 7;
            map[iterationMapGrid].drawrect.DoubleSizePosition(tileW);
            if (map[iterationMapGrid].columnInMap == 3 || map[iterationMapGrid].columnInMap == 8)
            {
                map[iterationMapGrid].drawrect.DoubleSizePosition_Merged(tileW);
            }
            map[iterationMapGrid].drawrect.SetFrame(1, 0, frameW * 2, frameH);
            map[iterationMapGrid].style = build_style.fastfood;
            map[iterationMapGrid].state = buildingState.canRead;
            return true;
        }

        private bool BUILD_MineCharbon(int iterationMapGrid)
        {
            //-- canBuild? --
            int iteration_up = iterationMapGrid + 1;
            if (map[iteration_up].style != build_style.empty)
            {
                //todo message cannot build here!!!
                return false;
            }
            else
            {
                //-- destroy iteration_up
                map[iteration_up].style = build_style.mergedTile;
                map[iteration_up].merged_style = build_style.minedecharbon;

            }


            //-rappel- frameW = 80; frameH == 40;
            map[iterationMapGrid].drawrect.textureID = 7;
            map[iterationMapGrid].drawrect.DoubleSizePosition(tileW);
            if (map[iterationMapGrid].columnInMap == 3 || map[iterationMapGrid].columnInMap == 8)
            {
                map[iterationMapGrid].drawrect.DoubleSizePosition_Merged(tileW);
            }
            map[iterationMapGrid].drawrect.SetFrame(2, 0, frameW * 2, frameH);
            map[iterationMapGrid].style = build_style.minedecharbon;
            map[iterationMapGrid].state = buildingState.canRead;
            return true;
        }

        private bool BUILD_LaboMagie(int iterationMapGrid)
        {
            //-- canBuild? --
            int iteration_up = iterationMapGrid + 1;
            if (map[iteration_up].style != build_style.empty)
            {
                //todo message cannot build here!!!
                return false;
            }
            else
            {
                //-- destroy iteration_up
                map[iteration_up].style = build_style.mergedTile;
                map[iteration_up].merged_style = build_style.labomagie;

            }


            //-rappel- frameW = 80; frameH == 40;
            map[iterationMapGrid].drawrect.textureID = 7;
            map[iterationMapGrid].drawrect.DoubleSizePosition(tileW);
            if (map[iterationMapGrid].columnInMap == 3 || map[iterationMapGrid].columnInMap == 8)
            {
                map[iterationMapGrid].drawrect.DoubleSizePosition_Merged(tileW);
            }
            map[iterationMapGrid].drawrect.SetFrame(3, 0, frameW * 2, frameH);
            map[iterationMapGrid].style = build_style.labomagie;
            map[iterationMapGrid].state = buildingState.canRead;
            return true;
        }

        private bool BUILD_Forge(int iterationMapGrid)
        {
            //-- canBuild? --
            int iteration_up = iterationMapGrid + 1;
            if (map[iteration_up].style != build_style.empty)
            {
                //todo message cannot build here!!!
                return false;
            }
            else
            {
                //-- destroy iteration_up
                map[iteration_up].style = build_style.mergedTile;
                map[iteration_up].merged_style = build_style.forge;

            }


            //-rappel- frameW = 80; frameH == 40;
            map[iterationMapGrid].drawrect.textureID = 7;
            map[iterationMapGrid].drawrect.DoubleSizePosition(tileW);

            if (map[iterationMapGrid].columnInMap == 3 || map[iterationMapGrid].columnInMap == 8)
            {
                map[iterationMapGrid].drawrect.DoubleSizePosition_Merged(tileW);
            }
            map[iterationMapGrid].drawrect.SetFrame(4, 0, frameW * 2, frameH);
            map[iterationMapGrid].style = build_style.forge;
            map[iterationMapGrid].state = buildingState.canRead;
            return true;
        }

        private bool BUILD_Stockage(int iterationMapGrid)
        {
            //-- canBuild? --
            int iteration_up = iterationMapGrid + 1;
            if (map[iteration_up].style != build_style.empty)
            {
                //todo message cannot build here!!!
                return false;
            }
            else
            {
                //-- destroy iteration_up
                map[iteration_up].style = build_style.mergedTile;
                map[iteration_up].merged_style = build_style.salledestockage;

            }
            int iteration_up_up = iterationMapGrid + 2;
            if (iteration_up_up > map.Length - 1) { return false; }

            if (map[iteration_up_up].style != build_style.empty)
            {
                //todo message cannot build here!!!
                return false;
            }
            else
            {
                //-- destroy iteration_up
                map[iteration_up_up].style = build_style.mergedTile;
                map[iteration_up].merged_style = build_style.salledestockage;

            }


            //-rappel- frameW = 80; frameH == 40;
            map[iterationMapGrid].drawrect.textureID = 8;

            map[iterationMapGrid].drawrect.TripleSizePosition(tileW);

            if (map[iterationMapGrid].columnInMap == 2 || map[iterationMapGrid].columnInMap==7)
            {
                map[iterationMapGrid].drawrect.TripleSizePositionMerged(tileW);
            }
            map[iterationMapGrid].drawrect.SetFrame(0, 0, frameW * 3, frameH);
            map[iterationMapGrid].style = build_style.salledestockage;
            map[iterationMapGrid].state = buildingState.canRead;
            return true;
        }

        private bool KillBuilding(int iterationMapGrid)
        {
            if (map[iterationMapGrid].style == build_style.empty) { return false; }
            if (map[iterationMapGrid].style == build_style.forbidden) { return false; }
            if (map[iterationMapGrid].rowInMap == 0) { return false; }
            if (map[iterationMapGrid].columnInMap == 0) { return false; }
            if (map[iterationMapGrid].columnInMap == 10) { return false; }
            if (map[iterationMapGrid].rowInMap == 1
                && map[iterationMapGrid].columnInMap == 2) { return false; }
            if (map[iterationMapGrid].rowInMap == 1
                && map[iterationMapGrid].columnInMap == 3) { return false; }
            if (map[iterationMapGrid].rowInMap == 1
                && map[iterationMapGrid].columnInMap == 4) { return false; }
            if (map[iterationMapGrid].rowInMap == 1
                && map[iterationMapGrid].columnInMap == 5) { return false; }
            if (map[iterationMapGrid].rowInMap == 1
                && map[iterationMapGrid].columnInMap == 7) { return false; }
            if (map[iterationMapGrid].rowInMap == 1
                && map[iterationMapGrid].columnInMap == 8) { return false; }

            if (map[iterationMapGrid].style != build_style.empty)
            {
                map[iterationMapGrid].style = build_style.empty;
                map[iterationMapGrid].drawrect.ResetTile_noStair();
                if (map[iterationMapGrid].columnInMap == 4)
                {
                    map[iterationMapGrid].drawrect.frame = new Rectangle(5 * 40, 0, 40, 40);
                }
                else
                {
                    map[iterationMapGrid].drawrect.frame = new Rectangle(80, 0, 80, 40);
                }
                
                //-- déplacer à l'extérieur du refuge les actors
             /*
              *  feature à coder ++++++++++ tard :/
              * */

                map[iterationMapGrid].actors.Clear();

            }

            if (map[iterationMapGrid + 1].style == build_style.mergedTile)
            {
                map[iterationMapGrid + 1].style = build_style.empty;
                map[iterationMapGrid + 1].drawrect.ResetTile_noStair();
                if (map[iterationMapGrid + 1].columnInMap == 4)
                {
                    map[iterationMapGrid + 1].drawrect.frame = new Rectangle(5 * 40, 0, 40, 40);
                }
                else
                {
                    map[iterationMapGrid + 1].drawrect.frame = new Rectangle(80, 0, 80, 40);
                }
            }

            if ((iterationMapGrid + 2) < gridSize
                && map[iterationMapGrid + 2].style == build_style.mergedTile)
            {
                map[iterationMapGrid + 2].style = build_style.empty;
                map[iterationMapGrid + 2].drawrect.ResetTile_noStair();
                if (map[iterationMapGrid + 2].columnInMap == 4)
                {
                    map[iterationMapGrid + 2].drawrect.frame = new Rectangle(5 * 40, 0, 40, 40);
                }
                else
                {
                    map[iterationMapGrid + 2].drawrect.frame = new Rectangle(80, 0, 80, 40);
                }
            }

            return true;
        }


        private bool CanBuildStairs_Tile(ref int iteration)
        {
            if (map[iteration].style != build_style.empty) { return false; }
            int column = map[iteration].columnInMap;
            if (column != 1 && column != 4 && column != 9) return false;

            int iteration_UP = iteration - 11;

            if (iteration_UP < gridW) { return false; }

            if (map[iteration_UP].style == build_style.stairs
                && map[iteration].style == build_style.empty) { return true; }

            if (column == 1)
            {
                if (map[iteration + 1].style == build_style.empty) { return false; }
                else { return true; }
            }

            if (column == 9)
            {
                if (map[iteration - 1].style == build_style.empty) { return false; }
                else { return true; }


            }

            if (column == 4)
            {
                if (map[iteration + 1].style == build_style.empty
                    && map[iteration - 1].style == build_style.empty) { return false; }
            }

            return false;
        }

        private bool CanBuildSize1_Tile(ref int iteration)
        {
            int column = map[iteration].columnInMap;
            if (column == 1 && column == 4 && column == 9) return false;

            int temp_up = iteration + 1;
            int temp_down = iteration - 1;

            if (temp_up >= gridSize) return false;
            if (temp_down < gridW) return false;

            if (map[iteration].style != build_style.empty) return false;
            if (map[temp_up].style == build_style.forbidden) return false;
            if (map[temp_down].style == build_style.forbidden) return false;

            if (map[temp_up].style != build_style.empty
                && map[iteration].style == build_style.empty) return true;
            if (map[temp_down].style != build_style.empty
                && map[iteration].style == build_style.empty) return true;
            return false;
        }

        private bool CanBuildSize2_Tile(ref int iteration)
        {
            int temp_up = iteration + 1;
            int temp_up_up = iteration + 1 + 1;
            int temp_down = iteration - 1;
            if (temp_up > gridSize - 1) { return false; }
            if (temp_up_up > gridSize - 1) { return false; }
            if (temp_down <= gridW) { return false; }

            if (map[iteration].style != build_style.empty) { return false; }
            if (map[temp_up].style != build_style.empty) { return false; }
            if (map[temp_up].style == build_style.forbidden) { return false; }

            if (map[temp_up_up].style == build_style.forbidden
                && map[temp_down].style == build_style.empty) { return false; }

            if (map[temp_down].style == build_style.empty
                && map[temp_up_up].style == build_style.empty) { return false; }
            if (map[temp_down].style == build_style.forbidden &&
                map[temp_up_up].style == build_style.empty) { return false; }

            return true;
        }

        private bool CanBuildSize3_Tile(ref int iteration)
        {
            int temp_up = iteration + 1;
            int temp_up_up = iteration + 1 + 1;
            int temp_up_up_up = iteration + 1 + 1 + 1;
            int temp_down = iteration - 1;
            if (temp_up > gridSize - 1) { return false; }
            if (temp_up_up > gridSize - 1) { return false; }
            if (temp_up_up_up > gridSize - 1) { return false; }
            if (temp_down <= gridW) { return false; }

            if (map[temp_up].style != build_style.empty) { return false; }
            if (map[temp_up_up].style != build_style.empty) { return false; }
            if (map[temp_up_up_up].style != build_style.empty) { return false; }

            if (map[iteration].style != build_style.empty) { return false; }

            if (map[temp_up].style != build_style.empty) { return false; }
            if (map[temp_up].style == build_style.forbidden) { return false; }

            if (map[temp_up_up].style == build_style.forbidden
                && map[temp_down].style == build_style.empty) { return false; }

            if (map[temp_down].style == build_style.empty
                && map[temp_up_up].style == build_style.empty) { return false; }
            if (map[temp_down].style == build_style.forbidden &&
                map[temp_up_up].style == build_style.empty) { return false; }

            return true;
        }

        #endregion </Building behaviours>

        #region <Main buttons UX>
        private void IsBuildBtnClicked(ref Point mouseposition, ref bool isclicked, ref bool detectsTile)
        {
            if (!isclicked) { mouseticks = 0; }
            //-- opérations bouton build
            if (btn_build.IsCollide(mouseposition))
            {
                btn_build.drawrect.color = Color.Orange;
                detectsTile = false;

                if (isclicked && mouseticks == 0)
                {
                    PlayAudio(audio[1],0.5f);
                    mouseticks++;
                    canShowPanelsCMD = !canShowPanelsCMD;

                    if (canShowPanelsCMD)
                    {
                        state_playerIsBuildingTile = 1;
                    }
                    else
                    {
                        state_playerIsBuildingTile = 0;
                    }
                }
            }
            else
            {

                btn_build.drawrect.color = btn_build.drawrect.DEFAULT_COLOR;
            }
        }
        private void IsExploreBtnClicked(ref Point mouseposition, ref bool isclicked, ref bool detectsTile)
        {
            if (!isclicked)
            {
                mouseticks = 0;
            }
            //-- opérations bouton exploration
            if (btn_exploration.IsCollide(mouseposition))
            {
                btn_exploration.drawrect.color = Color.Orange;
                detectsTile = false;

                if (isclicked && currentDay>=1)
                {
                    mouseticks++;

                    int rand = Randomizer.GiveRandomInt(0, 100);

                    if(rand>80)
                    {
                        RefugeWrapper._fightmode = FightMode.monsterlvl1;

                    }
                    else if (rand>40)                    
                    {
                    RefugeWrapper._fightmode = FightMode.monsterlvl2;
                    }
                    else
                    {
                        RefugeWrapper._fightmode = FightMode.monsterlvl3;

                    }
                    main.ChangeScene(scene.standardFight);
                }
            }
            else
            {
                btn_exploration.drawrect.color = btn_exploration.drawrect.DEFAULT_COLOR;
            }
        }
        private void IsNextTurnBtnClicked(ref Point mouseposition, ref bool isclicked, ref bool detectsTile)
        {
            if(!isclicked)
            {
                mouseticks = 0;
            }
            //-- opérations bouton exploration
            if (btn_Next_Turn.IsCollide(mouseposition))
            {
                btn_Next_Turn.drawrect.color = Color.Orange;
                detectsTile = false;

                if(isclicked && mouseticks==0)
                {
                    mouseticks++;
                    StartANewDay = true;
                    PlayAudio(audio[0],0.5f);
                }
            }
            else
            {
                btn_Next_Turn.drawrect.color = btn_Next_Turn.drawrect.DEFAULT_COLOR;
            }
        }
        private void IsQuitBtnClicked(ref Point mouseposition, ref bool isclicked, ref bool detectsTile)
        {
            if (!isclicked)
            {
                mouseticks = 0;
            }
            //-- opérations bouton exploration
            if (btn_Exit.IsCollide(mouseposition))
            {
                btn_Exit.drawrect.color = Color.Orange;
                detectsTile = false;

                if (isclicked)
                {
                    mouseticks++;
                    main.ChangeScene(scene.start);
                }
            }
            else
            {
                btn_Exit.drawrect.color = btn_Exit.drawrect.DEFAULT_COLOR;
            }
        }


        private void IsMusicBtnClicked(ref Point mouseposition, ref bool isclicked, ref bool detectsTile)
        {
            if (!isclicked)
            {
                mouseticks = 0;
            }
            //-- opérations bouton exploration
            if (btn_music.IsCollide(mouseposition))
            {
                btn_music.drawrect.color = Color.Orange;
                detectsTile = false;

                if (isclicked && mouseticks==0)
                {
                    mouseticks++;
                    PunchMusic();
                }
            }
            else
            {
                btn_music.drawrect.color = btn_music.drawrect.DEFAULT_COLOR;
            }
        }
        #endregion </Main buttons UX>

        #region <UX Behaviours>
        private void ReadCameraInputs(ref KeyboardState kbs, ref MouseState mouseState)
        {
            int speed = 5;
            if (mouseState.LeftButton == ButtonState.Pressed && mouseticks == 0)
            {
                frame_cursor = cursors[1];
                canStampLayer = false;
                Point delta = oldmouseposition - mouseState.Position;
                mover = new Vector2(mover.X + (int)delta.X, mover.Y + (int)delta.Y);
                mover *= speed;
                cursorPos = maincamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();
                UIcursorPos = uicamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();
                return;
            }
            else
            {
                frame_cursor = cursors[0];
                cursorPos = maincamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();
                UIcursorPos = uicamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();
            }
            if (kbs.IsKeyDown(Keys.Left))
            {
                mover += new Vector2(-speed, 0);
            }
            if (kbs.IsKeyDown(Keys.Right))
            {
                mover += new Vector2(speed, 0);
            }
            if (kbs.IsKeyDown(Keys.Up))
            {
                mover += new Vector2(0, -speed);
            }
            if (kbs.IsKeyDown(Keys.Down))
            {
                mover += new Vector2(0, speed);
            }
        }   

        private void PlayAudioBuilding()
        {
            PlayAudio(audio[5], 0.5f);
            UpdateStatsBuildings();
            UpdatePopulationCounter();
        }

        Action playBuildingSound; 
        private void ReadTiles(ref Point mouseposition, ref bool isclicked)
        {
            if (bg_btn_main_commands.Contains(mouseposition)) return;

            if (map == null) return;


            if (!isclicked)
            {
                if (mouseticks != 0) mouseticks = 0;
            }

            for (int i =map.Length-1  ; i>= (gridW = 5); i--)
            {

                if (map[i].style == build_style.mergedTile)
                {
                    map[i].drawrect.DEFAULT_COLOR = Color.White * 0.0f;
                }
                else if (map[i].state != buildingState.ishidden)
                {
                    
                    map[i].drawrect.DEFAULT_COLOR = Color.White * 1.0f;
                }

                switch (canDoWork)
                {
                    case false:
                        if (map[i].drawrect.textureID == 0) continue;
                        if (map[i].drawrect.frame.Y == 0 && map[i].drawrect.textureID == 3) continue;
                    break;
                    case true:
                        int currentiteration = i;
                        Color selectionColor = Color.DarkOrange;
                        switch(doWork)
                        {
                            case ActionTile.buildDortoir:

                                if (CanBuildSize2_Tile(ref currentiteration)
                                    && map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;
                                    map[i+1].drawrect.color = selectionColor;


                                    if (isclicked && mouseticks == 0)
                                    {

                                        mouseticks++;
                                        canDoWork = false;

                                        if (BUILD_Dortoir(currentiteration))
                                        {
                                            playBuildingSound();
                                            UpdateBankAcount(doWork);

                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }
                                break;
                            case ActionTile.buildFastfood:
                                if (CanBuildSize2_Tile(ref currentiteration)
                                   && map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;
                                    map[i + 1].drawrect.color = selectionColor;

                                    if (isclicked && mouseticks == 0)
                                    {
                                        mouseticks++;
                                        canDoWork = false;
                                        if (BUILD_FastFood(currentiteration))
                                        {
                                            playBuildingSound();
                                            UpdateBankAcount(doWork);
                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }
                                break;
                            case ActionTile.buildfiltreeau:
                                if (CanBuildSize1_Tile(ref currentiteration)
                                 && map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;

                                    if (isclicked && mouseticks == 0)
                                    {
                                        mouseticks++;
                                        canDoWork = false;
                                        if (BUILD_FiltreEau(currentiteration))
                                        {
                                            playBuildingSound();
                                            UpdateBankAcount(doWork);
                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }
                                break;
                            case ActionTile.buildminecharbon:
                                if (CanBuildSize2_Tile(ref currentiteration)
                                   && map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;
                                    map[i + 1].drawrect.color = selectionColor;

                                    if (isclicked && mouseticks == 0)
                                    {
                                        mouseticks++;
                                        canDoWork = false;
                                        if (BUILD_MineCharbon(currentiteration))
                                        {
                                            playBuildingSound();
                                            UpdateBankAcount(doWork);
                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }
                                break;
                            case ActionTile.buildstockage:
                                if (CanBuildSize3_Tile(ref currentiteration)
                                   && map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;
                                    map[i + 1].drawrect.color = selectionColor;

                                    if (isclicked && mouseticks == 0)
                                    {
                                        mouseticks++;
                                        canDoWork = false;
                                        if (BUILD_Stockage(currentiteration))
                                        {
                                            playBuildingSound();
                                            UpdateBankAcount(doWork);
                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }
                                break; 
                            case ActionTile.buildmagie:
                                if (CanBuildSize2_Tile(ref currentiteration)
                                   && map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;
                                    map[i + 1].drawrect.color = selectionColor;

                                    if (isclicked && mouseticks == 0)
                                    {
                                        mouseticks++;
                                        canDoWork = false;
                                        if (BUILD_LaboMagie(currentiteration))
                                        {
                                            playBuildingSound();
                                            UpdateBankAcount(doWork);
                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }
                                break;
                            case ActionTile.buildforge:
                                if (CanBuildSize2_Tile(ref currentiteration)
                                   && map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;
                                    map[i + 1].drawrect.color = selectionColor;

                                    if (isclicked && mouseticks == 0)
                                    {
                                        mouseticks++;
                                        canDoWork = false;
                                        if (BUILD_Forge(currentiteration))
                                        {
                                            playBuildingSound();
                                            UpdateBankAcount(doWork);
                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }
                                break;
                            case ActionTile.buildbiere:
                                if (CanBuildSize1_Tile(ref currentiteration)
                                 && map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;

                                    if (isclicked && mouseticks == 0)
                                    {
                                        mouseticks++;
                                        canDoWork = false;
                                        if (BUILD_Biere(currentiteration))
                                        {
                                            playBuildingSound();
                                            UpdateBankAcount(doWork);
                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }
                                break;
                            case ActionTile.buildstairs:
                                if (CanBuildStairs_Tile(ref currentiteration)
                                  && map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;

                                    if (isclicked && mouseticks == 0)
                                    {
                                        mouseticks++;
                                        canDoWork = false;
                                        if (BUILD_Stairs(currentiteration))
                                        {
                                            playBuildingSound();
                                            UpdateBankAcount(doWork);
                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }
                                break;

                            case ActionTile.kill_tile:

                                if (map[i].IsCollide(mouseposition))
                                {
                                    map[i].drawrect.color = selectionColor;

                                    if (isclicked && mouseticks == 0)
                                    {
                                        mouseticks++; 
                                        canDoWork = false;
                                        if (KillBuilding(currentiteration))
                                        {
                                            PlayAudio(audio[8], 0.5f);
                                            UpdateBankAcount(doWork);
                                        }
                                    }
                                }
                                else
                                {
                                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                                }

                               
                                break;
                        }
                        break;
                }
            

                if (map[i].IsCollide(mouseposition)
                && map[i].state != buildingState.nonAvailable)
                {
                  //  map[i].drawrect.color = Color.Orange;
                }
                else
                {
                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                }
            }
        }
        private void ReadBuilderPanel_cmd_btn(ref Point mouseposition, ref bool isclicked, ref bool detectsTile)
        {
            if (!canShowPanelsCMD && state_playerIsBuildingTile == 0) return;

            if (!isclicked) { mouseticks = 0; }

            for (int i = 0; i < build_btn_panel.Length; i++)
            {
                if (build_btn_panel[i] == null) continue;
                if (build_btn_panel[i].state==buildingState.nonAvailable) continue;

               
                if (build_btn_panel[i].IsCollide(mouseposition))
                {
                    build_btn_panel[i].drawrect.color = Color.Orange;
                    detectsTile = false;
                    buildingName = RefugeMainRules.GetName(i);
                    buildingPrice = "" + RefugeMainRules.GetPrice(i) + " V ";

                    if (isclicked && mouseticks == 0)
                    {
                        mouseticks++;
                        canShowPanelsCMD = !canShowPanelsCMD;
                        state_playerIsBuildingTile = 0;
                        doWork = ActionTile.empty;

                        if (IsEnoughMoneyToBuild(i))
                        {
                            switch (i)
                            {
                                case 0: doWork = ActionTile.buildDortoir; break;
                                case 1: doWork = ActionTile.buildFastfood; break;
                                case 2: doWork = ActionTile.buildfiltreeau; break;
                                case 3: doWork = ActionTile.buildminecharbon; break;
                                case 4: doWork = ActionTile.buildstockage; break;
                                case 5: doWork = ActionTile.buildmagie; break;
                                case 6: doWork = ActionTile.buildforge; break;
                                case 7: doWork = ActionTile.buildbiere; break;
                                case 8: doWork = ActionTile.buildstairs; break;
                                case 9: doWork = ActionTile.kill_tile; break;
                            }
                        }


                        if (doWork != ActionTile.empty)
                        {
                            canDoWork = true;
                        }
                    }
                }
                else
                {

                    build_btn_panel[i].drawrect.color = build_btn_panel[i].drawrect.DEFAULT_COLOR;
                }
            }


        }

        int dropTicks = 0;

        private void ReadCurrentTileDragged(ref Point mouseposition, ref bool isclicked)//, ref List<ActorRefuge> _ownerActors)
        {
            if(!isclicked) 
            { 
               
                dropTicks = 0; 
            }

            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].style == build_style.empty
                    || map[i].style == build_style.stairs
                    || map[i].style == build_style.forbidden
                    || map[i].state == buildingState.nonAvailable
                    || map[i].state == buildingState.nomorePlace
                    || map[i].state == buildingState.ishidden)
                {
                    continue;
                }

                if (map[i].IsCollide(mouseposition))
                {
                    map[i].drawrect.color = Color.Red;


                    if (!isclicked && map[i].CanAddActor(ref stampActorRefuge) && dropTicks==0)
                    {
                        //-- ajouter l'actor --
                        dropTicks++;
                        map[i].CanAddActor(ref stampActorRefuge, true);
                        deleteDraggedActor = true;

                        map[map_iteraton_dragged].actors.RemoveAll(x => x.state == actorState.ishidden);
                        stampActorRefuge = null;
                    }
                }
                else
                {
                    map[i].drawrect.color = map[i].drawrect.DEFAULT_COLOR;
                }


            }
        }
        #endregion </UX Behaviours>
      
        bool diseableDrop = false;
        bool coliderDetectionFocus = true;
        int2 actorCollider_for_dragandrop_data = new int2(0,0);

        bool ironMineIsOpen = false;
        bool mithrilMineIsOpen = false;
        bool AdamMineIsOpen = false;

        #region <Drag and drop actors>
        private void DragActor(ref Point mouseposition, ref bool isclicked,ref List<ActorRefuge> actors, int map_iteration = -100)
        {
            if(!isclicked)
            {
               if(dragdetected)
                {
                    //-- la copie est terminée
                   // maincamera.RestoreZoom();
                    cursorPos = maincamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();
                    UIcursorPos = maincamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();
                    dragdetected = false;
                    coliderDetectionFocus = true;
                    if(deleteDraggedActor)
                    {
                        PlayAudio(audio[4], 0.5f);
                        actors.RemoveAll(x=>x.state==actorState.ishidden);
                        deleteDraggedActor = false;
                    }
                    else
                    {
                        actors.ForEach(x => x.state=actorState.isopen);

                    }
                }
                    mouseticks = 0;
            }

            if(dragdetected) { return; }

            for (int i = 0; i < actors.Count; i++)
            {
               
                if (actors[i].isCollide(ref mouseposition))
                {
                    //-- oblige à ne sélectionner qu'un seul actor
                    actors[i].color = Color.Red;
                    map_iteraton_dragged = map_iteration;

                    if (isclicked && mouseticks == 0 && coliderDetectionFocus)
                    {
                        PlayAudio(audio[3], 0.5f);

                        coliderDetectionFocus = false;
                        //-- peux recopier l'actor
                        dragdetected = true;
                        mouseticks = 1;
                        actors[i].state = actorState.ishidden;
                        ActorRefuge temp = actors[i];
                        stampActorRefuge = new ActorRefuge(
                            new Rectangle(0, 0, 20, 30), temp.frame);
                    }
                    
                }
                else
                {

                actors[i].color = Color.White;
                }
                
            }
          
        }    
        private void DragActorInsideBuildings(ref Point mousePosition, ref bool isclicked)
        {
            diseableDrop = false;
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].IsCollide(mousePosition))
                {
                    if (map[i].style == build_style.empty
                      || map[i].style == build_style.stairs
                      || map[i].style == build_style.forbidden
                     || map[i].state == buildingState.nonAvailable
                     || map[i].state == buildingState.nomorePlace
                     || map[i].state == buildingState.ishidden)
                    {
                        diseableDrop = true;
                        continue;
                    }

                   
                }
              
            }


            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].style == build_style.empty
                    || map[i].style == build_style.stairs
                    || map[i].style == build_style.forbidden
                    || map[i].state == buildingState.nonAvailable
                    || map[i].state == buildingState.ishidden)
                {
                    continue;
                }

                if (map[i].actors.Count > 0 && !diseableDrop)
                {
                    DragActor(ref mousePosition, ref isclicked, ref map[i].actors, i);
                }
            }
        }
        #endregion </Drag and drop actors>

        float chronoForceNextDay = 0;
        float chronoBankNewMoney = 0;
        public override void Update()
        {

            if(displayNewBankAccount)
            {
                chronoBankNewMoney += 0.015f;
                if(chronoBankNewMoney>2.0f)
                {
                    chronoBankNewMoney = 0;
                    displayNewBankAccount = false;
                }
            }

            if (map == null) return;

            //-- forcer à garder la même couleur pour les tuiles d'entrée et accueil
            map[0].drawrect.color = Color.White;
            map[13].drawrect.color = Color.White;
            //-end-

          

            //-- constantes
            if (maincamera == null) { return; }
            mover = Vector2.Zero;
            KeyboardState kbs = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            mouseposition = mouseState.Position;

            //-- mettre à jour la position virtuelle du curseur
            ReadCameraInputs(ref kbs, ref mouseState);
            maincamera.MoveCamera(mover, true);

            var isclicked = mouseState.LeftButton == ButtonState.Pressed;
            canDetectTiles = true;
            //-end- constantes

            if (startdayfromwrapper)
            {
                StartDayFromWrapper();
            }


            if (StartANewDay)
            {
                PlayNextDay();
                return;
            }

            UpdatePopulationCounter();
            UpdatePriceVisual();

            //-- ne sais pas pourquoi celà fonctionne UNIQUEMENT de cette manière :/
            dragcursor = new Point(cursorPos.X, cursorPos.Y);

            if (dragdetected && stampActorRefuge != null)
            {
                stampActorRefuge.position = new Rectangle(cursorPos.X, cursorPos.Y, 10, 15);
            }
            //-end- ne sais pas pourquoi celà fonctionne UNIQUEMENT de cette manière :/
        
            if (dragdetected)
            {
               // cursorPos = maincamera.ScreenToWorld(mouseposition.ToVector2()).ToPoint();
                //-- déposer un actor
                ReadCurrentTileDragged(ref cursorPos, ref isclicked);
            }

            UpdateActors();

            //-- commandes page de jeu
            IsExploreBtnClicked(ref UIcursorPos, ref isclicked, ref canDetectTiles);
            IsNextTurnBtnClicked(ref UIcursorPos, ref isclicked, ref canDetectTiles);
            IsQuitBtnClicked(ref UIcursorPos, ref isclicked, ref canDetectTiles);
            IsBuildBtnClicked(ref UIcursorPos, ref isclicked, ref canDetectTiles);
            IsMusicBtnClicked(ref UIcursorPos, ref isclicked, ref canDetectTiles);

            //-- commandes si bouton principal cliqué
            ReadBuilderPanel_cmd_btn(ref UIcursorPos, ref isclicked, ref canDetectTiles);


            //-- attrapper un actor
            if (map != null)
            {
                DragActorInsideBuildings(ref cursorPos, ref isclicked);
            }


            if (bg_btn_main_commands.Contains(UIcursorPos)) { canDetectTiles = false; }

            if (canShowPanelsCMD && alpha_maincamera > 0.02f)
            {
                alpha_maincamera -= 0.015f;
                if (alpha_maincamera <= 0.2f) { alpha_maincamera = 0.2f; }

            }
            else if (alpha_maincamera != 1.0f)
            {
                alpha_maincamera += 0.015f;
                if (alpha_maincamera >= 1) { alpha_maincamera = 1.0f; }

            }

            if (canDetectTiles 
                && !canShowPanelsCMD
                && state_playerIsBuildingTile==0)
            {
                ReadTiles(ref cursorPos, ref isclicked);

            }



            chronoForceNextDay += 0.0015f;

            if(chronoForceNextDay>=8)
            {
                chronoForceNextDay = 0;
                StartANewDay = true;
            }


            old_kbs = kbs;

            oldmouseposition = mouseState.Position;

            base.Update();
        }

        #region <StackFlow Behaviours>

        string buildingPrice = string.Empty;
        string buildingName = string.Empty;

      
        private void UpdateBankAcount(ActionTile _dowork)
        {
            int value = 0;
            switch (_dowork)
            {
                case ActionTile.buildDortoir: value = 0; break;
                case ActionTile.buildFastfood: value = 1; break;
                case ActionTile.buildfiltreeau: value = 2; break;
                case ActionTile.buildminecharbon: value = 3; break;
                case ActionTile.buildstockage: value = 4; break;
                case ActionTile.buildmagie: value = 5; break;
                case ActionTile.buildforge: value = 6; break;
                case ActionTile.buildbiere: value = 7; break;
                case ActionTile.buildstairs: value = 8; break;
                case ActionTile.kill_tile: value = 9; break;
            }

            int price = RefugeMainRules.GetPrice(value);
            bank -= price;
            alphaPrice_remover = 1.0f;
            hitBank_remover = " -" + price;
        }
        private bool IsEnoughMoneyToBuild(int typeOfBuilding)
        {
            if (bank >= RefugeMainRules.GetPrice(typeOfBuilding))
            {
                Debug.WriteLine("tu as le pognon pour batir limmeuble");
                return true;
            }
            else
            {
                Debug.WriteLine("désolé");
                return false;
            }
        }

        private void UpdatePriceVisual()
        {
            if (alphaPrice_remover >= 0)
            {
                alphaPrice_remover -= 0.0075f;
                if (alphaPrice_remover <= 0)
                {
                    alphaPrice_remover = 0;
                    hitBank_remover = string.Empty;
                }
            }
        }

        int actorCounter = 0;
        
        private void UpdateActors()
        {
            actorCounter = 0;
            if (map != null)
            {
                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i].actors.Count > 0)
                    {
                        foreach (var item in map[i].actors)
                        {
                            actorCounter++;
                            item.UpdateMove();
                        }
                    }
                }
            }

            //-- mettre à jour le nombre d'habitants
            if(totalpopulation!=actorCounter) { totalpopulation = actorCounter; }
        }

        int nextdaystate = 0;
        float chrononextday = 0.0f;
        bool StartANewDay = false;
        bool startdayfromwrapper = false;
        private void StartDayFromWrapper()
        {
            switch (nextdaystate)
            {
                case 0:
                    alpha_maincamera -= 0.015f;
                    alphadays -= 0.015f;
                    if (alpha_maincamera <= 0)
                    {
                        RefugeWrapper.SaveDataInWrapper(ref map);
                        nextdaystate = 1;
                        alphadays = 0;
                        alpha_maincamera = 0;


                        while (true)
                        {
                            daysVisual[0].color = Color.White * 0.2f;
                            daysVisual[1].color = Color.White * 0.2f;
                            daysVisual[2].color = Color.White * 0.2f;
                            daysVisual[3].color = Color.White * 0.2f;
                            daysVisual[4].color = Color.White * 0.2f;
                            daysVisual[5].color = Color.White * 0.2f;
                            daysVisual[6].color = Color.White * 0.2f;
                            break;
                        }
                    }



                    break;
                case 1:
                    chrononextday += 0.04f;
                    if (chrononextday >= 1)
                    {
                        chrononextday = 0;
                        nextdaystate = 2;
                        GetWaterCount();
                        GetFoodCount();
                        GetEnergyCount();
                        PlayAudio(audio[2], 0.5f);
                        KillActors();
                        FeedActors();

                        //-- transfert données au wrapper
                        maxpopulation = totalbuilddortoir * 4;
                        int temp = currentDay;
                        temp++;
                        if (temp >= 7)
                        {
                            PlayAudio(audio[1], 0.5f);
                            temp = 0;
                        }
                        currentDay = temp;

                        daysVisual[currentDay].color = Color.White;
                        //-- mettre à jour les boutons de contruction --
                        UpdateBuilderPanelButtons();
                    }
                    break;
                case 2:
                    alpha_maincamera += 0.15f;
                    alphadays += 0.15f;
                    if (alpha_maincamera >= 1.0f)
                    {
                        nextdaystate = 0;
                        alphadays = 1.0f;
                        alpha_maincamera = 1.0f;
                        startdayfromwrapper = false;
                    }
                    break;
            }
        }


        private void PlayNextDay()
        {
            switch(nextdaystate)
            {
                case 0:
                    alpha_maincamera -= 0.015f;
                    alphadays -= 0.015f;
                    if(alpha_maincamera <= 0) 
                    {
                        RefugeWrapper.SaveDataInWrapper(ref map);
                        nextdaystate = 1;
                        alphadays = 0;
                        alpha_maincamera = 0;


                        while(true)
                        {
                            daysVisual[0].color = Color.White * 0.2f;
                            daysVisual[1].color = Color.White * 0.2f;
                            daysVisual[2].color = Color.White * 0.2f;
                            daysVisual[3].color = Color.White * 0.2f;
                            daysVisual[4].color = Color.White * 0.2f;
                            daysVisual[5].color = Color.White * 0.2f;
                            daysVisual[6].color = Color.White * 0.2f;  
                            break;
                        }
                    }



                    break;
                case 1:
                    chrononextday += 0.04f;
                    if(chrononextday>=1)
                    {
                        chrononextday = 0;
                        nextdaystate = 2;
                        GetWaterCount();
                        GetFoodCount();
                        GetEnergyCount();
                        PlayAudio(audio[2], 0.5f);
                        KillActors();
                        FeedActors();

                        //-- transfert données au wrapper

                        
                        data.totalbuilddortoir = totalbuilddortoir;
                        data.totalbuildfasfood = totalbuildfasfood;
                        data.totalbuildminecharbon = totalbuildminecharbon;
                        data.totalbuildmagic = totalbuildmagic;
                        data.totalbuildforge = totalbuildforge;
                        data.totalbuildfiltreeau= totalbuildfiltreeau;
                        data.totalbuildbiere = totalbuildbiere;
                        data.totalbuildstockage = totalbuildstockage;
                        data.totaleau=totaleau;
                        data.totalpain= totalpain;
                        data.totalpopulation= totalpopulation;
                        data.totalenergie= totalenergie;
                        data.bank=bank;
                        data.currentDay= currentDay;
                        data.daysVisual=daysVisual;
                        RefugeWrapper.SaveDataRefuge(ref data);

                        maxpopulation = totalbuilddortoir * 4;

                        int temp = currentDay;
                        temp++;
                        if(temp>=7)
                        {
                            PlayAudio(audio[1], 0.5f);

                            if(data.totalpopulation<maxpopulation)
                            {
                                Add_Friend_ActorRefuge(2);
                            }
                            
                            temp = 0;
                        }
                        currentDay = temp;

                        daysVisual[currentDay].color = Color.White;

                        //-- mettre à jour les boutons de contruction --
                        UpdateBuilderPanelButtons();
                    }
                    break;
                case 2:
                    alpha_maincamera += 0.15f;
                    alphadays += 0.15f;
                    if (alpha_maincamera >=1.0f)
                    {
                        nextdaystate = 0;
                        alphadays = 1.0f;
                        alpha_maincamera = 1.0f;
                        StartANewDay = false;
                    }
                    break;
            }
        }

        private void UpdateBuilderPanelButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                if(RefugeMainRules.FreeBuildings(i,maxpopulation))
                {
                    build_btn_panel[i].drawrect.DEFAULT_COLOR = Color.White;
                }
                else
                {
                    build_btn_panel[i].drawrect.color = Color.Gray * 0.5f;
                    build_btn_panel[i].state = buildingState.nonAvailable;
                }
            }
        }

        private void GetWaterCount()
        {
            int maxEau = totalbuildfiltreeau * 10;

            int waterCount = 0;
            int multiplicateur = 6;

            if (map != null)
            {
                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i].style == build_style.filtreEau)
                    {
                        if (map[i].actors.Count > 0)
                        {

                            map[i].actors.ForEach(x => x.state = actorState.iswater);
                            waterCount += map[i].actors.Count* multiplicateur;
                        }
                    }
                }
            }

            if(totaleau < maxEau)
            {
                totaleau += waterCount;
            }
            else
            {
                totaleau = 0;
            }
        }

        private void GetFoodCount()
        {
            int maxFood = totalbuildfasfood * 10;

            int counter = 0;
            int multiplicateur = 6;

            if (map != null)
            {
                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i].style == build_style.fastfood)
                    {
                        if (map[i].actors.Count > 0)
                        {

                            map[i].actors.ForEach(x => x.state = actorState.iswater);
                            counter += map[i].actors.Count *multiplicateur;
                        }
                    }
                }
            }

            if (totalpain < maxFood)
            {
                totalpain += counter;
            }
            else
            {
                totalpain = 0;
            }

        }

        private void GetEnergyCount()
        {
            int maxEnergy = totalbuildminecharbon * 10;

            int counter = 0;
            int multiplicateur = 2;

            if (map != null)
            {
                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i].style == build_style.minedecharbon)
                    {
                        if (map[i].actors.Count > 0)
                        {

                            map[i].actors.ForEach(x => x.state = actorState.iswater);
                            counter += map[i].actors.Count * multiplicateur;
                        }
                    }
                }
            }

            if (totalenergie < maxEnergy)
            {
                totalenergie += counter;
            }
            else
            {
                totalenergie = 0;
            }

        }
        int maxpopulation;
        private void UpdatePopulationCounter()
        {
            maxpopulation = totalbuilddortoir * 4;



            actorCounter = 0;
            if (map != null)
            {
                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i].actors.Count > 0)
                    {
                        foreach(ActorRefuge actor in map[i].actors)
                        {
                            if(actor.state!=actorState.ishidden)
                            {
                               
                                if(actorCounter>maxpopulation)
                                {
                                    actor.state = actorState.ishidden;
                                }
                                else
                                {

                                }
                                
                            }
                        }
                    }
                }
            }

         

            //-- mettre à jour le nombre d'habitants
            totalpopulation = actorCounter;

        }

        private void FeedActors()
        {
            totalpain -= (totalpopulation);
            totaleau -= (totalpopulation);

            int totalbuildings = totalbuilddortoir + totalbuildminecharbon+
            totalbuildbiere*4+totalbuildfasfood+ totalbuildmagic*2+
            totalbuildforge+ totalbuildfiltreeau+ totalbuildstockage*2;

            totalenergie -= totalbuildings;
        }


        private void KillActors()
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].actors.Count > 0)
                {
                    map[i].actors.RemoveAll(x=>x.state == actorState.ishidden);
                    map[i].actors.RemoveAll(x=>x.state == actorState.toremove);
                }
            }
        }

        int totalbuilddortoir = 1, totalbuildminecharbon,
            totalbuildbiere, totalbuildfasfood, totalbuildmagic,
            totalbuildforge, totalbuildfiltreeau, totalbuildstockage;


        private void UpdateStatsBuildings()
        {
            int totaldortoir_counter=0, totalminecharbon_counter = 0,
           totalbiere_counter=0, totalfasfood_counter=0, totalmagic_counter=0,
           totalforge_counter=0, totalfiltreeau_counter=0, totalstockage_counter=0;


            if (map != null)
            {
                for (int i = 0; i < map.Length; i++)
                {
                    switch(map[i].style)
                    {
                        case build_style.dortoir: totaldortoir_counter++; break;
                        case build_style.fastfood: totalfasfood_counter++; break;
                        case build_style.minefer: totalminecharbon_counter++; break;
                        case build_style.labomagie: totalmagic_counter++; break;
                        case build_style.forge: totalforge_counter++; break;
                        case build_style.filtreEau: totalfiltreeau_counter++; break;
                        case build_style.biere: totalbiere_counter++; break;
                        case build_style.salledestockage: totalstockage_counter++; break;
                    }
                }
            }


            totalbuilddortoir = totaldortoir_counter;
            totalbuildfasfood = totalfasfood_counter;
            totalbuildminecharbon = totalminecharbon_counter;
            totalbuildmagic = totalmagic_counter;
            totalbuildforge = totalforge_counter;
            totalbuildfiltreeau = totalfiltreeau_counter;
            totalbuildbiere  = totalbiere_counter;
            totalbuildstockage = totalstockage_counter;
        }

        #endregion  </StackFlow Behaviours>

        public override void Draw(ref SpriteBatch _sp)
        {
            //-- dessiner les tuiles de la map
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].style == build_style.mergedTile)
                {
                    map[i].drawrect.color = Color.White * 0.0f;
                }
                int tex_ID = map[i].drawrect.textureID;
                _sp.Draw(textures[tex_ID],
                    map[i].drawrect.position,
                    map[i].drawrect.frame,
                    map[i].drawrect.color * alpha_maincamera
                    , 0.0f, Vector2.Zero, SpriteEffects.None, 0.9f);


                if (map[i].actors.Count > 0)
                {

                    for (int j = 0; j < map[i].actors.Count; j++)
                    {
                        if (map[i].actors[j].state != actorState.ishidden)
                        _sp.Draw(textures[9], map[i].actors[j].position,
                            map[i].actors[j].frame,
                            map[i].actors[j].color * alpha_maincamera,
                            0.0f, map[i].actors[j].origin,
                            map[i].actors[j].spriteEffect,
                            0.1f);
                    }
                }
            }
            
            Rectangle posSoldat = new Rectangle(40, 25, 10, 15);
            Rectangle frameSoldat = new Rectangle(0, 0, 20, 30);

            if(actor_friends.Count > 0)
            {
                float actorfriends_layerdepth = 0.9f;
                foreach(ActorRefuge actor in  actor_friends)
                {
                    if(actor.state != actorState.ishidden)
                    _sp.Draw(textures[9], actor.position, actor.frame, actor.color,
                        0,actor.origin, actor.spriteEffect, actorfriends_layerdepth);
                }
            }

            if(dragdetected && stampActorRefuge!=null)
            {
                _sp.Draw(textures[9], stampActorRefuge.position, stampActorRefuge.frame, stampActorRefuge.color,
                       0, Vector2.Zero, stampActorRefuge.spriteEffect, 0.1f);
            }

            base.Draw(ref _sp);
        }

        float alphadays = 0.0f;
        public override void Draw_UI(ref SpriteBatch _spUI)
        {
            if(dragdetected) { return; }
            if (textures == null) return;

            //-- boutons de commande
            _spUI.Draw(statusTex, bg_btn_main_commands, new Rectangle(0, 0, 16, 16), Color.White * 0.5f
                 , 0, Vector2.Zero, SpriteEffects.None, 0.4f); ;// * 0.5f);
            _spUI.Draw(textures[4], btn_build.drawrect.position, btn_build.drawrect.frame, btn_build.drawrect.color, 0, Vector2.Zero, SpriteEffects.None, 0.2f);
            _spUI.Draw(textures[4], btn_exploration.drawrect.position, btn_exploration.drawrect.frame, btn_exploration.drawrect.color, 0, Vector2.Zero, SpriteEffects.None, 0.2f);
            _spUI.Draw(textures[4], btn_Exit.drawrect.position, btn_Exit.drawrect.frame, btn_Exit.drawrect.color, 0, Vector2.Zero, SpriteEffects.None, 0.2f);


            _spUI.Draw(statusTex, bg_btn_dayNightPanel, new Rectangle(0, 0, 16, 16), Color.White * 0.5f
                , 0, Vector2.Zero, SpriteEffects.None, 0.6f); ;

            _spUI.Draw(textures[4], btn_Next_Turn.drawrect.position,
                btn_Next_Turn.drawrect.frame, btn_Next_Turn.drawrect.color, 
                0, Vector2.Zero, SpriteEffects.None, 0.2f);

            if (daysVisual != null)
            {
                for (int i = 0; i < daysVisual.Length; i++)
                {
                    _spUI.Draw(textures[10], daysVisual[i].position, 
                        daysVisual[i].frame, daysVisual[i].color*alphadays
               , 0, Vector2.Zero, SpriteEffects.None, 0.4f); ;
                }
            }
            //-- icones d'information (visuel)
            _spUI.Draw(statusTex, bg_infos, new Rectangle(0, 0, 16, 16), Color.White * 0.5f
                , 0, Vector2.Zero, SpriteEffects.None, 0.9f);

            float iconLayerdepth = 0.3f;
            _spUI.Draw(textures[4], pop_dr.position, pop_dr.frame, Color.White,
                0, Vector2.Zero, SpriteEffects.None, iconLayerdepth);
            _spUI.Draw(textures[4], food_dr.position, food_dr.frame,
                Color.White, 0, Vector2.Zero, SpriteEffects.None, iconLayerdepth);
            _spUI.Draw(textures[4], water_dr.position, water_dr.frame,
                Color.White, 0, Vector2.Zero, SpriteEffects.None, iconLayerdepth);
            _spUI.Draw(textures[4], energy_rect, energy_dr.frame,
                Color.White, 0, Vector2.Zero, SpriteEffects.None, iconLayerdepth);


            //-- informations
            float stringLayerdepth = 0.5f;
            _spUI.DrawString(cutsceneFont, totalpopulation.ToString(), infospositions[0], 
                Color.Black,
               0, Vector2.Zero, 1.0f, SpriteEffects.None, stringLayerdepth);
            _spUI.DrawString(cutsceneFont, totalpain.ToString(), infospositions[1], 
                Color.Black,
               0, Vector2.Zero, 1.0f, SpriteEffects.None, stringLayerdepth);
            _spUI.DrawString(cutsceneFont, totaleau.ToString(), infospositions[2], 
                Color.Black,
               0, Vector2.Zero, 1.0f, SpriteEffects.None, stringLayerdepth);
            _spUI.DrawString(cutsceneFont, totalenergie.ToString(), infospositions[3], 
                Color.Black,
               0, Vector2.Zero, 1.0f, SpriteEffects.None, stringLayerdepth);

            _spUI.Draw(textures[4], btn_music.drawrect.position,
                btn_music.drawrect.frame, btn_music.drawrect.color, 
                0, Vector2.Zero, SpriteEffects.None, 0.2f);

            if (displayNewBankAccount)
            {
                _spUI.DrawString(cutsceneFont, newBankAccount,
              ToOffset(new Vector2(30, 2)), Color.OrangeRed * alphaPrice_remover,
                 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);
            }

            if (mouseticks>0) return;
            if (cursor == null) return;
            Rectangle mousecursorPosition = new Rectangle(UIcursorPos.X, UIcursorPos.Y, 20, 20);
            _spUI.Draw(cursor, mousecursorPosition, frame_cursor, Color.White);



            _spUI.DrawString(cutsceneFont, ""+map[0].drawrect.position.X + " : " + map[0].drawrect.position.Y, Vector2.Zero,
              Color.White,
             0, Vector2.Zero, 1.0f, SpriteEffects.None, stringLayerdepth);


            base.Draw_UI(ref _spUI);
        }

        float alpha_maincamera = 1.0f;

        public override void Draw_Friend(ref SpriteBatch _spUI)
        {
            /*
             *  Exclusivement dédié pour la page de construction, d'exploration 
             *  , de fin de tour et de fin de partie
             */

            //-- compte en banque

            _spUI.Draw(statusTex, bankPosition, new Rectangle(0,0,16,16),Color.Black,
                0.0f,Vector2.Zero,SpriteEffects.None,0.9f);


            _spUI.Draw(textures[4], bankIconPosition, new Rectangle(24 * 4, 0, 24, 24),
              Color.White,
                0.0f, Vector2.Zero, SpriteEffects.None, 0.3f);
            _spUI.DrawString(cutsceneFont, " : " + bank.ToString(),
               ToOffset(new Vector2(18, 7)), Color.Yellow * 4.0f,
               0,Vector2.Zero,1.0f,SpriteEffects.None,0.5f);

            _spUI.DrawString(cutsceneFont, hitBank_remover,
            ToOffset(new Vector2(30, 2)), Color.Red * alphaPrice_remover,
               0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.4f);


           
            //--


            if (!canShowPanelsCMD)
            {
                return;
            }


            _spUI.Draw(statusTex, bg_buildPanel,
                new Rectangle(0, 0, 16, 16),
                Color.Salmon
             , 0, Vector2.Zero, SpriteEffects.None, 0.4f);

            for (int i = 0; i < build_btn_panel.Length; i++)
            {
                _spUI.Draw(textures[build_btn_panel[i].drawrect.textureID],
                    build_btn_panel[i].drawrect.position,
                    build_btn_panel[i].drawrect.frame,
                    build_btn_panel[i].drawrect.color,
                    0, Vector2.Zero, SpriteEffects.None, 0.3f);
            }

            _spUI.DrawString(cutsceneFont, buildingName,ToOffset(new Vector2(80, 8)),
                Color.White,
                0,Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);
            _spUI.DrawString(cutsceneFont, buildingPrice , ToOffset(new Vector2(124, 50)),
               Color.Yellow,
               0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.1f);

            base.Draw_Friend(ref _spUI);
        }
    }
}



