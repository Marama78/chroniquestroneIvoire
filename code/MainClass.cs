using _TheShelter.Scene;
using DinguEngine;
using DinguEngine.Camera;
using DinguEngine.Scenes;
using DinguEngine.Shared;
using DinguEngine.UI.TE_Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    }


    public class SceneManager
    {
        public ModelScene currentScene;
        bool isLoadingNewScene = true;

        public ModelScene combat, refuge;

        private MainClass main;

        public SceneManager(MainClass _main)
        {
            combat = new CombatMode(main);
            refuge = new Refuge(main);
            main = _main;
        }

        public void LoadScene(scene scenetoload)
        {
            if (currentScene != null)
            {
                currentScene = null;
                isLoadingNewScene = true;
            }



            switch (scenetoload)
            {
                case scene.devmode: currentScene = new CombatMode(main); break;


                case scene.splashscreen: currentScene = new SplashScreen(main); break;
                case scene.start: currentScene = new Start(main); break;
                case scene.newgame: currentScene = new NewGame(main); break;
                case scene.loose: currentScene = new Loose(main); break;
                case scene.combatmode: currentScene = new CombatMode(main); break;
                case scene.refuge: currentScene = new Refuge(main); break;
                case scene.quit: break;
            }


        }

    }

    public abstract class ModelScene
    {
        public TE_Camera maincamera;
        public TE_Camera uicamera;
        public TE_Camera friendcamera;
        public TE_Camera ennemycamera;
        protected MainClass main;
        protected SpriteFont mainFont, cutsceneFont, UI_Fonts;
        protected int2 cameraOffset = new int2(-120, -80);

        protected Rectangle ToOffset(Rectangle value)
        {
            return new Rectangle(value.X + cameraOffset.x, value.Y + cameraOffset.y, value.Width, value.Height); ;
        }

        protected Vector2 ToOffset(Vector2 value)
        {
            return new Vector2(value.X + cameraOffset.x, value.Y + cameraOffset.y);
        }

        protected ModelScene(MainClass _mainclass)
        {
            main = _mainclass;
        }

        public virtual void Load(ref ContentManager _content)
        {
            mainFont = _content.Load<SpriteFont>("mainFont");
            cutsceneFont = _content.Load<SpriteFont>("cutsceneFont");
            UI_Fonts = _content.Load<SpriteFont>("UI_Fonts");
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(ref SpriteBatch _sp)
        {

        }

        public virtual void Draw_UI(ref SpriteBatch _spUI)
        {

        }

        public virtual void Draw_Friend(ref SpriteBatch _spUI)
        {

        }

        public virtual void Draw_Ennemy(ref SpriteBatch _spUI)
        {

        }

        public void PlayAudio(SoundEffect effect, float volume = 1.0f)
        {
            effect.Play(volume, pitch: 0.0f, pan: 1.0f);
        }

    }
    public static class Randomizer
    {
        public static Random randInt = new Random();

        public static int GiveRandomInt(out int value, int min, int max)
        {
            return value = randInt.Next(min, max);
        }

        public static int GiveRandomInt(int min, int max)
        {
            return randInt.Next(min, max);
        }

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
        public MainClass()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _content = Content;
            // IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1200;// 960;// 760;// 480;// 240;
            _graphics.PreferredBackBufferHeight = 800;// 640;// 480;// 320;// 160;
            _graphics.ApplyChanges();


        }

        protected override void Initialize()
        {
            IsMouseVisible = false;

            Window.Title = "Les chroniques du trône d'Ivoire";
            Window.AllowUserResizing = true;
            base.Initialize();
        }


        public static Point GetWindowSize()
        {
            return new Point(_window.ClientBounds.Width, _window.ClientBounds.Height);
        }

        protected override void LoadContent()
        {
            TE_Manager.screenW = 1200;// _graphics.GraphicsDevice.Viewport.Width; 
            TE_Manager.screenH = 800;// _graphics.GraphicsDevice.Viewport.Height;
            TE_Manager.viewportWidth = 1200;// _graphics.PreferredBackBufferWidth;  


            scenemanager = new SceneManager(this);
            ///* scenemanager.LoadScene(scene.splashscreen, this);
            scenemanager.LoadScene(scene.devmode);
            scenemanager.currentScene.Load(ref _content);

            _window = Window;

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

        protected override void Update(GameTime gameTime)
        {
            if (oldscene != scenemanager.currentScene)
            {
                scenemanager.currentScene.Load(ref _content);
                oldscene = scenemanager.currentScene;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (scenemanager.currentScene != null)
            {
                scenemanager.currentScene.Update();
            }

            if (TE_Manager.shakeennemy || TE_Manager.shakefriend)
            {
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
                    shakeoffset = new Vector2(0, 0);
                }

                Vector2 anchor = new Vector2(0, 0);

                shakeoffset += anchor;

                if(TE_Manager.shakefriend)
                {
                    scenemanager.currentScene.friendcamera.CenterOn(shakeoffset);
                }
                else if(TE_Manager.shakeennemy)
                {
                    scenemanager.currentScene.ennemycamera.CenterOn(shakeoffset);
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (scenemanager.currentScene == null) return;
            if (scenemanager.currentScene.maincamera == null) { return; }
            GraphicsDevice.Clear(Color.Black);

            if (scenemanager.currentScene != null)
            {
                _spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                samplerState: SamplerState.PointClamp,
                transformMatrix: scenemanager.currentScene.maincamera.TranslationMatrix);

                _spriteBatch_combatFriend.Begin(
               SpriteSortMode.BackToFront,
               samplerState: SamplerState.PointClamp,
               transformMatrix: scenemanager.currentScene.friendcamera.TranslationMatrix);

                _spriteBatch_combatEnnemy.Begin(
                SpriteSortMode.BackToFront,
                samplerState: SamplerState.PointClamp,
                transformMatrix: scenemanager.currentScene.ennemycamera.TranslationMatrix);

                _spriteBatch_UserInterface.Begin(
                SpriteSortMode.BackToFront,
                samplerState: SamplerState.PointClamp,
                transformMatrix: scenemanager.currentScene.uicamera.TranslationMatrix);

                scenemanager.currentScene.Draw(ref _spriteBatch);
                scenemanager.currentScene.Draw_Friend(ref _spriteBatch_combatFriend);
                scenemanager.currentScene.Draw_Ennemy(ref _spriteBatch_combatEnnemy);
                scenemanager.currentScene.Draw_UI(ref _spriteBatch_UserInterface);

                _spriteBatch.End();
                _spriteBatch_combatFriend.End();
                _spriteBatch_combatEnnemy.End();
                _spriteBatch_UserInterface.End();
            }
            base.Draw(gameTime);
        }

        public void ChangeScene(scene scenetoload)
        {
            scenemanager.LoadScene(scenetoload);
        }
    }
}
