using _TheShelter.Scene;
using DinguEngine;
using DinguEngine.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace _TheShelter.CombatSystem
{
    public class Actor
    {
        public Actor(actorType _type, int _texture)
        {
            textureID = _texture;
            type = _type;
            frame = new Rectangle(0, 0, 75, 95);

            name = ActorsStatesRules.Get_Name(_type);
            MAXlife = GetLifeMAXBar();
            MAXmagic = GetMagicMaxBAR();
            life = MAXlife;
            
            magic = MAXmagic;
        }

        public Actor(actorType _type, int _texture, int2 framePosition, int2 frameSize)
        {
            textureID = _texture;
            type = _type;

            frame = new Rectangle(framePosition.x * frameSize.x, framePosition.y * frameSize.y, frameSize.x, frameSize.y);
            name = ActorsStatesRules.Get_Name(_type);

            MAXlife = GetLifeMAXBar();
            MAXmagic = GetMagicMaxBAR();
            life = MAXlife;
            magic = MAXmagic;
        }
        public actorType type;
        public Texture2D texture;
        public Rectangle frame;

        public int textureID;
        public string name;
        public int life;
        public int magic;

        public int MAXlife;
        public int MAXmagic;

        public int defense;
        public int level;
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
            return ActorsStatesRules.Get_LIFE_BAR(type, level);
        }

        public int GetMagicMaxBAR()
        {
            level = UpdateLevel();
            return ActorsStatesRules.Get_LIFE_BAR(type, level);
        }

        public void Hitted(ref int hit)
        {
            this.life -= hit;
        }

        public actorType GetActorTYpe()
        {
            return type;
        }
    }
}
