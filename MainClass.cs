using DinguEngine;
using DinguEngine.Camera;
using DinguEngine.Scenes;
using DinguEngine.UI.TE_Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Linq;

namespace TheShelter
{
    public enum scene
    {
        splashscreen,
        start,
        cuscene,
        combatmode,
        refuge,
        devmode,
        newgame,
        quit,
        loose,
        standardFight,
        credits
    }

    public class MainClass : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _spriteBatch_UserInterface;
        private SpriteBatch _spriteBatch_combatFriend;
        private SpriteBatch _spriteBatch_combatEnnemy;
        private ContentManager _content;

        
        public static GameWindow _window;

        TE_ModelScene _modelScene;

        SceneManager scenemanager;

        ModelScene oldscene;

        ContentManager content;

        bool devmodeON = true;

        private readonly Canvas _canvas;

        public int screenW, screenH;
        public Game _game
        {
            get { return this; }
        }

        public GraphicsDeviceManager _graphicExt
        {
            get { return _graphics; }
        }
        public MainClass()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _content = Content;
            // IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth =  960;// 720;// 480;// 240;
            _graphics.PreferredBackBufferHeight =  640;// 480;// 320;// 160;
            _graphics.ApplyChanges();
          
        }

        public void SetResolution(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            TE_Manager._graphics = _graphics;
            IsMouseVisible = false;

            Window.Title = "Les chroniques du trône d'Ivoire";
            Window.AllowUserResizing = true;
            base.Initialize();
        }


        public static Point GetWindowSize()
        {
            return new Point(_window.ClientBounds.Width, _window.ClientBounds.Height);
        }

        RenderTarget2D _rendertarget;
        float scale = 0.444f;
        protected override void LoadContent()
        {
            _rendertarget = new RenderTarget2D(_graphics.GraphicsDevice, 240, 160);


            TE_Manager.screenW = 960;// _graphics.GraphicsDevice.Viewport.Width; 
            TE_Manager.screenH = 640;// _graphics.GraphicsDevice.Viewport.Height;
            TE_Manager.viewportWidth = 960;// _graphics.PreferredBackBufferWidth;  


            scenemanager = new SceneManager(this);

            if (devmodeON) { scenemanager.LoadScene(scene.devmode); }
            else { scenemanager.LoadScene(scene.splashscreen); }
            scenemanager.currentScene.Load(ref _content);


            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteBatch_UserInterface = new SpriteBatch(GraphicsDevice);
            _spriteBatch_combatFriend = new SpriteBatch(GraphicsDevice);
            _spriteBatch_combatEnnemy = new SpriteBatch(GraphicsDevice);

        }
        float chronoShake = 0;
        float speedShake = 0.25f;
        private float shakeStartAngle, shakeRadius;
        double shakeStart;
        private Vector2 shakeoffset;

        int kbticks = 0;

        bool fullscreen = false;

        private void UpdateCameras(int virtualwidth, int virtualheight)
        {
            float zoom;
            if (_graphics.PreferredBackBufferWidth < _graphics.PreferredBackBufferHeight)
            {
                zoom = _graphics.PreferredBackBufferWidth / virtualwidth;
             
            }
            else
            {
                zoom = _graphics.PreferredBackBufferHeight / virtualheight;
            }

            if (scenemanager.currentScene.maincamera != null) { scenemanager.currentScene.maincamera.SetZoom(zoom); }
            if (scenemanager.currentScene.uicamera != null) { scenemanager.currentScene.uicamera.SetZoom(zoom); }
            if (scenemanager.currentScene.ennemycamera != null) { scenemanager.currentScene.ennemycamera.SetZoom(zoom); }
            if (scenemanager.currentScene.friendcamera != null) { scenemanager.currentScene.friendcamera.SetZoom(zoom); }
        }

        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                SetResolution(1200, 800);
                _graphics.IsFullScreen = false;
                UpdateCameras(240, 160);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                SetResolution(960, 640);
                _graphics.IsFullScreen = false;
                UpdateCameras(240, 160);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                SetResolution(720, 480);
                _graphics.IsFullScreen = false;
                UpdateCameras(240, 160);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F4))
            {
               

                fullscreen = !fullscreen;
                _graphics.IsFullScreen = fullscreen;
                _graphics.ApplyChanges();

                UpdateCameras(240, 160);
            }

            if (scenemanager.currentScene != null)
            {
                if (oldscene != scenemanager.currentScene)
                {
                    scenemanager.currentScene.Load(ref _content);
                    oldscene = scenemanager.currentScene;
                }
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (scenemanager.currentScene != null)
            {
                scenemanager.currentScene.Update();
            }

            if (TE_Manager.shakeennemy || TE_Manager.shakefriend)
            {
                if (scenemanager.currentScene.friendcamera == null)
                {
                    scenemanager.currentScene.friendcamera = new DinguEngine.Camera.TE_Camera( );
                }

                if (scenemanager.currentScene.ennemycamera == null)
                {
                    scenemanager.currentScene.ennemycamera = new DinguEngine.Camera.TE_Camera( );
                }

                chronoShake += speedShake;

                shakeoffset = new Vector2((float)(Math.Sin(shakeStartAngle) * shakeRadius), (float)(Math.Cos(shakeStartAngle) * shakeRadius));
                shakeRadius -= 0.50f;
                shakeStartAngle += (150 + Randomizer.randInt.Next(60));
                if (chronoShake > 1)
                {
                    chronoShake = 0;
                    TE_Manager.shakeennemy = false;
                    TE_Manager.shakefriend = false;
                    shakeStart = 0;
                    shakeRadius = 0;
                    shakeoffset = new Vector2(0, 0);
                    scenemanager.currentScene.friendcamera.CenterOn(shakeoffset);
                    scenemanager.currentScene.ennemycamera.CenterOn(shakeoffset);
                }

                Vector2 anchor = new Vector2(0, 0);

                shakeoffset += anchor;

                if (TE_Manager.shakefriend)
                {
                    if (scenemanager.currentScene.friendcamera != null)
                    {
                        scenemanager.currentScene.friendcamera.CenterOn(shakeoffset);
                    }
                    else
                    {
                        scenemanager.currentScene.friendcamera = new DinguEngine.Camera.TE_Camera( );
                    }
                }
                else if (TE_Manager.shakeennemy)
                {
                    if (scenemanager.currentScene.ennemycamera != null)
                    {
                        scenemanager.currentScene.ennemycamera.CenterOn(shakeoffset);
                    }
                    else
                    {
                        scenemanager.currentScene.ennemycamera = new DinguEngine.Camera.TE_Camera( );
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
          
            GraphicsDevice.Clear(Color.Black);

               if (scenemanager.currentScene == null) return;
               if (scenemanager.currentScene.maincamera == null) { return; }
               GraphicsDevice.Clear(Color.Black);
               if (scenemanager.currentScene != null)
               {
                   if (scenemanager.currentScene.maincamera != null)
                   {
                       _spriteBatch.Begin(
                       SpriteSortMode.BackToFront,
                       samplerState: SamplerState.PointClamp,
                       transformMatrix: scenemanager.currentScene.maincamera.TranslationMatrix);
                   }



                   if (scenemanager.currentScene.ennemycamera != null)
                   {
                       _spriteBatch_combatEnnemy.Begin(
                       SpriteSortMode.BackToFront,
                       samplerState: SamplerState.PointClamp,
                       transformMatrix: scenemanager.currentScene.ennemycamera.TranslationMatrix);
                   }

                   if (scenemanager.currentScene.friendcamera != null)
                   {
                       _spriteBatch_combatFriend.Begin(
                       SpriteSortMode.BackToFront,
                       samplerState: SamplerState.PointClamp,
                       transformMatrix: scenemanager.currentScene.friendcamera.TranslationMatrix);
                   }

                   if (scenemanager.currentScene.uicamera != null)
                   {
                       _spriteBatch_UserInterface.Begin(
                       SpriteSortMode.BackToFront,
                       samplerState: SamplerState.PointClamp,
                       transformMatrix: scenemanager.currentScene.uicamera.TranslationMatrix);
                   }

                   //-- <DRAW()>
                   if (scenemanager.currentScene.maincamera != null) { scenemanager.currentScene.Draw(ref _spriteBatch); }
                   if (scenemanager.currentScene.ennemycamera != null) { scenemanager.currentScene.Draw_Ennemy(ref _spriteBatch_combatEnnemy); }
                   if (scenemanager.currentScene.friendcamera != null) { scenemanager.currentScene.Draw_Friend(ref _spriteBatch_combatFriend); }
                   if (scenemanager.currentScene.uicamera != null) { scenemanager.currentScene.Draw_UI(ref _spriteBatch_UserInterface); }

                   //-- <END()>
                   if (scenemanager.currentScene.maincamera != null) { _spriteBatch.End(); }
                   if (scenemanager.currentScene.ennemycamera != null) { _spriteBatch_combatEnnemy.End(); }
                   if (scenemanager.currentScene.friendcamera != null) { _spriteBatch_combatFriend.End(); }
                   if (scenemanager.currentScene.uicamera != null) { _spriteBatch_UserInterface.End(); }

               }

             
             

            base.Draw(gameTime);
        }

        public void ChangeScene(scene scenetoload)
        {
            scenemanager.LoadScene(scenetoload);
        }
    }
}
