using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _TheShelter.cutscenesManagement
{
    public class CutsceneData
    {
        public List<Texture2D> textures;
        public List<string> innertext_line1;
        public List<string> innertext_line2;
        public List<float> timer;

        public CutsceneData(
            ref List<Texture2D> textures, 
            ref List<string> innertext_line1, 
            ref List<string> innertext_line2, 
            ref List<float> timer)
        {
            this.textures = textures;
            this.innertext_line1 = innertext_line1;
            this.innertext_line2 = innertext_line2;
            this.timer = timer;
        }
    }

    public static class Cutscenes_library
    {

        
    }
}
