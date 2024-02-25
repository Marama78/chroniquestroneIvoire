using CTI_RPG.Scene;
using DinguEngine;

namespace CTI_RPG.CombatSystem
{
    public static class ActorCombat_MainRules
    {

        public static ATTACK[] GET_ATTACK_VALUES(actorType actorType)
        {
            ATTACK[] temp;

            switch (actorType)
            {
                case actorType.princess:

                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp;

                case actorType.soldier:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Acier rédempteur",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Rage du paladin",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Exorcisme",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("A genoux!",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp;
                case actorType.archer:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Comète",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Flèche de tonnerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Flèche paralysante",4,ATTACK_TYPE.PARALYSIE,4,4),
                        new ATTACK("Pluie d'étoiles",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp;
                case actorType.mage:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Boule de feu",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Temps absolu",1,ATTACK_TYPE.reduceSPEED,4,4),
                        new ATTACK("Météores",4,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Inferno",1,ATTACK_TYPE.FIRE,4,4),
                    };
                    return temp; 
                case actorType.priest:
                    return temp = new ATTACK[]
                    {
                        new ATTACK("Poing de Ryu",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Succube",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Chaînes nébulaire",4,ATTACK_TYPE.reduceSPEED,4,4),
                        new ATTACK("Gravité",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 

                case actorType.soldier_bad:
                    return temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.archer_bad:
                    return temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.mage_bad:
                    return temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.priest_bad:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 

                case actorType.kingGolem:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.orque:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.priestchief:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp;
                case actorType.magechief:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.soldiershief:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.queen:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.emperor:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.zombie:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.wolf:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.slim:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.carnivoreplant:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.golemstone:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.golemfire:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                case actorType.golemice:
                    temp = new ATTACK[]
                    {
                        new ATTACK("Coup de sabre",2,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Cri de guerre",1,ATTACK_TYPE.reduceFORCE,4,4),
                        new ATTACK("Multiples épines",4,ATTACK_TYPE.reduceHP,4,4),
                        new ATTACK("Vengeance sacrée",1,ATTACK_TYPE.reduceDEFENSE,4,4),
                    };
                    return temp; 
                default: return null;
            }
        }

        public static string[] GetRepliques(ref actorType type)
        {
            switch (type)
            {
                case actorType.princess:
                    return new string[]
                {
                    "Ma lame purgera votre infâmie",
                    "Ne vous mettez pas en travers de mon chemin!",
                    "l'acier est le langage que vous comprenez",
                    "J'en ai assez! mais vous ne me laissez pas le choix!"
                };

                case actorType.soldier:
                    return new string[]
                {
                    "Mon bouclier et mon épée protègeront notre Reine!",
                    "Vous ne toucherez aucun cheveux de notre Reine!",
                    "Je vous laisse encore du temps pour réfléchir!",
                    "Si seulement, nous pouvions éviter de nous battre!"
                };

                case actorType.archer:
                    return new string[]
                {
                    "Nul n'échappe à mon regard!",
                    "Vous ne me laissez pas le choix",
                    "Je rétablirai l'équilibre dans ce monde",
                    "Mes flèches sont des mots, aiguisés et profonds!"
                };

                case actorType.mage:
                    return new string[]
                {
                    "Les flammes purgeront vos crimes",
                    "L'ordre du néant a été rompu!",
                    "Comment osez-vous vous dressez devant moi!",
                    "Nul n'échappera à ma magie"
                };

                case actorType.priest:
                    return new string[]
                {
                    "Vous ne connaîtrez jamais la rédemption!",
                    "Vos crimes seront punis par les cieux!",
                    "Je vous laisse une chance de vous enfuir!",
                    "pff, je n'ai que faire de ces bêtises!"
                };


                default:
                    return new string[]
                {
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                };
            }
        }

        public static string[] GetAtkNames(ref actorType type)
        {
            switch (type)
            {
                case actorType.princess:
                    return new string[]
                {
                    "Coup de sabre",
                    "Cri de la Reine",
                    "Multiple épines",
                    "Peau de pierre"
                };

                case actorType.soldier:
                    return new string[]
            {
                    "Acier de purge",
                    "Rage du paladin",
                    "A genoux!",
                    "Purge des morts"
            };

                case actorType.archer:
                    return new string[]
            {
                    "Flèche sanguinaire",
                    "Invisibilité",
                    "Pluie d'étoiles",
                    "Assassinat"
            };

                case actorType.mage:
                    return new string[]
            {
                    "Boule de feu",
                    "Distorsion du temp",
                    "Pluie de météores",
                    "Plan de l'enfer"
            };

                case actorType.priest:
                    return new string[]
            {
                    "Tonnerre",
                    "Domination",
                    "Invocation",
                    "Poing légendaire"
            };


                default:
                    return new string[]
                {
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                };
            }


        }

        public static int GetHP(actorType actorType, int level)
        {
            switch (actorType)
            {
                case actorType.princess: return (int)(8 * level);
                case actorType.soldier: return (int)(8 * level);
                case actorType.archer: return (int)(8 * level);
                case actorType.mage: return (int)(5 * level);
                case actorType.priest: return (int)(5 * level);
                case actorType.kingGolem: return (int)(10 * level);
                case actorType.orque: return (int)(10 * level);
                case actorType.priestchief: return (int)(6 * level);
                case actorType.magechief: return (int)(6 * level);
                case actorType.soldiershief: return (int)(9 * level);
                case actorType.queen: return (int)(9 * level);
                case actorType.emperor: return (int)(15 * level);
                case actorType.zombie: return (int)(4 * level);
                case actorType.wolf: return (int)(4 * level);
                case actorType.slim: return (int)(4 * level);
                case actorType.carnivoreplant: return (int)(5 * level);
                case actorType.golemstone: return (int)(9 * level);
                case actorType.golemfire: return (int)(9 * level);
                case actorType.golemice: return (int)(9 * level);
                default: return 5;
            }
        }

        public static int GetDEF(actorType actorType, int level)
        {
            switch (actorType)
            {
                case actorType.princess: return (int)(2 * level);
                case actorType.soldier: return (int)(2 * level);
                case actorType.archer: return (int)(2 * level);
                case actorType.mage: return (int)(1 * level);
                case actorType.priest: return (int)(1 * level);
                case actorType.kingGolem: return (int)(4 * level);
                case actorType.orque: return (int)(4 * level);
                case actorType.priestchief: return (int)(1 * level);
                case actorType.magechief: return (int)(1 * level);
                case actorType.soldiershief: return (int)(3 * level);
                case actorType.queen: return (int)(4 * level);
                case actorType.emperor: return (int)(6 * level);
                case actorType.zombie: return (int)(1 * level);
                case actorType.wolf: return (int)(1 * level);
                case actorType.slim: return (int)(1 * level);
                case actorType.carnivoreplant: return (int)(1 * level);
                case actorType.golemstone: return (int)(4 * level);
                case actorType.golemfire: return (int)(4 * level);
                case actorType.golemice: return (int)(4 * level);
                default: return 5;
            }
        }


        public static int GetSPEED(actorType actorType, int level)
        {
            switch (actorType)
            {
                case actorType.princess: return (int)(4 * level);
                case actorType.soldier: return (int)(4 * level);
                case actorType.archer: return (int)(5 * level);
                case actorType.mage: return (int)(2 * level);
                case actorType.priest: return (int)(2 * level);
                case actorType.kingGolem: return (int)(1 * level);
                case actorType.orque: return (int)(3 * level);
                case actorType.priestchief: return (int)(2 * level);
                case actorType.magechief: return (int)(2 * level);
                case actorType.soldiershief: return (int)(5 * level);
                case actorType.queen: return (int)(4 * level);
                case actorType.emperor: return (int)(3 * level);
                case actorType.zombie: return (int)(2 * level);
                case actorType.wolf: return (int)(2 * level);
                case actorType.slim: return (int)(2 * level);
                case actorType.carnivoreplant: return (int)(2 * level);
                case actorType.golemstone: return (int)(2 * level);
                case actorType.golemfire: return (int)(2 * level);
                case actorType.golemice: return (int)(2 * level);
                default: return 5;
            }
        }

        public static int GetFORCE(actorType actorType, int level)
        {
            switch (actorType)
            {
                case actorType.princess: return (int)(4 * level);
                case actorType.soldier: return (int)(4 * level);
                case actorType.archer: return (int)(3 * level);
                case actorType.mage: return (int)(1 * level);
                case actorType.priest: return (int)(1 * level);
                case actorType.kingGolem: return (int)(6 * level);
                case actorType.orque: return (int)(6 * level);
                case actorType.priestchief: return (int)(1 * level);
                case actorType.magechief: return (int)(1 * level);
                case actorType.soldiershief: return (int)(5 * level);
                case actorType.queen: return (int)(4 * level);
                case actorType.emperor: return (int)(8 * level);
                case actorType.zombie: return (int)(2 * level);
                case actorType.wolf: return (int)(2 * level);
                case actorType.slim: return (int)(2 * level);
                case actorType.carnivoreplant: return (int)(2 * level);
                case actorType.golemstone: return (int)(5 * level);
                case actorType.golemfire: return (int)(5 * level);
                case actorType.golemice: return (int)(5 * level);
                default: return 5;
            }
        }




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
