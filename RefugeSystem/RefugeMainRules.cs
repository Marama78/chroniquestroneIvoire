using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace _TheShelter.RefugeSystem
{
    public static class RefugeMainRules
    {


        public static string GetName(int typeOfBuilding)
        {
            //-- prix des bâtiments

            switch (typeOfBuilding)
            {
                case 0: return "Dortoir"; // dortoir
                case 1: return "Salle à manger"; // salle à manger
                case 2: return "Fontaine eau"; // filtre eau
                case 3: return "Mine de charbon"; // mine de charbon
                case 4: return "Réserve"; // salle de stockage
                case 5: return "Arbre mana"; // labo mana
                case 6: return "Forge"; // forge
                case 7: return "Cave à vin"; // biere
                case 8: return "Escalier"; // escalier
                case 9: return "Démonter"; // kill tile
                default: return string.Empty;
            }
        }




        public static int GetPrice(int typeOfBuilding)
        {
            //-- prix des bâtiments

            switch(typeOfBuilding)
            {
                case 0: return 100; // dortoir
                case 1: return 100; // salle à manger
                case 2: return 200; // filtre eau
                case 3: return 300; // mine de charbon
                case 4: return 1000; // salle de stockage
                case 5: return 2000; // labo mana
                case 6: return 800; // forge
                case 7: return 1500; // biere
                case 8: return 100; // escalier
                case 9: return 50; // kill tile
                default: return 1000;
            }
        }

        public static bool FreeBuildings(int typeOfBuilding, int population)
        {
            switch (typeOfBuilding)
            {
                case 0: 
                    if (population >= 0)  {return true; }  break; // dortoir
                case 1:
                    if (population >= 0)  {  return true; }  break;  // salle à manger
                case 2:
                    if (population >= 0)
                    {
                        return true;
                    }
                    break; // filtre eau
                case 3:
                    if (population >= 0)
                    {
                        return true;
                    }
                    break; // mine de charbon
                case 4:
                    if (population >= 40)
                    {
                        return true;
                    }
                    break; // salle de stockage
                case 5:
                    if (population >= 20)
                    {
                        return true;
                    }
                    break; // labo mana
                case 6:
                    if (population >= 12)
                    {
                        return true;
                    }
                    break; // forge
                case 7:
                    if (population >= 60)
                    {
                        return true;
                    }
                    break; // biere
                case 8: 
                    if (population >= 0) 
                    {
                        return true; 
                    } 
                    break; // escalier

                case 9: return true; // kill tile
                default: return false;
            }
            return false;
        }

    }
}
