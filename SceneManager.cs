using CTI_RPG.Scene;

namespace CTI_RPG
{
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
            currentScene = new SplashScreen(main);
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
                case scene.devmode: currentScene = new SplashScreen(main); break;

                case scene.splashscreen: currentScene = new SplashScreen(main); break;
                case scene.start: currentScene = new Start(main); break;

                case scene.newgame: currentScene = new NewGame(main); break;
                case scene.loose: currentScene = new Loose(main); break;

                case scene.combatmode: currentScene = new RPG_systemCombat(main); break; ///CombatMode(main); break;

                case scene.refuge:  currentScene=new Refuge(main); break;
                case scene.quit: break;

                case scene.standardFight: currentScene = new StandarFight(main); break;
                case scene.credits: currentScene = new Credits(main); break;
            }


        }

    }
}
