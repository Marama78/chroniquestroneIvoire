using DinguEngine;
using DinguEngine.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CTI_RPG.CombatSystem
{
    public enum ATTAXK_SFX
    {
        sword,
        fire,
        thunder,
        stone,

    }

    public enum ATTACK_TYPE
    {
        reduceHP,
        reduceSPEED,
        reduceFORCE,
        reduceDEFENSE,
        drainHP,
        SLEEP,
        POISON,
        PARALYSIE,
        FIRE,
        SHADOW,

    }

    public struct ATTACK
    {
        public string Name;
        public int damage;
        public ATTACK_TYPE type;
        public int crit_probability;
        public int precision;

        public ATTACK(string name, int damage, ATTACK_TYPE type, int crit_probability, int precision)
        {
            Name = name;
            this.damage = damage;
            this.type = type;
            this.crit_probability = crit_probability;
            this.precision = precision;
        }
    }

    public class Actor
    {
        public int poisoned, paralysed, sleeping, onFire, shadowed;

        public int Force = 1;
        public int Defense =1;
        public int Speed = 1;
        public ATTACK[] Attack;
        public int level = 1;
        public int HP = 1;
        public int MAX_HP = 1;

        public double currentXP = 0;
        public int id = 0;
        public int2 frameposition;
        public int2 frameSize;
        public Actor(actorType _type, int _texture, int2 _framePosition, int2 _frameSize, int _portraitID = 0, int level = 0)
        {
            textureID = _texture;
            type = _type;
            this.frameposition = _framePosition;
            this.frameSize = _frameSize;

            frame = new Rectangle(_framePosition.x * _frameSize.x, _framePosition.y * _frameSize.y, _frameSize.x, _frameSize.y);
            name = ActorCombat_MainRules.Get_Name(_type);

         
            magic = MAXmagic;
            textur2D_portraitID = _portraitID;
            attaqueNames = ActorCombat_MainRules.GetAtkNames(ref _type);
            repliques = ActorCombat_MainRules.GetRepliques(ref _type);

            UpdateLevel(level, _type);

            Attack = ActorCombat_MainRules.GET_ATTACK_VALUES(_type);
        }

        public void UpdateLevel(int level, actorType _type)
        {
            MAX_HP = ActorCombat_MainRules.GetHP(_type, level);
            HP = ActorCombat_MainRules.GetHP(_type, level);
            Defense = ActorCombat_MainRules.GetDEF(_type, level);
            Speed = ActorCombat_MainRules.GetSPEED(_type, level);
            Force = ActorCombat_MainRules.GetFORCE(_type, level);
        }

        public void UpdateLevel(int _level)
        {
            this.level = _level;
            MAX_HP = ActorCombat_MainRules.GetHP(type, _level);
            HP = ActorCombat_MainRules.GetHP(type, _level);
            Defense = ActorCombat_MainRules.GetDEF(type, _level);
            Speed = ActorCombat_MainRules.GetSPEED(type, _level);
            Force = ActorCombat_MainRules.GetFORCE(type, _level);
        }

        public int actionPoint = 1;


        public string GetSomeWords()
        {
            return "Ma lame purgera votre infamie!";
        }


        public string[] attaqueNames;
        public string[] repliques;


        public int textur2D_portraitID;

        public actorType type;
        public Texture2D texture;
        public Rectangle frame;

        public int textureID;
        public string name;
      
        public int magic;

        public int MAXlife;
        public int MAXmagic;

       
     
        public int exp;

        public int atak_pts;
        public int magic_atk_pts_fire, magic_atk_atk_ice, magic_atk_atk_earth;
        public int priest_atk_drain_life, priest_atak_drain_magic, priest_healing;

        public int UpdateLevel()
        {
            if (level < 200) return 1;
            else if (level < 400) return 2;
            else if (level < 800) return 3;
            else if (level < 1600) return 5;
            else if (level < 3200) return 6;
            else if (level < 6400) return 7;
            else if (level < 12800) return 8;
            else if (level < 25600) return 9;
            else return 10;
        }

        public void AddEXP(int _exp)
        {
            exp += _exp;
        }

        public int GetLifeMAXBar()
        {
            level = UpdateLevel();
            return ActorCombat_MainRules.Get_LIFE_BAR(type, level);
        }

        public int GetMagicMaxBAR()
        {
            level = UpdateLevel();
            return ActorCombat_MainRules.Get_LIFE_BAR(type, level);
        }

        public void Hitted(ref int hit)
        {
            this.HP -= hit;
        }

        public actorType GetActorTYpe()
        {
            return type;
        }
    }
}
