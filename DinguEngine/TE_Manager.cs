using DinguEngine.Camera;
using DinguEngine.Shared;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Schema;

/* élève de l'école de jeux vidéos, gamecodeur.fr, depuis 2017,
 * ceci est un extrait d'un code perso, dans le pur esprit viking
 * Faîtes en bon usage. Pas besoin de crédits. Un viking, çà vit de
 * conquêtes et de victoire, brouaaaaaahhhhhh!!!!
 * ---------------------------------------------------------------
 * tu peux le recopier, l'utiliser à des fins perso et commerciales,
 * ou bien le modifier à ta guise.
 * EBB Dan Marama
*/

namespace DinguEngine
{
    public static class TE_Randomizer
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
    public enum actorType
    {
        princess,
        soldier,
        archer,
        mage,


        soldier_bad,
        archer_bad,
        mage_bad,
        priest_bad,

        priest,
        kingGolem,
        orque,
        priestchief,
        magechief,
        soldiershief,
        queen,
        emperor,
        zombie,
        wolf,
        slim,
        carnivoreplant,
        golemstone,
        golemfire,
        golemice

    }
    public enum actorRefuge
    {
        worker,
        artisan,
        miner,
        chomeur,
        soldier,
        archer,
        mage,
        pretre,

        soldier_bad,
        archer_bad,
        mage_bad,
        priest_bad,

        monster,
        golem_stone,
        golem_fire,
        golem_ice,
        miniboss,
        bigboss,
    }

    public static class TE_Manager
    {

        public static List<actorType> friends = new List<actorType>();
        public static List<actorType> ennemies = new List<actorType>();
        public static List<actorRefuge> refuge_friends = new List<actorRefuge>();
        public static List<actorRefuge> refuge_ennemies = new List<actorRefuge>();


        public static bool shakefriend = false, shakeennemy = false;

        public static int tileW = 40, tileH = 20;

        public static int gridW = 11, gridH = 9;
        public static int windowW, windowH;
        public static int viewportWidth = 11;
        public static KeyboardState kbstate = Keyboard.GetState();
        public static KeyboardState old_kbstate;
        public static Keys old_key;

        public static TE_Camera camera = new TE_Camera();
        public static bool shakeViewPort = false;

        public static float screenW = 240, screenH = 160;

        public static List<xmlTexture2D> xmlTexture2Ds = new List<xmlTexture2D>();
        public static List<Texture2D> keyboardTex = new List<Texture2D>();
        public static TextureData[] tilesets_path;

        public static string customextension = ".dat";

        public static int GetOffsetCameraMAX_X()
        {
            return (int)((tileW / 2) + tileW);
        }
      

    }
}
