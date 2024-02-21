using DinguEngine.Shared;
using DinguEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _TheShelter.GameWrapping
{

    public enum FightMode
    {
        monsterlvl1,
        monsterlvl2,
        monsterlvl3,
        monsterlvl4,
        monsterlvl5,
        soldierlvl1,
        soldierlvl2,
        soldierlvl3,
        archerlvl1,
        archerlvl2,
        archerlvl3,
        magelvl1,
        magelvl2,
        magelvl3,
        priestlvl1,
        priestlvl2,
        priestlvl3,
        boss_orque,
        boos_mage,
        boss_priest,
        boss_soldier,
        boss_shanae,
        boss_empereur,
    }



    public struct DataRefuge
    {
        public int totalbuilddortoir;
       public int totalbuildfasfood;
       public int totalbuildminecharbon;
      public int totalbuildmagic;
       public int totalbuildforge;
       public int totalbuildfiltreeau;
       public int totalbuildbiere;
        public int totalbuildstockage;
        public int totaleau;
        public int totalpain;
        public int totalpopulation;
        public int totalenergie;
        public int bank;

        public int currentDay;
     public   DrawRect[] daysVisual;
    }

    public static class RefugeWrapper
    {
        public static int stateWrapping = 0;
        public static TE_Button[] map;
        public static FightMode _fightmode;
        public static DataRefuge datarefuge = new DataRefuge();



        public static void SaveDataRefuge(ref DataRefuge _data)
        {
            datarefuge = _data;
        }

        public static void LoadDataRefuge(ref DataRefuge _data)
        {
            _data = datarefuge;
        }
        public static void SaveDataInWrapper(ref TE_Button[] _map)
        {
            stateWrapping = 1;
            map = _map;
        }

        public static void LoadDataWrapper_MAP(ref TE_Button[] _map)
        {
            if(stateWrapping == 0) { return; }
            _map = map;
        }
    }
}
