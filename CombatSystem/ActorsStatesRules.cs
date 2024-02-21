using _TheShelter.Scene;
using DinguEngine;

namespace _TheShelter.CombatSystem
{
    public static class ActorsStatesRules
    {

        public static int GetAtk_melee_degats(actorType actorType)
        {
            float level = 10.0f;
            switch (actorType)
            {
                case actorType.princess: return (int)(20 * level);

                case actorType.soldier: return (int)(3 * level); 
                case actorType.archer: return (int)(3 * level);
                case actorType.mage: return (int)(1 * level);
                case actorType.priest: return (int)(1 * level);
                case actorType.kingGolem: return (int)(6 * level);
                case actorType.orque: return (int)(6 * level);
                case actorType.priestchief: return (int)(4 * level);
                case actorType.magechief: return (int)(4 * level);
                case actorType.soldiershief: return (int)(10 * level);
                case actorType.queen: return (int)(2 * level);
                case actorType.emperor: return (int)(15 * level);
                case actorType.zombie: return (int)(4 * level);
                case actorType.wolf: return (int)(4 * level);
                case actorType.slim: return (int)(1 * level);
                case actorType.carnivoreplant: return (int)(10 * level);
                case actorType.golemstone: return (int)(5 * level);
                case actorType.golemfire: return (int)(5 * level);
                case actorType.golemice: return (int)(5 * level);
                default: return 5; 
            }
        }

        public static  int GetAtk_MAGIC_degats(SFX_style typeOfCast)//actorType actorType)
        {
            switch(typeOfCast)
            {
                case SFX_style.fire: return 15;
                    case SFX_style.ice: return 15;
                    case SFX_style.thunder: return 15;
                    case SFX_style.fistofryu: return 45;
                    case SFX_style.invoke: return 35;
                    case SFX_style.earth: return 25;
                    default: return 15;
            }


            /*float level = 17.0f;
            switch (actorType)
            {
                case actorType.princess: return (int)(8 * level);

                case actorType.soldier: return (int)(5 * level);
                case actorType.archer: return (int)(5 * level);
                case actorType.mage: return (int)(8 * level);
                case actorType.priest: return (int)(8 * level);


                case actorType.soldier_bad: return (int)(5 * level);
                case actorType.archer_bad: return (int)(5 * level);
                case actorType.mage_bad: return (int)(8 * level);
                case actorType.priest_bad: return (int)(8 * level);

                case actorType.kingGolem: return (int)(5 * level);
                case actorType.orque: return (int)(5 * level);
                case actorType.priestchief: return (int)(12 * level);
                case actorType.magechief: return (int)(12 * level);
                case actorType.soldiershief: return (int)(5 * level);
                case actorType.queen: return (int)(20 * level);
                case actorType.emperor: return (int)(25 * level);
                case actorType.zombie: return (int)(5 * level);
                case actorType.wolf: return (int)(5 * level);
                case actorType.slim: return (int)(15 * level);
                case actorType.carnivoreplant: return (int)(5 * level);
                case actorType.golemstone: return (int)(5 * level);
                case actorType.golemfire: return (int)(10 * level);
                case actorType.golemice: return (int)(10 * level);
                default: return 5;
            }*/
        }

        public static int Get_MAGIC_cost(actorType actorType)
        {

            float level = 6.0f;
            switch (actorType)
            {
                case actorType.princess: return (int)(1 * level);

                case actorType.soldier: return (int)(3 * level);
                case actorType.archer: return (int)(3 * level);
                case actorType.mage: return (int)(3 * level);
                case actorType.priest: return (int)(3 * level);

                case actorType.soldier_bad: return (int)(3 * level);
                case actorType.archer_bad: return (int)(3 * level);
                case actorType.mage_bad: return (int)(3 * level);
                case actorType.priest_bad: return (int)(3 * level);

                case actorType.kingGolem: return (int)(3 * level);
                case actorType.orque: return (int)(3 * level); ;
                case actorType.priestchief: return (int)(2 * level);
                case actorType.magechief: return (int)(2 * level);
                case actorType.soldiershief: return (int)(3 * level);
                case actorType.queen: return (int)(1 * level);
                case actorType.emperor: return (int)(1 * level);
                case actorType.zombie: return (int)(5 * level);
                case actorType.wolf: return (int)(3 * level);
                case actorType.slim: return (int)(1 * level);
                case actorType.carnivoreplant: return (int)(3 * level);
                case actorType.golemstone: return (int)(3 * level);
                case actorType.golemfire: return (int)(3 * level);
                case actorType.golemice: return (int)(3 * level);
                default: return 5;
            }
        }


        public static int Get_MAGIC_BAR(actorType actorType)
        {
            float level = 1.0f;
            switch (actorType)
            {
                case actorType.princess: return (int)(32 * level);

                case actorType.soldier: return (int)(32 * level);
                case actorType.archer: return (int)(32 * level);
                case actorType.mage: return (int)(21 * level);
                case actorType.priest: return (int)(21 * level);

                case actorType.soldier_bad: return (int)(32 * level);
                case actorType.archer_bad: return (int)(32 * level);
                case actorType.mage_bad: return (int)(21 * level);
                case actorType.priest_bad: return (int)(21 * level);


                case actorType.kingGolem: return (int)(30 * level);
                case actorType.orque: return (int)(0 * level);
                case actorType.priestchief: return (int)(50 * level);
                case actorType.magechief: return (int)(50 * level);
                case actorType.soldiershief: return (int)(21 * level);
                case actorType.queen: return (int)(50 * level);
                case actorType.emperor: return (int)(50 * level);
                case actorType.zombie: return (int)(15 * level);
                case actorType.wolf: return (int)(10 * level);
                case actorType.slim: return (int)(4 * level);
                case actorType.carnivoreplant: return (int)(20 * level);
                case actorType.golemstone: return (int)(15 * level);
                case actorType.golemfire: return (int)(15 * level);
                case actorType.golemice: return (int)(15 * level);
                default: return 5;
            }
        }



        public static int Get_LIFE_BAR(actorType actorType, float level)
        {
            switch (actorType)
            {
                case actorType.princess: return (int)(300 * level);

                case actorType.soldier: return (int)(100 * level);
                case actorType.archer: return (int)(100 * level);
                case actorType.mage: return (int)(40 * level);
                case actorType.priest: return (int)(45 * level);

                case actorType.soldier_bad: return (int)(100 * level);
                case actorType.archer_bad: return (int)(100 * level);
                case actorType.mage_bad: return (int)(40 * level);
                case actorType.priest_bad: return (int)(45 * level);


                case actorType.kingGolem: return (int)(100 * level);
                case actorType.orque: return (int)(100 * level);
                case actorType.priestchief: return (int)(150 * level);
                case actorType.magechief: return (int)(150 * level);
                case actorType.soldiershief: return (int)(100 * level);
                case actorType.queen: return (int)(140 * level);
                case actorType.emperor: return (int)(180 * level);
                case actorType.zombie: return (int)(100 * level);
                case actorType.wolf: return (int)(100 * level);
                case actorType.slim: return (int)(80 * level);
                case actorType.carnivoreplant: return (int)(100 * level);
                case actorType.golemstone: return (int)(100 * level);
                case actorType.golemfire: return (int)(150 * level);
                case actorType.golemice: return (int)(155 * level);
                default: return 5;
            }
        }

        public static string Get_Name(actorType actorType)
        {
            switch (actorType)
            {
                case actorType.princess: return "Princesse";

                case actorType.soldier: return "Paladin";
                case actorType.archer: return "Hunter";
                case actorType.mage: return "Mage de guerre";
                case actorType.priest: return "Prêtresse";

                case actorType.soldier_bad: return "bandit";
                case actorType.archer_bad: return "mercenaire";
                case actorType.mage_bad: return "sorciere";
                case actorType.priest_bad: return "invocatrice";

                case actorType.kingGolem: return "Roi des Golems";
                case actorType.orque: return "Gualvak";
                case actorType.priestchief: return "Avatar divin";
                case actorType.magechief: return "Gardienne des ombres";
                case actorType.soldiershief: return "Excalibur";
                case actorType.queen: return "Reine Shanae";
                case actorType.emperor: return "Empereur EBEN";
                case actorType.zombie: return "pas de chance";
                case actorType.wolf: return "loup wof wof";
                case actorType.slim: return "glissière";
                case actorType.carnivoreplant: return "ouillesapike";
                case actorType.golemstone: return "Golem terre";
                case actorType.golemfire: return "Golem feu";
                case actorType.golemice: return "Golem glace";
                default: return "fantôme";
            }
        }

    }
}
