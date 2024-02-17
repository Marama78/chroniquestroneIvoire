using DinguEngine;
using DinguEngine.Camera;
using DinguEngine.Shared;
using DinguEngine.UI;
using DinguEngine.UI.TE_Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;
using TheShelter;

namespace _TheShelter.Scene
{
    public class Refuge : ModelScene
    {        

        TE_Button[] map;
        TE_Button[] cmd_buildings;

        Texture2D tex_entree, tex_buildings;
        Texture2D[] textures;
        Texture2D cursor;
        Rectangle frame_cursor;
        Rectangle[] cursors;
        Point cursorPos;
        Point UIcursorPos;
        int gridW = 11;
        int gridH = 9;
        int gridSize;

        int tileW, tileH;

        int frameW, frameH;

        Vector2 mover;
        Point oldmouseposition,mouseposition;
        bool canStampLayer = false;
        KeyboardState old_kbs;

        int mouseticks = 0;

        int offsetY_UIPANEL = 0;// 80;

        TE_Window buildPanel;
        DrawRect[] buildPanel_rect;

        TE_Button[] builderbtns;

        bool focusOnUI = false;
        bool isShowing_BuilderPanel = false;

        TE_Button btn_build, btn_exploration;
        int2 cameraOffset = new int2(-120, -80);
        Texture2D statusTex;

        Rectangle population_rect, food_rect, water_rect, energy_rect;

        Rectangle BuilderPanel_rect;
        public Refuge( MainClass _mainclass) : base( _mainclass)
        {
        }

        public Rectangle ToOffset(Rectangle value)
        {
            return new Rectangle(value.X + cameraOffset.x, value.Y + cameraOffset.y, value.Width, value.Height); ;
        }

        public Vector2 ToOffset(Vector2 value)
        {
            return new Vector2(value.X + cameraOffset.x, value.Y + cameraOffset.y);
        }

        private void SetUIButtons()
        {
            Rectangle temp = ToOffset(new Rectangle(14, 122, 30, 30));
            DrawRect rect = new DrawRect(temp, 16);
            btn_build = new TE_Button(rect,new int2(0,0),16);


            Rectangle tempex = ToOffset(new Rectangle(196, 122, 30, 30));
            DrawRect rectex = new DrawRect(tempex, 16);
            btn_exploration = new TE_Button(rectex, new int2(0, 0), 16);


            population_rect = ToOffset(new Rectangle(125,122,30,30));
            energy_rect = ToOffset(new Rectangle(213,8,18,18));
            food_rect = ToOffset(new Rectangle(213,28,18,18));
            water_rect = ToOffset(new Rectangle(213,48,18,18));
        }

        private void LoadTextures(ref ContentManager _content)
        {
            if (_content == null) return;

            statusTex = _content.Load<Texture2D>("statusBar");


            cursor = _content.Load<Texture2D>("system\\cursor");
            frame_cursor = new Rectangle(0, 0, 8, 8);

            cursors = new Rectangle[5]
            {
                new Rectangle(0, 0, 16, 16),
                new Rectangle(16, 0, 16, 16),
                new Rectangle(0, 16, 16, 16),
                new Rectangle(16, 16, 16, 16),
                new Rectangle(0, 32, 16, 16),
            };

            tex_entree = _content.Load<Texture2D>("Tilesets\\entree");
            tex_buildings = _content.Load<Texture2D>("Tilesets\\buildings");


            textures = new Texture2D[6];
            textures[0] = _content.Load<Texture2D>("Tilesets\\entree");
            textures[1] = _content.Load<Texture2D>("Tilesets\\accueil");
            textures[2] = _content.Load<Texture2D>("Tilesets\\stairs");
            textures[3] = _content.Load<Texture2D>("Tilesets\\buildings");
            textures[4] = _content.Load<Texture2D>("Tilesets\\UIwindow");
            textures[5] = _content.Load<Texture2D>("Tilesets\\builder_icon");
        }

        private void SetInterface()
        {
            buildPanel = new TE_Window();
            buildPanel_rect = buildPanel.DrawWindow(new int2(10,6), new int2(8,8), 12, 12);

            SetBuilderButtons();
        }

        private void SetBuilderButtons()
        {
            builderbtns = new TE_Button[8];



            int line = -1;
            int column = 0;
            int framecounterH = 0;
            int framecounterV = 0;
            for (int i = 0; i < 8; i++)
            {
                if (i % 4 == 0)
                {
                    column = 0;
                    line++;
                }
                else
                {
                    column++;
                }
                int outputsize = 20;
                int posX = (12 + 8) + column * (outputsize + 4);
                int posY = 18 + line * (outputsize + 4);
                int framesize = 48;
                Rectangle temp_rect = new Rectangle(posX, posY, outputsize, outputsize);
                DrawRect temp = new DrawRect(temp_rect, framesize);

                builderbtns[i] = new TE_Button(temp, new int2(0, 0), framesize);
           

                if(framecounterH==3)
                {
                    framecounterH = 0;
                    framecounterV++;
                }

                builderbtns[i].drawrect.SetFrame(framecounterH, framecounterV, 48);
                framecounterH++;
            }
        }

        Song[] bgsongs;
        private void GetSongs(ref ContentManager _content)
        {
            bgsongs = new Song[5]
            {
                _content.Load<Song>("Songs\\refuge\\OST 1 - Beyond Infinity (Loopable)"),
                _content.Load<Song>("Songs\\refuge\\OST 2 - Lantern Light Lore (Loopable)"),
                _content.Load<Song>("Songs\\refuge\\OST 3 - Wanderers' Whispers (Loopable)"),
                _content.Load<Song>("Songs\\refuge\\OST 4 - Fireside Legends (Loopable)"),
                _content.Load<Song>("Songs\\refuge\\OST 5 - Moonlit Myths (Loopable)"),
            };
        }

        public override void Load(ref ContentManager _content)
        {
            GetSongs(ref _content);


            int rand = Randomizer.GiveRandomInt(0,bgsongs.Length);

            if (rand >= 5) rand = 4;


            MediaPlayer.Play(bgsongs[rand]);


            maincamera = new TE_Camera();
            uicamera = new TE_Camera();
            friendcamera = new TE_Camera();
            ennemycamera = new TE_Camera();

            maincamera.MoveCamera(mover, true);
            LoadTextures(ref _content);
            SetInterface();
            tileW = TE_Manager.tileW;// 40;// 80;
            tileH = TE_Manager.tileH;// 20;// 40;
            frameW = 80;
            frameH = 40;
            TE_Manager.gridW = gridW;
            TE_Manager.gridH = gridH;
            TE_Manager.tileW = tileW;
            TE_Manager.tileH = tileH;

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
                int posY = line * (tileH)  + offsetY_UIPANEL;
                int rectWidth = tileW;
                
                if (column == 1 || column == 4 || column==9) rectWidth = tileW / 2;
                if(column>9)
                {
                    posX -= (int)(tileW*1.5f);
                }
                else if(column>4)
                {
                    posX -= tileW;

                }
                else if (column>1)
                {
                    posX -= tileW/2;

                }

                DrawRect temp = new DrawRect(posX, posY, rectWidth, tileH);
                temp.color = Color.White;
                temp.alpha = 1.0f;
                temp.gridID = i;
                temp.textureID = 3;

                if (line==0)
                {
                    temp.SetFrame(2, 0, frameW, frameH);

                    map[i] = new TE_Button(temp, new int2(2, 0), frameW, frameH);
                }
                else if(column == 0)
                {
                temp.SetFrame(0, 0, frameW, frameH);

                    map[i] = new TE_Button(temp, new int2(0, 0), frameW, frameH);

                }
                else if (column == gridW-1)
                {
                    temp.SetFrame(2, 0, frameW, frameH);
                    map[i] = new TE_Button(temp, new int2(1, 0), frameW, frameH);

                }
                else
                {



                    if (column == 1 || column == 4 || column == 9)
                    {
                        //-- les escaliers --
                        temp.textureID = 2;
                        temp.SetFrame(0, 0, frameW/2, frameH);
                    map[i] = new TE_Button(temp, new int2(0, 0), frameW/2, frameH);
                    }
                    else
                    {
                        temp.textureID = 3;
                        temp.SetFrame(0, 0, frameW, frameH);

                        map[i] = new TE_Button(temp, new int2(2, 0), frameW, frameH);
                    }


                }

            }

            //-- désassembler les tuiles de l'entréee (9x2)
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    //-- entrée du refuge --
                    map[i].drawrect.position = new Rectangle(0, offsetY_UIPANEL, (int)(tileW*1.5f), tileH*2);
                    map[i].drawrect.textureID = 0;
                    map[i].drawrect.SetFrame(0, 0, 200, frameH*2);

                }
                else
                {
                    map[i].drawrect.position = new Rectangle(0, 0, 0, 0);
                }
            }
                for (int i = gridW; i < (gridW+2); i++)
                {
                        map[i].drawrect.position = new Rectangle(0, 0, 0, 0);
                    
                }


                int rectX = map[13].drawrect.position.X;
                int rectY = map[13].drawrect.position.Y;
            map[13].drawrect.textureID = 1;
            map[13].drawrect.frame = new Rectangle(0, 0, frameW*2, frameH);
            map[13].drawrect.position = new Rectangle(rectX, rectY, tileW*2, tileH);
            map[14].drawrect.position = Rectangle.Empty;
            map[23].drawrect.SetFrame(1, 0, 40);// = Rectangle.Empty;

            map[25].drawrect.textureID = 3;
            map[25].drawrect.SetFrame(0, 2, frameW, frameH);
            map[27].drawrect.textureID = 3;
            map[27].drawrect.SetFrame(1, 1, frameW, frameH);
            map[39].drawrect.textureID = 3;
            map[39].drawrect.SetFrame(1, 2, frameW, frameH);
            map[47].drawrect.textureID = 3;
            map[47].drawrect.SetFrame(1, 1, frameW, frameH);


            SetUIButtons();
            base.Load(ref _content);
        }
        private void ReadCameraMove_by_KB(ref KeyboardState kbs, ref MouseState mouseState)
        {
            int speed = 5;

            if (mouseState.LeftButton == ButtonState.Pressed && mouseticks==0)
            {
                frame_cursor = cursors[1];
                canStampLayer = false;
                Point delta = oldmouseposition - mouseState.Position;
                Debug.WriteLine("" + delta);
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

        private void ReadTiles(ref Point mouseposition, ref bool isclicked)
        {
           if(!isclicked) 
           {
                if(mouseticks!=0) mouseticks = 0; 
           }
            int line = -1;
            int column = 0;
            for (int i = 0; i < map.Length; i++)
            {

                if(i==0)
                {
                    continue; }
                if(i%gridW==0)
                {
                    column = 0;
                    line++;
                    continue;
                }
                else
                {
                    column++;
                }
                
                if(column==gridW-1) { continue; }

                if (map[i].IsCollide(mouseposition))
                {
                    map[i].drawrect.color = Color.Orange;
                }
                else
                {
                    map[i].drawrect.color = Color.White;

                }
            }
        }


        private void ReadUIButtons(ref Point mouseposition, ref bool isclicked)
        {
         
        }

        public override void Update()
        {
            if (maincamera == null) { return; }
            mover = Vector2.Zero;
            KeyboardState kbs = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            mouseposition = mouseState.Position;

            maincamera.HandleInput();
            ReadCameraMove_by_KB(ref kbs, ref mouseState);
            maincamera.MoveCamera(mover, true);

            var isclicked = mouseState.LeftButton == ButtonState.Pressed;
            ReadUIButtons(ref UIcursorPos, ref isclicked);

            ReadTiles(ref cursorPos, ref isclicked);

            for (int i = 0; i<map.Length; i++)
            {

            }

            //------------------
            old_kbs = kbs;

            oldmouseposition = mouseState.Position;

            base.Update();
        }

        public override void Draw(ref SpriteBatch _sp)
        {
            for (int i = 0; i < map.Length; i++)
            {
                int tex_ID = map[i].drawrect.textureID;
                _sp.Draw(textures[tex_ID], map[i].drawrect.position, map[i].drawrect.frame, map[i].drawrect.color
                    , 0.0f, Vector2.Zero, SpriteEffects.None, 0.9f);
            }
            Rectangle posSoldat = new Rectangle(40,25,10,15);
            Rectangle frameSoldat = new Rectangle(0,0,20,30);
           
            
            Rectangle mousecursorPosition = new Rectangle(cursorPos.X, cursorPos.Y, 20, 20);
            _sp.Draw(cursor, mousecursorPosition, frame_cursor, Color.White);



            base.Draw(ref _sp);
        }

        public override void Draw_UI(ref SpriteBatch _spUI)
        {
           
            _spUI.Draw(statusTex, btn_build.drawrect.position, btn_build.drawrect.frame, btn_build.drawrect.color);
            _spUI.Draw(statusTex, btn_exploration.drawrect.position, btn_exploration.drawrect.frame, btn_exploration.drawrect.color);
            
            
            _spUI.Draw(statusTex, energy_rect,Color.Green);
            _spUI.Draw(statusTex, food_rect,Color.Red);
            _spUI.Draw(statusTex, water_rect, Color.Cyan);

            _spUI.Draw(statusTex, population_rect,Color.Blue);

            if (cursor == null) return;
            Rectangle mousecursorPosition = new Rectangle(UIcursorPos.X, UIcursorPos.Y, 20, 20);
            _spUI.Draw(cursor, mousecursorPosition, frame_cursor, Color.White);

            base.Draw_UI(ref _spUI);
        }

      
    }
}



